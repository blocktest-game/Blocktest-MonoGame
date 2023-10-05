using LiteNetLib;
using LiteNetLib.Utils;
using Shared.Code.Networking;
using Shared.Code.Packets;
namespace Blocktest.Networking;

public sealed class Client {
    private readonly EventBasedNetListener _listener;
    private readonly NetManager _manager;
    private NetPeer? _server;
    public TickBuffer ClientTickBuffer = new(0);

    public Client() {
        _listener = new EventBasedNetListener();
        _manager = new NetManager(_listener);
        _listener.NetworkReceiveEvent += NetworkRecieveEvent;
        _manager.Start();
    }

    public void Start(string ip, int port, string key) {
        _manager.Connect(ip, port, key);
    }

    public void Stop() {
        _manager.Stop();
    }

    public void Update() {
        _manager.PollEvents();
    }

    /// <summary>
    ///     Recieve network events from LiteNetLib
    /// </summary>
    /// <param name="peer">The server the packet is coming from.</param>
    /// <param name="packetReader">Contains the packet from the server.</param>
    /// <param name="channelNumber"></param>
    /// <param name="deliveryMethod">The delivery method used to deliver this packet.</param>
    protected void NetworkRecieveEvent(NetPeer peer, NetPacketReader packetReader, byte channelNumber,
                                       DeliveryMethod deliveryMethod) {
        if (_server == null) {
            _server = peer;
            WorldDownload worldPacket = new();
            byte packetByte = packetReader.GetByte();
            PacketType packetType = (PacketType)packetByte;
            if (packetType != PacketType.WorldDownload) {
                return;
            }
            worldPacket.Deserialize(packetReader);
            ClientTickBuffer = new TickBuffer(worldPacket.TickNum);
            ClientTickBuffer.AddPacket(worldPacket);
        } else {
            HandlePackets(packetReader);
        }
    }

    /// <summary>
    ///     Handles packets after the first.
    /// </summary>
    /// <param name="packetReader">Contains the packet sent by the server.</param>
    public void HandlePackets(NetPacketReader packetReader) {
        byte packetByte = packetReader.GetByte();
        PacketType packetType = (PacketType)packetByte;
        switch (packetType) {
            case PacketType.TileChange:
                HandleTileChange(packetReader);
                break;
            case PacketType.BreakTile:
                HandleBreakTile(packetReader);
                break;
            default:
                Console.WriteLine("Bad packet!!!");
                break;
        }
    }

    private void HandleTileChange(NetPacketReader packetReader) {
        TileChange tileChange = new();
        tileChange.Deserialize(packetReader);
        ClientTickBuffer.AddPacket(tileChange);
    }

    private void HandleBreakTile(NetPacketReader packetReader) {
        BreakTile breakTile = new();
        breakTile.Deserialize(packetReader);
        ClientTickBuffer.AddPacket(breakTile);
    }

    public void SendTileChange(TileChange tileChange) {
        NetDataWriter writer = new();
        writer.Put((byte)PacketType.TileChange);
        writer.Put(tileChange);
        _server?.Send(writer, DeliveryMethod.ReliableUnordered);
    }

    public void SendBreakTile(BreakTile breakTile) {
        NetDataWriter writer = new();
        writer.Put((byte)PacketType.BreakTile);
        writer.Put(breakTile);
        _server?.Send(writer, DeliveryMethod.ReliableUnordered);
    }
}