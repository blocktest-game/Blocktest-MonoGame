using System.Collections.Generic;

namespace Shared.Networking
{
    public class Tick
    {
        /// <summary>
        /// Time at the beginning of the tick.
        /// </summary>
        //public long time;
        public TilemapShared foreground;
        public TilemapShared background;
        public List<Packet> packets;

        public Tick(TilemapShared newForeground, TilemapShared newBackground)
        {
            foreground = newForeground;
            background = newBackground;
            packets = new();
        }

        /// <summary>
        /// Process all packets taking action on this tick.
        /// </summary>
        public void ProcessStartTick()
        {
            Array.Copy(foreground.tileGrid, GlobalsShared.ForegroundTilemap.tileGrid, GlobalsShared.maxX * GlobalsShared.maxY);
            Array.Copy(background.tileGrid, GlobalsShared.BackgroundTilemap.tileGrid, GlobalsShared.maxX * GlobalsShared.maxY);
            foreach(Packet packet in packets)
            {
                packet.Process();
            }
        }
        public void ProcessTick()
        {
            Array.Copy(GlobalsShared.ForegroundTilemap.tileGrid, foreground.tileGrid, GlobalsShared.maxX * GlobalsShared.maxY);
            Array.Copy(GlobalsShared.BackgroundTilemap.tileGrid, background.tileGrid, GlobalsShared.maxX * GlobalsShared.maxY);
            foreach(Packet packet in packets)
            {
                packet.Process();
            }
        }
    }
}