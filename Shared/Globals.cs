using Shared;

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
        public static readonly int maxX = 255;
        /// <summary> The maximum world size. (Height) </summary>
        public static readonly int maxY = 255;

        /// <summary> The size of the grid the game is played on. </summary>
        public static readonly Vector2Int gridSize = new(8, 8);

        //public static BlockManager blockManager = new();

    }
}
