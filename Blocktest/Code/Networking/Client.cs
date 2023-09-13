using LiteNetLib;
using LiteNetLib.Utils;

namespace Blocktest.Networking
{
    public class Client
    {
        private EventBasedNetListener listener;

        private NetManager manager;
        private NetPeer? server;

        public Client(string ip, int port, string key)
        {
            listener = new();
            manager = new(listener);
            manager.Connect(ip, port, key);
        }

        protected void networkRecieveEvent(NetPeer peer, NetPacketReader packerReader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            if(server == null)
            {
                server = peer;
            }
        }
    }
}