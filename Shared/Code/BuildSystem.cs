using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Shared
{
    public static class BuildSystem
    {
        /// <summary>
        /// An array containing an entry for every block in the world. Used for saving games.
        /// </summary>
        private static readonly int[,,] currentWorld = new int[GlobalsShared.maxX, GlobalsShared.maxY, 2];

        /// <summary>
        /// The method called whenever an object is removed.
        /// </summary>
        /// <param name="foreground">Whether or not the block to be destroyed is in the foreground.</param>
        /// <param name="position">The position of the block to destroy (world coords)</param>
        //public static void BreakBlockWorld(bool foreground, Vector2 position) => BreakBlockCell(foreground, Globals.foreground.WorldToCell(position));

        /// <summary>
        /// The method called whenever an object is removed.
        /// </summary>
        /// <param name="foreground">Whether or not the block to be destroyed is in the foreground.</param>
        /// <param name="tilePosition">The position of the block to destroy (grid coords)</param>
        public static void BreakBlockCell(bool foreground, Vector2Int tilePosition)
        {
	        TilemapShared tilemap = foreground ? GlobalsShared.ForegroundTilemap : GlobalsShared.BackgroundTilemap;
	        
            if (tilemap.HasTile(tilePosition)) 
            {
	            tilemap.GetTile(tilePosition).SourceBlock.OnBreak(tilePosition, true);

	            tilemap.SetTile(tilePosition, null);
                currentWorld[tilePosition.X, tilePosition.Y, 0] = 0;
            } 
            else
            {
	            return;
            }

            foreach (Vector2Int loc in new List<Vector2Int>() { Vector2Int.Up, Vector2Int.Down, Vector2Int.Left, Vector2Int.Right }) { // Refreshes all blocks in cardinal dirs
                if ((tilePosition + loc).X >= 0 && (tilePosition + loc).X < tilemap.tilemapSize.X && (tilePosition + loc).Y >= 0 && (tilePosition + loc).Y < tilemap.tilemapSize.Y && (tilemap.HasTile(tilePosition + loc)))
                {
					tilemap.GetTile(tilePosition + loc).UpdateAdjacencies(tilePosition + loc, tilemap);
	            }
            }

        }

        /// <summary>
        /// The method called whenever a block is placed.
        /// </summary>
        /// <param name="toPlace">The block type to place.</param>
        /// <param name="foreground">Whether or not the block should be placed in the foreground.</param>
        /// <param name="position">The position of the placed block. (World coords)</param>
        //public static void PlaceBlockWorld(Block toPlace, bool foreground, Vector2 position) => PlaceBlockCell(toPlace, foreground, Globals.foreground.WorldToCell(position));

        /// <summary>
        /// The method called whenever a block is placed.
        /// </summary>
        /// <param name="toPlace">The block type to place.</param>
        /// <param name="foreground">Whether or not the block should be placed in the foreground.</param>
        /// <param name="tilePosition">The position of the placed block. (Grid coords)</param>
        public static void PlaceBlockCell(BlockShared toPlace, bool foreground, Vector2Int tilePosition)
        {
            TileShared newTile = new(toPlace, tilePosition);
            toPlace.OnPlace(tilePosition, foreground);

            if (foreground) {
                //newTile.colliderType = Tile.ColliderType.Grid;
                GlobalsShared.ForegroundTilemap.SetTile(tilePosition, newTile);
                currentWorld[tilePosition.X, tilePosition.Y, 0] = toPlace.blockID + 1;
            } else if (toPlace.canPlaceBackground) {
                newTile.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                GlobalsShared.BackgroundTilemap.SetTile(tilePosition, newTile);
                currentWorld[tilePosition.X, tilePosition.Y, 1] = toPlace.blockID + 1;
            }
        }

        /// <summary>
        /// The method called whenever a block is placed.
        /// </summary>
        /// <param name="toPlace">The block type to place.</param>
        /// <param name="foreground">Whether or not the block should be placed in the foreground.</param>
        /// <param name="tilePosition">The position of the placed block. (Grid coords)</param>
        public static void PlaceBlockCell(BlockShared toPlace, bool foreground, Vector2 tilePosition) => PlaceBlockCell(toPlace, foreground, (Vector2Int)tilePosition);

    }
}