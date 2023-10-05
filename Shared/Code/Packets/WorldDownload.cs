using LiteNetLib;
using LiteNetLib.Utils;

namespace Shared.Networking
{
    /// <summary>
    /// Packet for handling initial world download. Should be used with <see cref="DeliveryMethod.ReliableOrdered"/>
    /// </summary>
    /// <remarks>
    /// Should be replaced when chunks are added.
    /// </remarks>
    public class WorldDownload : Packet
    {
        public ushort tickNum;
        public int[,,] world = new int[GlobalsShared.maxX, GlobalsShared.maxY, 2];
        public ushort GetTickNum()
        {
            return tickNum;
        }
        
        public void Process()
        {
            BuildSystem.LoadNewWorld(world);
        }
        
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(tickNum);
            for(int x = 0; x < GlobalsShared.maxX; x++)
            {
                for(int y = 0; y < GlobalsShared.maxY; y++)
                {
                    for(int z = 0; z < 2; z++)
                    {
                        writer.Put(world[x,y,z]);
                    }
                }
            }
        }

        public void Deserialize(NetDataReader reader)
        {
            tickNum = reader.GetUShort();
            for(int x = 0; x < GlobalsShared.maxX; x++)
            {
                for(int y = 0; y < GlobalsShared.maxY; y++)
                {
                    for(int z = 0; z < 2; z++)
                    {
                        world[x,y,z] = reader.GetInt();
                    }
                }
            }
        }
    }
}