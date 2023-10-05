using LiteNetLib;
using LiteNetLib.Utils;
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
    public ushort TickNum;

    public ushort GetTickNum() => TickNum;

    public void Process() {
        BuildSystem.BreakBlockCell(Foreground, Position);
    }

    public void Serialize(NetDataWriter writer) {
        writer.Put(TickNum);
        writer.Put(Position.X);
        writer.Put(Position.Y);
        writer.Put(Foreground);
    }

    public void Deserialize(NetDataReader reader) {
        TickNum = reader.GetUShort();
        int x = reader.GetInt();
        int y = reader.GetInt();
        Foreground = reader.GetBool();

        Position = new Vector2Int(x, y);
    }
}