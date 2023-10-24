using Blocktest.Rendering;
using Shared.Code.Block_System;
namespace Blocktest.Block_System;

/// <summary>
///     Handles sprites for blocks.
/// </summary>
public sealed class BlockSprites {
    /// <summary> Shared block info </summary>
    private readonly BlockShared _blockShared;

    /// <summary> The block's sprite. </summary>
    public Drawable BlockSprite;

    /// <summary> The sprite sheet used for smoothing the block. </summary>
    public SpriteSheet SpriteSheet;

    /* METHODS */

    public BlockSprites(BlockShared newBlockShared) {
        _blockShared = newBlockShared;
        SpriteSheet = SpriteSheet.ErrorSpriteSheet;
        
        string path = @"Graphics\Blocks\" + _blockShared.BlockName.ToLower().Replace(" ", "");
        try {
            BlockSprite =
                new Drawable(path,
                    new Rectangle(1, 1, 10,
                        10)); //this might need to be expanded in the future in case we decide to make use of the full 12x12 tiles on our spritesheets
            if (!_blockShared.BlockSmoothing) {
                return;
            }
            SpriteSheet = new SpriteSheet(path, 4, 4, 1);
            if (SpriteSheet.OrderedSprites.Length <= 1) {
                Console.WriteLine(
                    $"Block {this} is marked as smoothable, but a sprite sheet could not be found at {path}!");
            }
        }
        catch (ContentLoadException) {
            BlockSprite = Drawable.ErrorDrawable;
            Console.WriteLine($"Block {this} does not have a sprite at {path}!");
        }
    }
}