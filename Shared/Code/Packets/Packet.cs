using LiteNetLib;
using LiteNetLib.Utils;

namespace Shared.Networking
{
    public enum PacketType : byte
    {
        WorldDownload,
        BreakTile,
        TileChange
    }
    
    public interface Packet : INetSerializable
    {
        /// <summary>
        /// Handles all processing for the packet.
        /// </summary>
        public abstract void Process();

        /// <summary>
        /// Returns the tick number that the simulation must rewind to.
        /// </summary>
        /// <returns>The number of the tick this packet applies to.</returns>
        public abstract ushort GetTickNum();
    }

}