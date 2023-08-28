namespace Blocktest
{
    public static class Globals
    {

        /// <summary> Tilemap for foreground objects. </summary>
        private static Tilemap foregroundTilemap;
        /// <summary> Tilemap for foreground objects. </summary>
        public static Tilemap ForegroundTilemap { get => foregroundTilemap; set => foregroundTilemap = value; }

        /// <summary> Tilemap for background (non-dense) objects. </summary>
        private static Tilemap backgroundTilemap;
        /// <summary> Tilemap for background (non-dense) objects. </summary>
        public static Tilemap BackgroundTilemap { get => backgroundTilemap; set => backgroundTilemap = value; }

        /// <summary> The maximum world size. (Width) </summary>
        public static readonly int maxX = 255;
        /// <summary> The maximum world size. (Height) </summary>
        public static readonly int maxY = 255;

        /// <summary> The size of the grid the game is played on. </summary>
        public static readonly Vector2Int gridSize = new(8, 8);



    }
}
