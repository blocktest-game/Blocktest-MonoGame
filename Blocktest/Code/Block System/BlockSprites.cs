using Blocktest.Rendering;
using Shared.Code.Block_System;
namespace Blocktest.Block_System;

/// <summary>
///     Handles sprites for blocks.
/// </summary>
public sealed class BlockSprites {
    /// <summary> Shared block info </summary>
    public BlockShared BlockShared;

    /// <summary> The block's sprite. </summary>
    public Drawable BlockSprite;

    /// <summary> The sprite sheet used for smoothing the block. </summary>
    public SpriteSheet SpriteSheet;

    /* METHODS */

    public BlockSprites(BlockShared newBlockShared) {
        BlockShared = newBlockShared;
        LoadSprite();
    }

    /// <summary>
    ///     Called when the block is created by the block sprites manager.
    /// </summary>
    /// <remarks>
    ///     DO NOT FORGET TO CALL THE BASE METHOD IF YOU OVERRIDE THIS.
    /// </remarks>
    public void LoadSprite() {
        string path = @"Graphics\Blocks\" + BlockShared.BlockName.ToLower().Replace(" ", "");
        try {
            BlockSprite =
                new Drawable(path,
                    new Rectangle(1, 1, 10,
                        10)); //this might need to be expanded in the future in case we decide to make use of the full 12x12 tiles on our spritesheets
            if (!BlockShared.BlockSmoothing) {
                return;
            }
            SpriteSheet = new SpriteSheet(path, 4, 4, 1);
            if (SpriteSheet.OrderedSprites.Length <= 1) {
                Console.WriteLine("Block " +
                                  this +
                                  " is marked as smoothable, but a sprite sheet could not be found at " +
                                  path +
                                  "!");
            }
        }
        catch (ContentLoadException) {
            BlockSprite = new Drawable(@"Graphics\Blocks\error");
            Console.WriteLine("Block " + this + " does not have an icon at " + path + "!");
        }
    }
}