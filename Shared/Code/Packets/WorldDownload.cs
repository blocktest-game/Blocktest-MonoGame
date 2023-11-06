using LiteNetLib;
using LiteNetLib.Utils;
namespace Shared.Code.Packets;

/// <summary>
///     Packet for handling initial world download. Should be used with <see cref="DeliveryMethod.ReliableOrdered" />
/// </summary>
/// <remarks>
///     Should be replaced when chunks are added.
/// </remarks>
public sealed class WorldDownload : IPacket {
    public string?[,,] World = new string[GlobalsShared.MaxX, GlobalsShared.MaxY, 2];
    public ushort TickNum { get; init; }

    public PacketType GetPacketType() => PacketType.WorldDownload;

    public int SourceId { get; init; }

    public void Process(WorldState worldState) {
        for (int x = 0; x < World.GetLength(0); x++)
        for (int y = 0; y < World.GetLength(1); y++) {
            worldState.Foreground.SetBlock(new Vector2Int(x, y), World[x, y, 1] ?? "air");
            worldState.Background.SetBlock(new Vector2Int(x, y), World[x, y, 0] ?? "air");
        }
    }

    public void Serialize(NetDataWriter writer) {
        for (int x = 0; x < GlobalsShared.MaxX; x++) {
            for (int y = 0; y < GlobalsShared.MaxY; y++) {
                for (int z = 0; z < 2; z++) {
                    writer.Put(World[x, y, z] ?? "air");
                }
            }
        }
    }

    public void Deserialize(NetDataReader reader) {
        for (int x = 0; x < GlobalsShared.MaxX; x++) {
            for (int y = 0; y < GlobalsShared.MaxY; y++) {
                for (int z = 0; z < 2; z++) {
                    World[x, y, z] = reader.GetString();
                }
            }
        }
    }

    public static WorldDownload Default() {
        string?[,,] newWorld = new string[GlobalsShared.MaxX, GlobalsShared.MaxY, 2];

        for (int i = 0; i < GlobalsShared.MaxX; i++) {
            newWorld[i, 0, 1] = "stone";
            newWorld[i, 1, 1] = "dirt";
            newWorld[i, 2, 1] = "dirt";
            newWorld[i, 3, 1] = "dirt";
            newWorld[i, 4, 1] = "dirt";
            newWorld[i, 5, 1] = "grass";
        }

        return new WorldDownload {
            World = newWorld,
            TickNum = 1
        };
    }
}