using LiteNetLib.Utils;
namespace Shared.Code.Packets;

public class PeerEvent : IPacket {
    public enum PeerEventType {
        PeerDisconnect,
        PeerConnect,
        PlayerList
    }

    public PeerEventType Type;

    public void Serialize(NetDataWriter writer) {
        writer.Put((byte)Type);
    }

    public void Deserialize(NetDataReader reader) {
        Type = (PeerEventType)reader.GetByte();
    }

    public void Process(WorldState worldState) { }

    public PacketType GetPacketType() => PacketType.PeerEvent;

    public int SourceId { get; init; }
    public ushort TickNum { get; init; }
}