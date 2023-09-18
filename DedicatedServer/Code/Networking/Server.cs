using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;
using Shared;
using Shared.Networking;

namespace Blocktest.Networking
{
    public class Server
    {
        private EventBasedNetListener listener = new();
        private NetManager manager;
        private ServerPlayerManager playerManager;

        public Server()
        {
            manager = new(listener);
            playerManager = new(GlobalsShared.MaxPlayers);
            listener.ConnectionRequestEvent += NewConnection;
            listener.PeerConnectedEvent += NewPeer;
        }

        public void Start()
        {
            manager.Start(9050);
        }
        public void Update()
        {
            manager.PollEvents();
        }

        protected void NewConnection(ConnectionRequest request)
        {
            Console.WriteLine("New Connection");
            if(playerManager.playerCount < GlobalsShared.MaxPlayers)
            {
                request.AcceptIfKey("testKey");
            }
            else
            {
                request.Reject();
            }
        }
        
        protected void NewPeer(NetPeer peer)
        {
            Console.WriteLine("New Peer");
            playerManager.addPlayer(peer);
            NetDataWriter writer = new();
            WorldDownload worldDownload = new();
            worldDownload.world = BuildSystem.getCurrentWorld();
            worldDownload.tickNum = GlobalsServer.serverTickBuffer.currTick;
            writer.Put(worldDownload);
            peer.Send(writer, DeliveryMethod.ReliableOrdered);
        }
    }
}