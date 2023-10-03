using System.Security.Cryptography;

namespace Shared.Networking
{
    public class TickBuffer
    {
        public ushort currTick;
        private int currentDistance;
        private ushort currentRecent;
        private Tick[] tickBuffer = new Tick[GlobalsShared.MaxTicksStored];

        public TickBuffer(ushort newTick)
        {
            currTick = newTick;
            for(int i = 0; i < GlobalsShared.MaxTicksStored; i++)
            {
                tickBuffer[i] = new(GlobalsShared.ForegroundTilemap, GlobalsShared.BackgroundTilemap);
            }
            currentDistance = 0;
            currentRecent = currTick;
        }
        /// <summary>
        /// Add additional tick
        /// </summary>
        public void IncrCurrTick()
        {
            //tickBuffer[currTick].ProcessStartTick();
            ProcessTicks(currentRecent);
            Tick newTick = new(GlobalsShared.ForegroundTilemap, GlobalsShared.BackgroundTilemap);
            currTick++;
            if(currTick == GlobalsShared.MaxTicksStored)
            {
                currTick = 0;
            }
            tickBuffer[currTick] = newTick;
            currentDistance = 0;
            currentRecent = currTick;
        }

        private bool CheckFurthestTick(ushort newTickNum)
        {
            int newTickDistance;
            if(newTickNum > currTick)
            {
                newTickDistance = GlobalsShared.MaxTicksStored - newTickNum + currTick;
            }
            else
            {
                newTickDistance = currTick - newTickNum;
            }
            if(newTickDistance > currentDistance)
            {
                currentDistance = newTickDistance;
                currentRecent = newTickNum;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ProcessTicks(ushort startTick)
        {
            Tick tick = tickBuffer[startTick];
            tick.ProcessStartTick();
            for(int i = startTick + 1; i != (currTick + 1); i++)
            {
                if(i == GlobalsShared.MaxTicksStored)
                {
                    i = 0;
                }
                tick = tickBuffer[i];
                tick.ProcessTick();
            }
        }

        public void AddPacket(Packet newPacket)
        {
            CheckFurthestTick(newPacket.GetTickNum());
            ushort tickNum = newPacket.GetTickNum();
            if(tickBuffer[tickNum] == null)
            {
                tickBuffer[tickNum] = new(GlobalsShared.ForegroundTilemap, GlobalsShared.BackgroundTilemap);
            }
            tickBuffer[tickNum].packets.Add(newPacket);
        }
    }
}