using System.Security.Cryptography;

namespace Shared.Networking
{
    public class TickBuffer
    {
        private ushort currTick;
        private Tick[] tickBuffer = new Tick[GlobalsShared.MaxTicksStored];

        public TickBuffer(ushort newTick)
        {
            currTick = newTick;
        }
        /// <summary>
        /// Add additional tick
        /// </summary>
        public void IncrCurrTick(Tick newTick)
        {
            currTick++;
            if(currTick == GlobalsShared.MaxTicksStored)
            {
                currTick = 0;
            }
            tickBuffer[currTick] = newTick;
        }

        public void AddPacket(ushort tickNum, Packet newPacket)
        {
            
        }
    }
}