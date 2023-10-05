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
    public int BlockId;
    public bool Foreground;
    public Vector2Int Position;
    public ushort TickNum;

    public ushort GetTickNum() => TickNum;

    public void Process() {
        BuildSystem.PlaceBlockCell(BlockManagerShared.AllBlocks[BlockId], Foreground, Position);
    }

    public void Serialize(NetDataWriter writer) {
        writer.Put(TickNum);
        writer.Put(Position.X);
        writer.Put(Position.Y);
        writer.Put(Foreground);
        writer.Put(BlockId);
    }

    public void Deserialize(NetDataReader reader) {
        TickNum = reader.GetUShort();
        int x = reader.GetInt();
        int y = reader.GetInt();
        Foreground = reader.GetBool();
        BlockId = reader.GetInt();

        Position = new Vector2Int(x, y);
    }
}