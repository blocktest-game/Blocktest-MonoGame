using Shared.Code;
namespace Shared;

public static class GlobalsShared {
    /// <summary> The total number of ticks stored. 10 secs for now. </summary>
    public const int MaxTicksStored = 600;

    public const int MaxPlayers = 10;

    /// <summary> The maximum world size. (Width) </summary>
    public const int MaxX = 100;

    /// <summary> The maximum world size. (Height) </summary>
    public const int MaxY = 60;

    /// <summary> The size of the grid the game is played on. </summary>
    public static readonly Vector2Int GridSize = new(8, 8);
}