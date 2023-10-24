using System.Linq;
using Shared.Code.Block_System;
namespace Blocktest.Block_System;

public sealed class BlockSpritesManager {
    /// <summary> Array which stores all block sprites instances for referencing as if they were globals. </summary>
    public static Dictionary<string, BlockSprites> AllBlocksSprites { get; private set; }

    public static void LoadBlockSprites() {
        AllBlocksSprites = BlockManagerShared.AllBlocks.ToDictionary(uid => uid.Key, block => new BlockSprites(block.Value));
    }
}