using Microsoft.Xna.Framework;
using Shared;
using Shared.Networking;

namespace Shared
{
    public static class GlobalsShared
    {

        /// <summary> Tilemap for foreground objects. </summary>
        public static TilemapShared ForegroundTilemap;

        /// <summary> Tilemap for background (non-dense) objects. </summary>
        public static TilemapShared BackgroundTilemap;

        public static Color backgroundColor = new Color(0.5f, 0.5f, 0.5f, 1f);

        /// <summary> The maximum world size. (Width) </summary>
        public static readonly int maxX = 100;
        /// <summary> The maximum world size. (Height) </summary>
        public static readonly int maxY = 60;

        /// <summary> The size of the grid the game is played on. </summary>
        public static readonly Vector2Int gridSize = new(8, 8);

        /// <summary> The total number of ticks stored. 10 secs for now. </summary>
        public const int MaxTicksStored = 600;

        public const int MaxPlayers = 10;
    }
}
