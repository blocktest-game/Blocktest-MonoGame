using System.Collections;
using LiteNetLib;
namespace DedicatedServer.Code.Networking;

public sealed class ServerPlayerManager : IEnumerable<NetPeer> {
    public readonly Dictionary<int, NetPeer> PlayerList;
    public int PlayerCount;

    public ServerPlayerManager(int maxPlayers) {
        PlayerList = new Dictionary<int, NetPeer>();
        PlayerList.EnsureCapacity(maxPlayers);
    }

    public IEnumerator<NetPeer> GetEnumerator() {
        return PlayerList.Select(player => player.Value).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void AddPlayer(NetPeer newPlayer) {
        PlayerList.Add(newPlayer.Id, newPlayer);
        PlayerCount++;
    }

    public void RemovePlayer(NetPeer player) {
        PlayerList.Remove(player.Id);
        PlayerCount--;
    }
}