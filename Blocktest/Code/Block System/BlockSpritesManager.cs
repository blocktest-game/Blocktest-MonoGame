using Shared.Code.Block_System;
namespace Blocktest.Block_System;

public sealed class BlockSpritesManager {
    /// <summary> Array which stores all blocksprites instances for referencing as if they were globals. </summary>
    private static BlockSprites[] _allBlocksSprites;

    /// <summary> List used to store the names of blocks. The indexes are the corresponding block's ID. </summary>
    private static string[] _blockSpriteNames;

    /// <summary> Array which stores all blocksprites instances for referencing as if they were globals. </summary>
    public static BlockSprites[] AllBlocksSprites {
        get => _allBlocksSprites;
        private set => _allBlocksSprites = value;
    }
    /// <summary> List used to store the names of blocks. The indexes are the corresponding block's ID. </summary>
    public static string[] BlockSpriteNames {
        get => _blockSpriteNames;
        private set => _blockSpriteNames = value;
    }

    public static void LoadBlockSprites(ContentManager content) {
        AllBlocksSprites = new BlockSprites[BlockManagerShared.AllBlocks.Length];
        BlockSpriteNames = new string[BlockManagerShared.AllBlocks.Length];

        for (int i = 0; i < BlockManagerShared.AllBlocks.Length; i++) {
            BlockShared block = BlockManagerShared.AllBlocks[i];
            BlockSprites newBlockSprites = new(block, content);
            BlockSpriteNames[block.BlockId] = block.BlockName;
            AllBlocksSprites[block.BlockId] = newBlockSprites;
        }
    }
}