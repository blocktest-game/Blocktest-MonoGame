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
    public ushort TickNum;
    public string?[,,] World = new string[GlobalsShared.MaxX, GlobalsShared.MaxY, 2];

    public ushort GetTickNum() => TickNum;

    public void Process() {
        BuildSystem.LoadNewWorld(World);
    }

    public void Serialize(NetDataWriter writer) {
        writer.Put(TickNum);
        for (int x = 0; x < GlobalsShared.MaxX; x++) {
            for (int y = 0; y < GlobalsShared.MaxY; y++) {
                for (int z = 0; z < 2; z++) {
                    writer.Put(World[x, y, z]);
                }
            }
        }
    }

    public void Deserialize(NetDataReader reader) {
        TickNum = reader.GetUShort();
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