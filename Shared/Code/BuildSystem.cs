using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Shared.Networking;

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
                int z = foreground ? 1 : 0;     // Convert foreground bool to int
	            tilemap.GetTile(tilePosition).SourceBlock.OnBreak(tilePosition, true);

	            tilemap.SetTile(tilePosition, null);
                currentWorld[tilePosition.X, tilePosition.Y, z] = 0;
            } 
            else
            {
	            return;
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

        /// <summary>
        /// Loads a new world into the tilemaps and currentWorld.
        /// </summary>
        /// <param name="newWorld">An array containing an entry for every block in the world. Used for loading games. MUST be the same dimensions as currentWorld</param>
        public static void LoadNewWorld(int[,,] newWorld)
        {
            for(int x = 0; x < GlobalsShared.maxX; x++)
            {
                for(int y = 0; y < GlobalsShared.maxY; y++)
                {
                    for(int z = 0; z < 2; z++)
                    {
                        Vector2Int tilePosition = new(x,y);
                        int blockNum = newWorld[x,y,z];
                        LoadNewBlock(blockNum, tilePosition, z);
                    }
                }
            }
        }

        /// <summary>
        /// Loads a new world into the tilemaps and currentWorld.
        /// </summary>
        /// <param name="packet">Incoming <see cref="WorldDownload"/> </param>
        public static void LoadNewWorld(WorldDownload packet) => LoadNewWorld(packet.world);

        /// <summary>
        /// Loads a new block from an int
        /// </summary>
        /// <param name="blockNum">The blockid + 1</param>
        /// <param name="tilePosition">The position of the tile in the tilemap</param>
        /// <param name="foregroundInt">Whether the block is in the foreground, expressed as an int</param>
        private static void LoadNewBlock(int blockNum, Vector2Int tilePosition, int foregroundInt)
        {
            currentWorld[tilePosition.X, tilePosition.Y, foregroundInt] = blockNum;
            bool foreground = Convert.ToBoolean(foregroundInt);
            if(blockNum > 0)
            {
                BlockShared newBlock = BlockManagerShared.AllBlocks[blockNum - 1];
                PlaceBlockCell(newBlock, foreground, tilePosition);
            }
            else
            {
                BreakBlockCell(foreground, tilePosition);
            }
        }
    }
}