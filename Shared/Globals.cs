using Microsoft.Xna.Framework;
using Shared.Code;
using Shared.Code.Block_System;
namespace Shared;

public static class GlobalsShared {
    /// <summary> The total number of ticks stored. 10 secs for now. </summary>
    public const int MaxTicksStored = 600;

    public const int MaxPlayers = 10;

    /// <summary> The maximum world size. (Width) </summary>
    public const int MaxX = 100;

    /// <summary> The maximum world size. (Height) </summary>
    public const int MaxY = 60;

    /// <summary> Tilemap for foreground objects. </summary>
    public static TilemapShared ForegroundTilemap;

    /// <summary> Tilemap for background (non-dense) objects. </summary>
    public static TilemapShared BackgroundTilemap;

    public static readonly Color BackgroundColor = new(0.5f, 0.5f, 0.5f, 1f);

    /// <summary> The size of the grid the game is played on. </summary>
    public static readonly Vector2Int GridSize = new(8, 8);
}