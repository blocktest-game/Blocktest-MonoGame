using LiteNetLib.Utils;
namespace Shared.Code.Packets;

public enum PacketType : byte {
    WorldDownload,
    BreakTile,
    TileChange,
    MovePlayer,
    PlayerList,
    PeerEvent
}

public interface IPacket : INetSerializable {
    public int SourceId { get; init; }

    public ushort TickNum { get; init; }

    /// <summary>
    ///     Handles all processing for the packet.
    /// </summary>
    public void Process(WorldState worldState);

    /// <summary>
    ///     Returns the type of packet.
    /// </summary>
    public PacketType GetPacketType();
}