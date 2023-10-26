using LiteNetLib;
using LiteNetLib.Utils;
using Shared;
using Shared.Code;
using Shared.Code.Components;
using Shared.Code.Networking;
using Shared.Code.Packets;
namespace DedicatedServer.Code.Networking;

public sealed class Server {
    private readonly EventBasedNetListener _listener = new();
    private readonly NetManager _manager;
    private readonly ServerPlayerManager _playerManager;
    private readonly WorldState _worldState;
    public readonly TickBuffer ServerTickBuffer;

    public Server(WorldState worldState) {
        _worldState = worldState;
        ServerTickBuffer = new TickBuffer(0, _worldState);
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
        _worldState.PlayerPositions.Add(peer.Id, new Transform(Vector2Int.Zero));

        WorldDownload worldDownload = new() {
            World = _worldState.CurrentWorld,
            TickNum = ServerTickBuffer.CurrTick,
            SourceId = -1
        };
        SendPacket(worldDownload, peer, DeliveryMethod.ReliableOrdered);

        SendPacketAllPeers(new PeerEvent {
            SourceId = peer.Id,
            TickNum = ServerTickBuffer.CurrTick,
            Type = PeerEvent.PeerEventType.PeerConnect
        }, peer);
    }

    /// <summary>
    ///     Removes a player from the playerManager.
    /// </summary>
    /// <param name="peer">The player to remove</param>
    /// <param name="disconnectInfo">The reason the player disconnected</param>
    private void PeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
        Console.WriteLine($"Peer Disconnected: {disconnectInfo.Reason}");
        _playerManager.RemovePlayer(peer);
        _worldState.PlayerPositions.Remove(peer.Id);

        SendPacketAllPeers(new PeerEvent {
            SourceId = peer.Id,
            TickNum = ServerTickBuffer.CurrTick,
            Type = PeerEvent.PeerEventType.PeerDisconnect
        }, peer);
    }

    /// <summary>
    ///     Receive network events from LiteNetLib
    /// </summary>
    /// <param name="peer">The client the packet is coming from.</param>
    /// <param name="packetReader">Contains the packet from the client.</param>
    /// <param name="channelNumber"></param>
    /// <param name="deliveryMethod">The delivery method used to deliver this packet.</param>
    /// ACCEPTS PacketType:TickNum:Packet
    private void NetworkReceiveEvent(NetPeer peer, NetPacketReader packetReader, byte channelNumber,
                                     DeliveryMethod deliveryMethod) {
        byte packetByte = packetReader.GetByte();
        PacketType packetType = (PacketType)packetByte;
        switch (packetType) {
            case PacketType.TileChange:
                HandlePacket<TileChange>(packetReader, peer);
                break;
            case PacketType.BreakTile:
                HandlePacket<BreakTile>(packetReader, peer);
                break;
            case PacketType.MovePlayer:
                HandlePacket<MovePlayer>(packetReader, peer);
                break;
            case PacketType.PeerEvent:
                PeerEvent eventPacket = new() { SourceId = peer.Id, TickNum = packetReader.GetUShort() };
                eventPacket.Deserialize(packetReader);
                HandleEvent(eventPacket);
                break;
            case PacketType.WorldDownload:
            case PacketType.PlayerList:
            default:
                Console.WriteLine("Bad packet!!!");
                break;
        }
    }

    private void HandlePacket<T>(NetDataReader packetReader, NetPeer source) where T : IPacket, new() {
        T packet = new() { SourceId = source.Id, TickNum = packetReader.GetUShort() };
        packet.Deserialize(packetReader);
        ServerTickBuffer.AddPacket(packet);

        SendPacketAllPeers(packet, source);
    }

    private void HandleEvent(PeerEvent peerEvent) {
        switch (peerEvent.Type) {
            case PeerEvent.PeerEventType.PlayerList:
                Console.WriteLine("PlayerList requested");
                NetPeer? peer = _manager.GetPeerById(peerEvent.SourceId);
                foreach (var player in _playerManager.PlayerList) {
                    if (player.Key == peerEvent.SourceId) {
                        continue;
                    }
                    SendPacket(
                        new PeerEvent {
                            SourceId = player.Key, Type = PeerEvent.PeerEventType.PeerConnect,
                            TickNum = ServerTickBuffer.CurrTick
                        }, peer);
                }

                //SendPacket(new PlayerList { PlayerIds = _playerManager.PlayerList.Keys.ToArray(), TickNum = ServerTickBuffer.CurrTick }, peer);
                break;
            case PeerEvent.PeerEventType.PeerDisconnect:
            case PeerEvent.PeerEventType.PeerConnect:
            default:
                throw new ArgumentOutOfRangeException(nameof(peerEvent), peerEvent, null);
        }
    }

    // SENDS PacketType:SourceID:TickNum:Packet
    private void SendPacketAllPeers(IPacket packet, NetPeer source,
                                    DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) {
        NetDataWriter writer = new();
        writer.Put((byte)packet.GetPacketType());
        writer.Put(source.Id);
        writer.Put(packet.TickNum);
        writer.Put(packet);
        _manager.SendToAll(writer, deliveryMethod, source); // Send to all clients except the source
    }

    // SENDS PacketType:SourceID:TickNum:Packet
    private void SendPacket(IPacket packet, NetPeer target,
                            DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) {
        NetDataWriter writer = new();
        writer.Put((byte)packet.GetPacketType());
        writer.Put(packet.SourceId);
        writer.Put(packet.TickNum);
        writer.Put(packet);
        target.Send(writer, deliveryMethod);
    }
}