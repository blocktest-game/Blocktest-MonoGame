using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

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
        public bool canPlaceBackground = true;

        /// <summary> The block's sprite. </summary>
        public Texture2D blockSprite;

        /// <summary> The sprite sheet used for smoothing the block. </summary>
        public SpriteSheet spriteSheet;

        /* METHODS */

        /// <summary>
        /// Called whenever a block is first loaded by the block manager.
        /// </summary>
        /// <remarks>
        /// DO NOT FORGET TO CALL THE BASE METHOD IF YOU OVERRIDE THIS.
        /// </remarks>
        public virtual void Initialize()
        {
            string path = "Blocks\\" + blockName.ToLower().Replace(" ", null);
            blockSprite = Globals.Game.Content.Load<Texture2D>(path);
            if (blockSprite == null) {
                Console.WriteLine("Block " + this + " does not have an icon at " + path + "!");
            }
            if (blockSmoothing && false) { // TODO: Remove "&& false" when sprite sheet system works
                spriteSheet = new SpriteSheet(path);
                if (spriteSheet.spritesDict.Count <= 1) {
                    Console.WriteLine("Block " + this + " is marked as smoothable, but a sprite sheet could not be found at " + path + "!");
                }
            }
        }

        /// <summary>
        /// Called whenever a block is placed.
        /// </summary>
        /// <param name="position">The position of the block being placed.</param>
        /// <param name="foreground">Whether the block being placed is in the foreground or not.</param>
        public virtual void OnPlace(Vector2Int position, bool foreground)
        {

        }
        /// <summary>
        /// Called whenever a block is broken.
        /// </summary>
        /// <param name="position">The position of the block being broken.</param>
        /// <param name="foreground">Whether the block being broken is in the foreground or not.</param>
        public virtual void OnBreak(Vector2Int position, bool foreground)
        {

        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return blockName;
        }
    }
}
