using LiteNetLib;
using LiteNetLib.Utils;

namespace Shared.Networking
{
    /// <summary>
    /// Packet for handling tile breaking. Planned to be used with <see cref="DeliveryMethod.ReliableUnordered"/>
    /// </summary>
    /// <remarks>
    /// Should be reworked when chunks are added.
    /// </remarks>
    public class BreakTile : Packet
    {
        public ushort tickNum;
        public Vector2Int position;
        public bool foreground;
        public ushort GetTickNum()
        {
            return tickNum;
        }
        public void Process()
        {
            BuildSystem.BreakBlockCell(foreground, position);
        }
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(tickNum);
            writer.Put(position.X);
            writer.Put(position.Y);
            writer.Put(foreground);
        }

        public void Deserialize(NetDataReader reader)
        {
            tickNum = reader.GetUShort();
            int x = reader.GetInt();
            int y = reader.GetInt();
            foreground = reader.GetBool();

            position = new(x, y);
        }
    }
}