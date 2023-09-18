using Shared;
using Shared.Networking;

namespace Shared
{
    public static class GlobalsShared
    {

        /// <summary> Tilemap for foreground objects. </summary>
        private static TilemapShared foregroundTilemap;
        /// <summary> Tilemap for foreground objects. </summary>
        public static TilemapShared ForegroundTilemap { get => foregroundTilemap; set => foregroundTilemap = value; }

        /// <summary> Tilemap for background (non-dense) objects. </summary>
        private static TilemapShared backgroundTilemap;
        /// <summary> Tilemap for background (non-dense) objects. </summary>
        public static TilemapShared BackgroundTilemap { get => backgroundTilemap; set => backgroundTilemap = value; }

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
