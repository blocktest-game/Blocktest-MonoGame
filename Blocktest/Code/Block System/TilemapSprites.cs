using Blocktest.Rendering;
namespace Blocktest
{
    /// <summary>
    /// Handles drawing <see cref="TilemapShared"/>.
    /// </summary>
    public class TilemapSprites
    {
        public TilemapShared tilemap;

        /// <summary>
        /// Creates a <see cref="Tilemap"/>.
        /// </summary>
        /// <param name="sizeX">The width of the tilemap in tiles.</param>
        /// <param name="sizeY">The height of the tilemap in tiles.</param>
        public TilemapSprites(TilemapShared newTilemap)
        {
            tilemap = newTilemap;
        }

        /// <summary>
        /// Called from the main draw loop, draws each tile in the tilemap.
        /// </summary>
        /// <remarks>
        /// This can almost definitely be optimized, but I'm focused on getting a decent version out
        /// quickly so the codebase split causes fewer issues for others.
        /// </remark>
        /// <param name="spriteBatch">The spritebatch to draw the tilemap tiles' sprite on.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (TileShared tile in tilemap.allTiles) {
                BlockSprites blockSprites = BlockSpritesManager.AllBlocksSprites[tile.SourceBlock.blockID];
                Drawable sprite = blockSprites.spriteSheet.OrderedSprites[tile.bitmask];
                spriteBatch.Draw(sprite.Texture, new Vector2(tile.rectangle.X, tile.rectangle.Y), sprite.Bounds, tile.color);
            }
        }
    }
}
