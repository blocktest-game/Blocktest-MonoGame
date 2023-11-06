using LiteNetLib.Utils;
namespace Shared.Code.Packets;

public sealed class MovePlayer : IPacket {
    public Vector2Int Position { get; set; }
    public ushort TickNum { get; init; }

    public int SourceId { get; init; }

    public void Serialize(NetDataWriter writer) {
        writer.Put(Position);
    }

    public void Deserialize(NetDataReader reader) {
        Position = reader.Get<Vector2Int>();
    }

    public void Process(WorldState worldState) {
        worldState.PlayerPositions[SourceId].Position = Position + new Vector2Int(256, 128);
    }

    public PacketType GetPacketType() => PacketType.MovePlayer;
}