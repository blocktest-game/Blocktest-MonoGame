using System.Collections.Generic;

namespace Shared.Networking
{
    public class Tick
    {
        /// <summary>
        /// Time at the beginning of the tick.
        /// </summary>
        //public long time;
        private readonly TilemapShared _foreground;

        private readonly TilemapShared _background;
        public readonly List<Packet> Packets;

        public Tick(TilemapShared newForeground, TilemapShared newBackground)
        {
            _foreground = newForeground;
            _background = newBackground;
            Packets = new();
        }

        /// <summary>
        /// Process all packets taking action on this tick.
        /// </summary>
        public void ProcessStartTick()
        {
            Array.Copy(_foreground.tileGrid, GlobalsShared.ForegroundTilemap.tileGrid, GlobalsShared.maxX * GlobalsShared.maxY);
            Array.Copy(_background.tileGrid, GlobalsShared.BackgroundTilemap.tileGrid, GlobalsShared.maxX * GlobalsShared.maxY);
            foreach(Packet packet in Packets)
            {
                packet.Process();
            }
        }
        
        public void ProcessTick()
        {
            Array.Copy(GlobalsShared.ForegroundTilemap.tileGrid, _foreground.tileGrid, GlobalsShared.maxX * GlobalsShared.maxY);
            Array.Copy(GlobalsShared.BackgroundTilemap.tileGrid, _background.tileGrid, GlobalsShared.maxX * GlobalsShared.maxY);
            foreach(Packet packet in Packets)
            {
                packet.Process();
            }
        }
    }
}