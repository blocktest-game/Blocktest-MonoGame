using Blocktest.Rendering;
using Shared.Code.Block_System;
namespace Blocktest.Block_System;

/// <summary>
///     Handles drawing <see cref="TilemapShared" />.
/// </summary>
public sealed class TilemapSprites {
    public TilemapShared Tilemap;

    /// <summary>
    ///     Creates a <see cref="Tilemap" />.
    /// </summary>
    /// <param name="sizeX">The width of the tilemap in tiles.</param>
    /// <param name="sizeY">The height of the tilemap in tiles.</param>
    public TilemapSprites(TilemapShared newTilemap) {
        Tilemap = newTilemap;
    }

    /// <summary>
    ///     Called from the main draw loop, draws each tile in the tilemap.
    /// </summary>
    /// <remarks>
    ///     This can almost definitely be optimized, but I'm focused on getting a decent version out
    ///     quickly so the codebase split causes fewer issues for others.
    ///     </remark>
    ///     <param name="spriteBatch">The spritebatch to draw the tilemap tiles' sprite on.</param>
    public void DrawAllTiles(SpriteBatch spriteBatch) {
        for (int x = 0; x < Tilemap.TilemapSize.X; x++) {
            for (int y = 0; y < Tilemap.TilemapSize.Y; y++) {
                TileShared? tile = Tilemap.TileGrid[x, y];
                BlockSprites blockSprites = BlockSpritesManager.AllBlocksSprites[tile.SourceBlock.BlockId];
                Drawable sprite = blockSprites.SpriteSheet.OrderedSprites[tile.Bitmask];
                spriteBatch.Draw(sprite.Texture, new Vector2(tile.Rectangle.X, tile.Rectangle.Y), sprite.Bounds,
                    tile.Color);
            }
        }
        /*foreach (TileShared tile in tilemap.allTiles) {
            BlockSprites blockSprites = BlockSpritesManager.AllBlocksSprites[tile.SourceBlock.blockID];
            Drawable sprite = blockSprites.spriteSheet.OrderedSprites[tile.bitmask];
            spriteBatch.Draw(sprite.Texture, new Vector2(tile.rectangle.X, tile.rectangle.Y), sprite.Bounds, tile.color);
        }*/
    }
}