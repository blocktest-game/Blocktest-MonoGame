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
    public void ProcessStartTick(WorldState worldState) {
        //Array.Copy(_foreground.TileGrid, worldState.Foreground.TileGrid,
        // GlobalsShared.MaxX * GlobalsShared.MaxY);
        //Array.Copy(_background.TileGrid, worldState.Background.TileGrid,
        //GlobalsShared.MaxX * GlobalsShared.MaxY);
        foreach (IPacket packet in Packets) {
            packet.Process(worldState);
        }
    }

    public void ProcessTick(WorldState worldState) {
        //Array.Copy(worldState.Foreground.TileGrid, _foreground.TileGrid,
        //  GlobalsShared.MaxX * GlobalsShared.MaxY);
        //Array.Copy(worldState.Background.TileGrid, _background.TileGrid,
        // GlobalsShared.MaxX * GlobalsShared.MaxY);
        foreach (IPacket packet in Packets) {
            packet.Process(worldState);
        }
    }
}