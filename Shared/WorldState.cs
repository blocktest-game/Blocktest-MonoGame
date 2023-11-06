using System.Collections.Generic;
using Shared.Code.Block_System;
using Shared.Code.Components;
namespace Shared;

public class WorldState {
    private const int MaxX = 100;
    private const int MaxY = 60;

    public WorldState(TilemapShared newForeground, TilemapShared newBackground) {
        Foreground = newForeground;
        Background = newBackground;
    }

    public WorldState() { }

    public TilemapShared Foreground { get; set; } = new(MaxX, MaxY, false);
    public TilemapShared Background { get; set; } = new(MaxX, MaxY, true);

    public string?[,,] CurrentWorld {
        get {
            string?[,,] world = new string[MaxX, MaxY, 2];
            for (int x = 0; x < MaxX; x++) {
                for (int y = 0; y < MaxY; y++) {
                    world[x, y, 0] = Foreground.TileGrid[x, y]?.SourceBlock.BlockUid;
                    world[x, y, 1] = Background.TileGrid[x, y]?.SourceBlock.BlockUid;
                }
            }
            return world;
        }
    }

    public Dictionary<int, Transform> PlayerPositions { get; set; } = new();
}