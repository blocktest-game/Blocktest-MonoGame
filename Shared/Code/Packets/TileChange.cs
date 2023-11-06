using LiteNetLib;
using LiteNetLib.Utils;
using Shared.Code.Block_System;
namespace Shared.Code.Packets;

/// <summary>
///     Packet for handling tile changes. Planned to be used with <see cref="DeliveryMethod.ReliableUnordered" />
/// </summary>
/// <remarks>
///     Should be reworked when chunks are added.
/// </remarks>
public sealed class TileChange : IPacket {
    public string BlockUid;
    public bool Foreground;
    public Vector2Int Position;

    public ushort TickNum { get; init; }

    public PacketType GetPacketType() => PacketType.TileChange;

    public int SourceId { get; init; }

    public void Process(WorldState worldState) {
        TilemapShared tilemap = Foreground ? worldState.Foreground : worldState.Background;
        tilemap.SetBlock(Position, BlockUid);
    }

    public void Serialize(NetDataWriter writer) {
        writer.Put(Position);
        writer.Put(Foreground);
        writer.Put(BlockUid);
    }

    public void Deserialize(NetDataReader reader) {
        Position = reader.Get<Vector2Int>();
        Foreground = reader.GetBool();
        BlockUid = reader.GetString();
    }
}