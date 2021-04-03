namespace Blocktest
{
    public static class Globals
    {
        /// <summary> The main Game instance. </summary>
        public static BlocktestGame Game;

        /// <summary> Tilemap for foreground objects. </summary>
        public static Tilemap foreground;
        /// <summary> Tilemap for background (non-dense) objects. </summary>
        public static Tilemap background;

        /// <summary> The maximum world size. (Width) </summary>
        public static readonly int maxX = 255;
        /// <summary> The maximum world size. (Height) </summary>
        public static readonly int maxY = 255;

        /// <summary> The size of the grid the game is played on. </summary>
        public static readonly Vector2Int gridSize = new(8, 8);
    }
}
