using Blocktest.Rendering;

namespace Blocktest
{
    public class BlockSpritesManager
    {
        /// <summary> Array which stores all blocksprites instances for referencing as if they were globals. </summary>
        private static BlockSprites[] allBlocksSprites;
        /// <summary> Array which stores all blocksprites instances for referencing as if they were globals. </summary>
        public static BlockSprites[] AllBlocksSprites { get => allBlocksSprites; private set => allBlocksSprites = value; }

        /// <summary> List used to store the names of blocks. The indexes are the corresponding block's ID. </summary>
        private static string[] blockSpriteNames;
        /// <summary> List used to store the names of blocks. The indexes are the corresponding block's ID. </summary>
        public static string[] BlockSpriteNames { get => blockSpriteNames; private set => blockSpriteNames = value; }

        public static void LoadBlockSprites(ContentManager content)
        {
            AllBlocksSprites = new BlockSprites[BlockManagerShared.AllBlocks.Length];
            BlockSpriteNames = new string[BlockManagerShared.AllBlocks.Length];

            for(int i = 0; i < BlockManagerShared.AllBlocks.Length; i++)
            {
                BlockShared block = BlockManagerShared.AllBlocks[i];
                BlockSprites newBlockSprites = new(block, content);
                BlockSpriteNames[block.blockID] = block.blockName;
                AllBlocksSprites[block.blockID] = newBlockSprites;
            }
        }
    }
}
