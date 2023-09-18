using System.Security.Cryptography;

namespace Shared.Networking
{
    public class TickBuffer
    {
        public ushort currTick;
        private int currentDistance;
        private Tick[] tickBuffer = new Tick[GlobalsShared.MaxTicksStored];

        public TickBuffer(ushort newTick)
        {
            currTick = newTick;
            /*for(int i = 0; i < GlobalsShared.MaxTicksStored; i++)
            {
                tickBuffer[i] = new(GlobalsShared.ForegroundTilemap, GlobalsShared.BackgroundTilemap);
            }*/
        }
        /// <summary>
        /// Add additional tick
        /// </summary>
        public void IncrCurrTick()
        {
            Tick newTick = new(GlobalsShared.ForegroundTilemap, GlobalsShared.BackgroundTilemap);
            currTick++;
            if(currTick == GlobalsShared.MaxTicksStored)
            {
                currTick = 0;
            }
            tickBuffer[currTick] = newTick;
        }
        
        public void AddPackets(Packet[] newPackets)
        {
            currentDistance = 600;
            ushort currentRecent = 0;
            foreach(Packet packet in newPackets)
            {
                if(CheckRecentTick(packet.GetTickNum()))
                {
                    currentRecent = packet.GetTickNum();
                }
                AddPacket(packet);
            }
            ProcessTicks(currentRecent);
        }

        private bool CheckRecentTick(ushort newTickNum)
        {
            int newTickDistance;
            if(newTickNum < currTick)
            {
                newTickDistance = GlobalsShared.MaxTicksStored - currTick + newTickNum;
            }
            else
            {
                newTickDistance = newTickNum - currTick;
            }
            if(newTickDistance < currentDistance)
            {
                currentDistance = newTickDistance;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ProcessTicks(ushort startTick)
        {
            //if(startTick == currTick){return;}
            Tick tick = tickBuffer[startTick];
            tick.ProcessStartTick();
            for(int i = startTick + 1; i != (currTick + 1); i++)        // We want to process ticks if we start at a ticknum higher than the current
            {
                if(i == GlobalsShared.MaxTicksStored)
                {
                    i = 0;
                }
                tick = tickBuffer[i];
                tick.ProcessTick();
            }
        }

        private void AddPacket(Packet newPacket)
        {
            Console.WriteLine("Begin AddPacket");
            ushort tickNum = newPacket.GetTickNum();
            Console.WriteLine("Got ticknum");
            if(tickBuffer[tickNum] == null)
            {
                tickBuffer[tickNum] = new(GlobalsShared.ForegroundTilemap, GlobalsShared.BackgroundTilemap);
            }
            tickBuffer[tickNum].packets.Add(newPacket);
            Console.WriteLine("End AddPacket");
        }
    }
}