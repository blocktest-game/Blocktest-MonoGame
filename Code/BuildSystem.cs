using Microsoft.Xna.Framework;
using System.Collections.Generic;
namespace Blocktest
{
    public static class BuildSystem
    {
        /// <summary>
        /// An array containing an entry for every block in the world. Used for saving games.
        /// </summary>
        private static readonly int[,,] currentWorld = new int[Globals.maxX, Globals.maxY, 2];

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
            if (foreground && Globals.ForegroundTilemap.HasTile(tilePosition)) {
                Tile prevTile = Globals.ForegroundTilemap.GetTile(tilePosition);
                prevTile.SourceBlock.OnBreak(tilePosition, true);

                Globals.ForegroundTilemap.SetTile(tilePosition, null);
                currentWorld[tilePosition.X, tilePosition.Y, 0] = 0;
            } else if (!foreground && Globals.BackgroundTilemap.HasTile(tilePosition)) {
                Tile prevTile = Globals.BackgroundTilemap.GetTile(tilePosition);
                prevTile.SourceBlock.OnBreak(tilePosition, false);

                Globals.BackgroundTilemap.SetTile(tilePosition, null);
                currentWorld[tilePosition.X, tilePosition.Y, 1] = 0;
            }

            Tilemap tilemap = foreground ? Globals.ForegroundTilemap : Globals.BackgroundTilemap;

            foreach (Vector2Int loc in new List<Vector2Int>() { Vector2Int.Up, Vector2Int.Down, Vector2Int.Left, Vector2Int.Right }) { // Refreshes all blocks in cardinal dirs
                tilemap.GetTile(tilePosition + loc).UpdateAdjacencies(tilePosition + loc, tilemap);
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
        public static void PlaceBlockCell(Block toPlace, bool foreground, Vector2Int tilePosition)
        {
            Tile newTile = new(toPlace, tilePosition);
            toPlace.OnPlace(tilePosition, foreground);

            if (foreground) {
                //newTile.colliderType = Tile.ColliderType.Grid;
                Globals.ForegroundTilemap.SetTile(tilePosition, newTile);
                currentWorld[tilePosition.X, tilePosition.Y, 0] = toPlace.blockID + 1;
            } else if (toPlace.canPlaceBackground) {
                newTile.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                Globals.BackgroundTilemap.SetTile(tilePosition, newTile);
                currentWorld[tilePosition.X, tilePosition.Y, 1] = toPlace.blockID + 1;
            }
        }

        /// <summary>
        /// The method called whenever a block is placed.
        /// </summary>
        /// <param name="toPlace">The block type to place.</param>
        /// <param name="foreground">Whether or not the block should be placed in the foreground.</param>
        /// <param name="tilePosition">The position of the placed block. (Grid coords)</param>
        public static void PlaceBlockCell(Block toPlace, bool foreground, Vector2 tilePosition) => PlaceBlockCell(toPlace, foreground, (Vector2Int)tilePosition);

    }
}