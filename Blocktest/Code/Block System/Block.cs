using Blocktest.Rendering;

namespace Blocktest
{
    /// <summary>
    /// Each block is a different type of tile which can be placed. The behaviours specified in each block class subtype will be used for every tile of that type.
    /// </summary>
    public class Block
    {
        /// <summary> The block's ID (index in the allblocks list). </summary>
        /// <remarks> Leave as -1 for automatic assignment based on init order (probably not a good idea) </remarks>
        public int blockID = -1;
        /// <summary> The block's name. </summary>
        public string blockName = "Error";

        /// <summary> Whether or not the block supports icon smoothing. </summary>
        public bool blockSmoothing = false;
        /// <summary> Whether or not a block smooths only with itself </summary>
        /// <remarks> (Use normal 8x8 sprites to prevent overlap) </remarks>
        public bool smoothSelf = false;
        /// <summary> Whether or not a block can be placed in the background. </summary>
        public bool CanPlaceBackground = true;

        /// <summary> The block's sprite. </summary>
        public Drawable BlockSprite;

        /// <summary> The sprite sheet used for smoothing the block. </summary>
        public SpriteSheet? SpriteSheet;
        
        /* METHODS */

        /// <summary>
        /// Called whenever a block is first loaded by the block manager.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Called whenever a block's content is loaded by the block manager.
        /// </summary>
        /// <remarks>
        /// DO NOT FORGET TO CALL THE BASE METHOD IF YOU OVERRIDE THIS.
        /// </remarks>
        public virtual void LoadSprite(ContentManager content)
        {
            string path = @"Graphics\Blocks\" + blockName.ToLower().Replace(" ", "");
            try {
                BlockSprite = new Drawable(path, new Rectangle(1, 1, 10, 10)); //this might need to be expanded in the future in case we decide to make use of the full 12x12 tiles on our spritesheets
                if (!blockSmoothing) {
                    return;
                }
                SpriteSheet = new SpriteSheet(path, 4, 4, 1);
                if (SpriteSheet.OrderedSprites.Length <= 1) {
                    Console.WriteLine("Block " + this + " is marked as smoothable, but a sprite sheet could not be found at " + path + "!");
                }
            }
            catch (ContentLoadException) {
                BlockSprite = new Drawable(@"Graphics\Blocks\error");
                Console.WriteLine("Block " + this + " does not have an icon at " + path + "!");
            }
        }

        /// <summary>
        /// Called whenever a block is placed.
        /// </summary>
        /// <param name="position">The position of the block being placed.</param>
        /// <param name="tilemap">The tilemap the block is placed on.</param>
        public virtual void OnPlace(Vector2Int position, Tilemap tilemap)
        {
            
        }
        /// <summary>
        /// Called whenever a block is broken.
        /// </summary>
        /// <param name="position">The position of the block being broken.</param>
        /// <param name="tilemap">The tilemap the block was on before it was broken.</param>
        public virtual void OnBreak(Vector2Int position, Tilemap tilemap)
        {

        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return blockName;
        }
    }
}
