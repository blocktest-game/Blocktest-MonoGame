using Microsoft.VisualBasic;

namespace Blocktest
{
    public static class Globals
    {

        /// <summary> Tilemap for foreground objects. </summary>
        private static TilemapSprites foregroundTilemapSprites;
        /// <summary> Tilemap for foreground objects. </summary>
        public static TilemapSprites ForegroundTilemapSprites { get => foregroundTilemapSprites; set => foregroundTilemapSprites = value; }

        /// <summary> Tilemap for background (non-dense) objects. </summary>
        private static TilemapSprites backgroundTilemapSprites;
        /// <summary> Tilemap for background (non-dense) objects. </summary>
        public static TilemapSprites BackgroundTilemapSprites { get => backgroundTilemapSprites; set => backgroundTilemapSprites = value; }
        

    }
}
