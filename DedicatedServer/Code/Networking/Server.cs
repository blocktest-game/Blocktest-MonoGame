using LiteNetLib;
using LiteNetLib.Utils;
using Shared;
using Shared.Code;
using Shared.Code.Networking;
using Shared.Code.Packets;
namespace DedicatedServer.Code.Networking;

public sealed class Server {
    private readonly EventBasedNetListener _listener = new();
    private readonly NetManager _manager;
    private readonly ServerPlayerManager _playerManager;
    public readonly TickBuffer ServerTickBuffer = new(0);

    public Server() {
        _manager = new NetManager(_listener);
        _playerManager = new ServerPlayerManager(GlobalsShared.MaxPlayers);

        _listener.ConnectionRequestEvent += NewConnection;
        _listener.PeerConnectedEvent += NewPeer;
        _listener.NetworkReceiveEvent += NetworkReceiveEvent;
        _listener.PeerDisconnectedEvent += PeerDisconnected;
    }

    public void Start() {
        _manager.Start(9050);
    }

    public void Update() {
        _manager.PollEvents();
    }

    private void NewConnection(ConnectionRequest request) {
        Console.WriteLine("New Connection");
        if (_playerManager.PlayerCount < GlobalsShared.MaxPlayers) {
            request.AcceptIfKey("testKey");
        } else {
            request.Reject();
        }
    }

    /// <summary>
    ///     Adds a new player to the playerManager and sends them the current world.
    /// </summary>
    /// <param name="peer">The new player</param>
    private void NewPeer(NetPeer peer) {
        Console.WriteLine("New Peer");
        _playerManager.AddPlayer(peer);
        NetDataWriter writer = new();
        WorldDownload worldDownload = new() {
            World = BuildSystem.CurrentWorld,
            TickNum = ServerTickBuffer.CurrTick
        };
        writer.Put((byte)PacketType.WorldDownload);
        writer.Put(worldDownload);
        peer.Send(writer, DeliveryMethod.ReliableOrdered);
    }

    /// <summary>
    ///     Removes a player from the playerManager.
    /// </summary>
    /// <param name="peer">The player to remove</param>
    /// <param name="disconnectInfo">The reason the player disconnected</param>
    private void PeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
        Console.WriteLine($"Peer Disconnected: {disconnectInfo.Reason}");
        _playerManager.RemovePlayer(peer);
    }

    /// <summary>
    ///     Receive network events from LiteNetLib
    /// </summary>
    /// <param name="peer">The client the packet is coming from.</param>
    /// <param name="packetReader">Contains the packet from the client.</param>
    /// <param name="channelNumber"></param>
    /// <param name="deliveryMethod">The delivery method used to deliver this packet.</param>
    private void NetworkReceiveEvent(NetPeer peer, NetPacketReader packetReader, byte channelNumber,
                                     DeliveryMethod deliveryMethod) {
        byte packetByte = packetReader.GetByte();
        PacketType packetType = (PacketType)packetByte;
        switch (packetType) {
            case PacketType.TileChange:
                HandleTileChange(packetReader, peer);
                break;
            case PacketType.BreakTile:
                HandleBreakTile(packetReader, peer);
                break;
            case PacketType.WorldDownload:
            default:
                Console.WriteLine("Bad packet!!!");
                break;
        }
    }

    private void HandleTileChange(NetPacketReader packetReader, NetPeer peer) {
        TileChange tileChange = new();
        tileChange.Deserialize(packetReader);
        ServerTickBuffer.AddPacket(tileChange);

        NetDataWriter writer = new();
        writer.Put((byte)PacketType.TileChange);
        writer.Put(tileChange);
        _manager.SendToAll(writer, DeliveryMethod.ReliableUnordered,
            peer); // For now, just exclude the one who sent it.
    }

    private void HandleBreakTile(NetPacketReader packetReader, NetPeer peer) {
        BreakTile breakTile = new();
        breakTile.Deserialize(packetReader);
        ServerTickBuffer.AddPacket(breakTile);
        NetDataWriter writer = new();
        writer.Put((byte)PacketType.BreakTile);
        writer.Put(breakTile);
        _manager.SendToAll(writer, DeliveryMethod.ReliableUnordered,
            peer); // For now, just exclude the one who sent it.
    }
}