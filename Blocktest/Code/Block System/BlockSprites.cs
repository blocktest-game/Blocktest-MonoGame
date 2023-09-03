using Blocktest.Rendering;

namespace Blocktest
{
    /// <summary>
    /// Handles sprites for blocks.
    /// </summary>
    public class BlockSprites
    {
        /// <summary> Shared block info </summary>
        public BlockShared blockShared;

        /// <summary> The block's sprite. </summary>
        public Drawable blockSprite;

        /// <summary> The sprite sheet used for smoothing the block. </summary>
        public SpriteSheet spriteSheet;

        /* METHODS */

        public BlockSprites(BlockShared newBlockShared, ContentManager content)
        {
            blockShared = newBlockShared;
            LoadSprite(content);
        }

        /// <summary>
        /// Called when the block is created by the block sprites manager.
        /// </summary>
        /// <remarks>
        /// DO NOT FORGET TO CALL THE BASE METHOD IF YOU OVERRIDE THIS.
        /// </remarks>
        public virtual void LoadSprite(ContentManager content)
        {
            string path = @"Graphics\Blocks\" + blockShared.blockName.ToLower().Replace(" ", "");
            try {
                blockSprite = new Drawable(path, new Rectangle(1, 1, 10, 10)); //this might need to be expanded in the future in case we decide to make use of the full 12x12 tiles on our spritesheets
                if (!blockShared.blockSmoothing) {
                    return;
                }
                spriteSheet = new SpriteSheet(path, 4, 4, 1);
                if (spriteSheet.OrderedSprites.Length <= 1) {
                    Console.WriteLine("Block " + this + " is marked as smoothable, but a sprite sheet could not be found at " + path + "!");
                }
            }
            catch (ContentLoadException) {
                blockSprite = new Drawable(@"Graphics\Blocks\error");
                Console.WriteLine("Block " + this + " does not have an icon at " + path + "!");
            }
        }
    }
}
