using LiteNetLib;
using LiteNetLib.Utils;
using Shared.Networking;

namespace Blocktest.Networking
{
    public class Client
    {
        private EventBasedNetListener listener;
        private NetManager manager;
        private NetPeer? server;
        public TickBuffer clientTickBuffer = new(0);

        public Client()
        {
            listener = new();
            manager = new(listener);
            listener.NetworkReceiveEvent += NetworkRecieveEvent;
            manager.Start();
        }

        public void Start(string ip, int port, string key)
        {
            manager.Connect(ip, port, key);
        }

        public void Update()
        {
            manager.PollEvents();
        }

        /// <summary>
        /// Recieve network events from LiteNetLib
        /// </summary>
        /// <param name="peer">The server the packet is coming from.</param>
        /// <param name="packetReader">Contains the packet from the server.</param>
        /// <param name="channelNumber"></param>
        /// <param name="deliveryMethod">The delivery method used to deliver this packet.</param>
        protected void NetworkRecieveEvent(NetPeer peer, NetPacketReader packetReader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            if(server == null)
            {
                server = peer;
                WorldDownload worldPacket = new();
                byte packetByte = packetReader.GetByte();
                PacketType packetType = (PacketType)packetByte;
                if (packetType != PacketType.WorldDownload) {
                    return;
                }
                worldPacket.Deserialize(packetReader); 
                clientTickBuffer = new(worldPacket.GetTickNum());
                clientTickBuffer.AddPacket(worldPacket);
            }
            else
            {
                HandlePackets(packetReader);
            }
        }

        /// <summary>
        /// Handles packets after the first.
        /// </summary>
        /// <param name="packetReader">Contains the packet sent by the server.</param>
        public void HandlePackets(NetPacketReader packetReader)
        {
            byte packetByte = packetReader.GetByte();
            PacketType packetType = (PacketType)packetByte;
            switch (packetType)
            {
                case PacketType.TileChange:
                    HandleTileChange(packetReader);
                    break;
                case PacketType.BreakTile:
                    HandleBreakTile(packetReader);
                    break;
                default:
                    Console.WriteLine("Bad packet!!!");
                    break;
            }
        }

        private void HandleTileChange(NetPacketReader packetReader)
        {
            TileChange tileChange = new();
            tileChange.Deserialize(packetReader);
            clientTickBuffer.AddPacket(tileChange);
        }

        private void HandleBreakTile(NetPacketReader packetReader)
        {
            BreakTile breakTile = new();
            breakTile.Deserialize(packetReader);
            clientTickBuffer.AddPacket(breakTile);
        }

        public void SendTileChange(TileChange tileChange)
        {
            NetDataWriter writer = new();
            writer.Put((byte)PacketType.TileChange);
            writer.Put(tileChange);
            server?.Send(writer, DeliveryMethod.ReliableUnordered);
        }

        public void SendBreakTile(BreakTile breakTile)
        {
            NetDataWriter writer = new();
            writer.Put((byte)PacketType.BreakTile);
            writer.Put(breakTile);
            server?.Send(writer, DeliveryMethod.ReliableUnordered);
        }
    }
}