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

        protected void NetworkRecieveEvent(NetPeer peer, NetPacketReader packetReader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            if(server == null)
            {
                Console.WriteLine("Got first packet");
                server = peer;
                WorldDownload worldPacket = new();
                worldPacket.Deserialize(packetReader);
                Packet[] starterPacket = {worldPacket};
                Globals.clientTickBuffer = new(worldPacket.GetTickNum());
                Globals.clientTickBuffer.AddPackets(starterPacket);
            }
            else
            {
                Console.WriteLine("Wrong packet");
            }
        }
    }
}