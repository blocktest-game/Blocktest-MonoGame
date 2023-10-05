using System.Collections;
using LiteNetLib;
namespace DedicatedServer.Code.Networking;

public sealed class ServerPlayerManager : IEnumerable<NetPeer> {
    private readonly Dictionary<int, NetPeer> _playerList;
    public int PlayerCount;

    public ServerPlayerManager(int maxPlayers) {
        _playerList = new Dictionary<int, NetPeer>();
        _playerList.EnsureCapacity(maxPlayers);
    }

    public IEnumerator<NetPeer> GetEnumerator() {
        return _playerList.Select(player => player.Value).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void AddPlayer(NetPeer newPlayer) {
        _playerList.Add(newPlayer.Id, newPlayer);
        PlayerCount++;
    }

    public void RemovePlayer(NetPeer player) {
        _playerList.Remove(player.Id);
        PlayerCount--;
    }
}