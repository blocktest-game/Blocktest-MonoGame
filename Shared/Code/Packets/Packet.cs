using LiteNetLib.Utils;
namespace Shared.Code.Packets;

public enum PacketType : byte {
    WorldDownload,
    BreakTile,
    TileChange
}

public interface IPacket : INetSerializable {
    /// <summary>
    ///     Handles all processing for the packet.
    /// </summary>
    public void Process();

    /// <summary>
    ///     Returns the tick number that the simulation must rewind to.
    /// </summary>
    /// <returns>The number of the tick this packet applies to.</returns>
    public ushort GetTickNum();
}