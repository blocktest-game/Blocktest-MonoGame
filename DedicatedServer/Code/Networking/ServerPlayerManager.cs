using LiteNetLib;
using LiteNetLib.Utils;
using System.Collections;
using System.Collections.Generic;

namespace Blocktest.Networking
{
    public class ServerPlayerManager : IEnumerable<NetPeer>
    {
        private NetPeer[] playerList;
        public int playerCount;
        private int playerNum = 0;

        public ServerPlayerManager(int maxPlayers)
        {
            playerList = new NetPeer[maxPlayers];
        }

        public IEnumerator<NetPeer> GetEnumerator()
        {
            for(int i = 0; i < playerCount; i++)
            {
                yield return playerList[i];
            }
        }

        public void addPlayer(NetPeer newPlayer)
        {
            playerList[playerNum] = newPlayer;
            playerNum++;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}