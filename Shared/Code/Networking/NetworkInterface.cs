using System.Net;
using LiteNetLib;
using LiteNetLib.Utils;
using Shared.Code.Packets;
namespace Shared.Code.Networking;

public abstract class NetworkInterface {
    protected readonly EventBasedNetListener Listener = new();
    protected readonly WorldState LocalWorldState;
    protected readonly NetManager Manager;

    protected NetworkInterface(WorldState worldState) {
        LocalWorldState = worldState;
        LocalTickBuffer = new TickBuffer(0, LocalWorldState);
        Manager = new NetManager(Listener);

        Listener.NetworkReceiveEvent += NetworkReceiveEvent;
    }

    public TickBuffer LocalTickBuffer { get; protected set; }
    public NetPeer? Server { get; private set; }

    public void StartServer(int port) {
        Manager.Start(port);
    }

    public void Connect(IPEndPoint ipEndPoint, string key = "testKey") {
        if (Server != null) {
            return;
        }
        Manager.Start();
        Server = Manager.Connect(ipEndPoint, key);
    }

    public void Stop() {
        Manager.Stop();
    }

    public void Update() {
        Manager.PollEvents();
        LocalTickBuffer.IncrCurrTick(LocalWorldState);
    }

    private void NetworkReceiveEvent(NetPeer peer, NetDataReader packetReader, byte channelNumber,
                                     DeliveryMethod deliveryMethod) {
        PacketType packetType = (PacketType)packetReader.GetByte();
        ushort tickNum = packetReader.GetUShort();
        int sourceId = packetReader.GetInt();

        HandlePackets(packetReader, sourceId, packetType, tickNum, peer);
    }

    protected abstract void HandlePackets(NetDataReader packetReader, int sourceId, PacketType packetType,
                                          ushort tickNum, NetPeer peer);

    protected abstract void HandleEvent(PeerEvent peerEvent);

    protected T HandlePacket<T>(NetDataReader packetReader, int sourceId, ushort tickNum) where T : IPacket, new() {
        T packet = new() { SourceId = sourceId, TickNum = tickNum };
        packet.Deserialize(packetReader);
        return packet;
    }

    public void SendPacket(IPacket packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) {
        if (Server == null) {
            SendPacketAllPeers(packet);
            return;
        }
        SendPacket(packet, Server, deliveryMethod);
    }

    public void SendPacket(IPacket packet, NetPeer target,
                           DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) {
        NetDataWriter writer = new();

        writer.Put((byte)packet.GetPacketType());
        writer.Put(packet.TickNum);
        writer.Put(packet.SourceId);
        writer.Put(packet);
        target.Send(writer, deliveryMethod);
    }

    public void SendPacketAllPeers(IPacket packet, NetPeer? source = null,
                                   DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) {
        NetDataWriter writer = new();

        writer.Put((byte)packet.GetPacketType());
        writer.Put(packet.TickNum);
        writer.Put(packet.SourceId);
        writer.Put(packet);
        Manager.SendToAll(writer, deliveryMethod, source); // Send to all clients except the source
    }
}