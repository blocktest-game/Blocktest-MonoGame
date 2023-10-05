using System.Collections.Generic;
using Shared.Code.Block_System;
using Shared.Code.Packets;
namespace Shared.Code.Networking;

public sealed class Tick {
    private readonly TilemapShared _background;

    /// <summary>
    ///     Time at the beginning of the tick.
    /// </summary>
    //public long time;
    private readonly TilemapShared _foreground;

    public readonly List<IPacket> Packets;

    public Tick(TilemapShared newForeground, TilemapShared newBackground) {
        _foreground = newForeground;
        _background = newBackground;
        Packets = new List<IPacket>();
    }

    /// <summary>
    ///     Process all packets taking action on this tick.
    /// </summary>
    public void ProcessStartTick() {
        Array.Copy(_foreground.TileGrid, GlobalsShared.ForegroundTilemap.TileGrid,
            GlobalsShared.MaxX * GlobalsShared.MaxY);
        Array.Copy(_background.TileGrid, GlobalsShared.BackgroundTilemap.TileGrid,
            GlobalsShared.MaxX * GlobalsShared.MaxY);
        foreach (IPacket packet in Packets) {
            packet.Process();
        }
    }

    public void ProcessTick() {
        Array.Copy(GlobalsShared.ForegroundTilemap.TileGrid, _foreground.TileGrid,
            GlobalsShared.MaxX * GlobalsShared.MaxY);
        Array.Copy(GlobalsShared.BackgroundTilemap.TileGrid, _background.TileGrid,
            GlobalsShared.MaxX * GlobalsShared.MaxY);
        foreach (IPacket packet in Packets) {
            packet.Process();
        }
    }
}