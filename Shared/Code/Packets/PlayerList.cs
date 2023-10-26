using System.Linq;
using LiteNetLib.Utils;
using Shared.Code.Components;
namespace Shared.Code.Packets;

public class PlayerList : IPacket {
    public int[] PlayerIds;

    public void Serialize(NetDataWriter writer) {
        writer.PutArray(PlayerIds);
    }

    public void Deserialize(NetDataReader reader) {
        PlayerIds = reader.GetIntArray();
    }

    public void Process(WorldState worldState) {
        foreach (var playerPosition in worldState.PlayerPositions) {
            if (!PlayerIds.Contains(playerPosition.Key)) {
                worldState.PlayerPositions.Remove(playerPosition.Key);
            }
        }

        foreach (int playerId in PlayerIds) {
            if (!worldState.PlayerPositions.ContainsKey(playerId)) {
                worldState.PlayerPositions.Add(playerId, new Transform(new Vector2Int(256, 128)));
            }
        }
    }

    public PacketType GetPacketType() => PacketType.PlayerList;

    public int SourceId { get; init; }
    public ushort TickNum { get; init; }
}