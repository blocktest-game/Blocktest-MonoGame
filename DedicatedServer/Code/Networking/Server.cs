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
        public TickBuffer serverTickBuffer = new(0);

        public Server()
        {
            manager = new(listener);
            playerManager = new(GlobalsShared.MaxPlayers);
            listener.ConnectionRequestEvent += NewConnection;
            listener.PeerConnectedEvent += NewPeer;
            listener.NetworkReceiveEvent += NetworkRecieveEvent;
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
        
        /// <summary>
        /// Adds a new player to the playerManager and sends them the current world.
        /// </summary>
        /// <param name="peer">The new player</param>
        protected void NewPeer(NetPeer peer)
        {
            Console.WriteLine("New Peer");
            playerManager.addPlayer(peer);
            NetDataWriter writer = new();
            WorldDownload worldDownload = new()
            {
                world = BuildSystem.CurrentWorld,
                tickNum = serverTickBuffer.currTick
            };
            writer.Put((byte)PacketType.WorldDownload);
            writer.Put(worldDownload);
            peer.Send(writer, DeliveryMethod.ReliableOrdered);
        }

        /// <summary>
        /// Recieve network events from LiteNetLib
        /// </summary>
        /// <param name="peer">The client the packet is coming from.</param>
        /// <param name="packetReader">Contains the packet from the client.</param>
        /// <param name="channelNumber"></param>
        /// <param name="deliveryMethod">The delivery method used to deliver this packet.</param>
        private void NetworkRecieveEvent(NetPeer peer, NetPacketReader packetReader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            byte packetByte = packetReader.GetByte();
            PacketType packetType = (PacketType) packetByte;
            switch (packetType)
            {
                case PacketType.TileChange:
                    HandleTileChange(packetReader, peer);
                    break;
                case PacketType.BreakTile:
                    HandleBreakTile(packetReader, peer);
                    break;
                default:
                    Console.WriteLine("Bad packet!!!");
                    break;
            }
        }

        private void HandleTileChange(NetPacketReader packetReader, NetPeer peer)
        {
            TileChange tileChange = new();
            tileChange.Deserialize(packetReader);
            serverTickBuffer.AddPacket(tileChange);
            
            NetDataWriter writer = new();
            writer.Put((byte)PacketType.TileChange);
            writer.Put(tileChange);
            manager.SendToAll(writer, DeliveryMethod.ReliableUnordered, peer);      // For now, just exclude the one who sent it.
        }

        private void HandleBreakTile(NetPacketReader packetReader, NetPeer peer)
        {
            BreakTile breakTile = new();
            breakTile.Deserialize(packetReader);
            serverTickBuffer.AddPacket(breakTile);
            NetDataWriter writer = new();
            writer.Put((byte)PacketType.BreakTile);
            writer.Put(breakTile);
            manager.SendToAll(writer, DeliveryMethod.ReliableUnordered, peer);      // For now, just exclude the one who sent it.
        }
    }
}