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
        /// <param name="tilePosition">The position of the block to destroy (grid coords)</param>
        public static void BreakBlockCell(bool foreground, Vector2Int tilePosition)
        {
            if(tilePosition.X >= GlobalsShared.maxX || tilePosition.Y >= GlobalsShared.maxY)
            {
                return;
            }
	        TilemapShared tilemap = foreground ? GlobalsShared.ForegroundTilemap : GlobalsShared.BackgroundTilemap;

            BlockShared toPlace = BlockManagerShared.AllBlocks[0];
            TileShared newTile = new(toPlace, tilePosition);
	        
            if (tilemap.HasTile(tilePosition)) 
            {
                int z = foreground ? 1 : 0;     // Convert foreground bool to int
	            tilemap.GetTile(tilePosition).SourceBlock.OnBreak(tilePosition, true);

	            tilemap.SetTile(tilePosition, newTile);
                currentWorld[tilePosition.X, tilePosition.Y, z] = 1;
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
        /// <param name="tilePosition">The position of the placed block. (Grid coords)</param>
        public static void PlaceBlockCell(BlockShared toPlace, bool foreground, Vector2Int tilePosition)
        {
            if(tilePosition.X >= GlobalsShared.maxX || tilePosition.Y >= GlobalsShared.maxY)
            {
                return;
            }
            TileShared newTile = new(toPlace, tilePosition);            // TODO - remove new
            toPlace.OnPlace(tilePosition, foreground);

            if (foreground) {
                GlobalsShared.ForegroundTilemap.SetTile(tilePosition, newTile);
                currentWorld[tilePosition.X, tilePosition.Y, 1] = toPlace.blockID + 1;
            } else if (toPlace.canPlaceBackground) {
                newTile.color = GlobalsShared.backgroundColor;
                GlobalsShared.BackgroundTilemap.SetTile(tilePosition, newTile);
                currentWorld[tilePosition.X, tilePosition.Y, 0] = toPlace.blockID + 1;
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
        /// <param name="foregroundInt">Whether the blo
                //Console.WriteLine("Current tick: " + currTick);
                //Console.WriteLine("i: " + i);ck is in the foreground, expressed as an int</param>
        private static void LoadNewBlock(int blockNum, Vector2Int tilePosition, int foregroundInt)
        {
            currentWorld[tilePosition.X, tilePosition.Y, foregroundInt] = blockNum;
            bool foreground = Convert.ToBoolean(foregroundInt);
            if(blockNum > 1)
            {
                BlockShared newBlock = BlockManagerShared.AllBlocks[blockNum - 1];
                PlaceBlockCell(newBlock, foreground, tilePosition);
            }
            else
            {
                BreakBlockCell(foreground, tilePosition);
            }
        }

        public static int[,,] getCurrentWorld()
        {
            return currentWorld;
        }
    }
}