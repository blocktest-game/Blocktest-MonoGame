using LiteNetLib;
using LiteNetLib.Utils;
using Shared.Code.Block_System;
namespace Shared.Code.Packets;

/// <summary>
///     Packet for handling tile breaking. Planned to be used with <see cref="DeliveryMethod.ReliableUnordered" />
/// </summary>
/// <remarks>
///     Should be reworked when chunks are added.
/// </remarks>
public sealed class BreakTile : IPacket {
    public bool Foreground;
    public Vector2Int Position;
    public ushort TickNum { get; init; }

    public PacketType GetPacketType() => PacketType.BreakTile;

    public int SourceId { get; init; }

    public void Process(WorldState worldState) {
        TilemapShared tilemap = Foreground ? worldState.Foreground : worldState.Background;
        tilemap.DeleteTile(Position);
    }

    public void Serialize(NetDataWriter writer) {
        writer.Put(Position);
        writer.Put(Foreground);
    }

    public void Deserialize(NetDataReader reader) {
        Position = reader.Get<Vector2Int>();
        Foreground = reader.GetBool();
    }
}