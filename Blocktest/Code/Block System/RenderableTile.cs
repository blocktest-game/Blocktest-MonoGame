using Blocktest.Rendering;
using Shared.Code;
using Shared.Code.Block_System;
namespace Blocktest.Block_System;

public class RenderableTile : TileShared {
    public readonly Renderable Renderable;

    public RenderableTile(TileShared tile, bool background) : base(tile.SourceBlock,
        tile.Transform.Position / GlobalsShared.GridSize) {
        Renderable = new Renderable(Transform, background ? Layer.BackgroundBlocks : Layer.ForegroundBlocks,
            BlockSpritesManager.AllBlocksSprites[tile.SourceBlock.BlockUid].BlockSprite,
            background ? Color.Gray : Color.White);
    }

    /// <summary>
    ///     This method is called whenever an adjacent (according to a tilemap's adjacency variable) tile is placed or removed.
    ///     Used for smoothing.
    /// </summary>
    /// <param name="position">The position of the current tile.</param>
    /// <param name="tilemap">The tilemap the tile is on.</param>
    public void UpdateAdjacencies(Vector2Int position, TilemapShared tilemap) {
        if (!SourceBlock.BlockSmoothing) {
            return;
        } // If the tile doesn't smooth, don't even try

        DirectionalBitmask dirBitmask = DirectionalBitmask.None;

        if (HasSmoothableTile(position + Vector2Int.Up, tilemap)) {
            dirBitmask |= DirectionalBitmask.Up;
        }
        if (HasSmoothableTile(position + Vector2Int.Down, tilemap)) {
            dirBitmask |= DirectionalBitmask.Down;
        }
        if (HasSmoothableTile(position + Vector2Int.Right, tilemap)) {
            dirBitmask |= DirectionalBitmask.Right;
        }
        if (HasSmoothableTile(position + Vector2Int.Left, tilemap)) {
            dirBitmask |= DirectionalBitmask.Left;
        }

        Renderable.Appearance = BlockSpritesManager.AllBlocksSprites[SourceBlock.BlockUid].SpriteSheet
            .OrderedSprites[(int)dirBitmask];
    }

    /// <summary>
    ///     Whether or not the tile at a certain <paramref name="position" /> can smooth with this tile.
    /// </summary>
    /// <param name="position">The position of the tile to check for smoothing.</param>
    /// <param name="tilemap">The tilemap on which the tile you want to check for smoothing is.</param>
    /// <returns>Whether or not the tile can smooth with this tile.</returns>
    private bool HasSmoothableTile(Vector2Int position, TilemapShared tilemap) {
        if (tilemap.TryGetTile(position, out TileShared? tile)) {
            return SourceBlock.SmoothSelf
                ? IsSameTileType(tile)
                : tile.SourceBlock.BlockUid !=
                  "air"; // Don't smooth with air, possibly find nicer way to do this later.
        }
        return false;
    }

    /// <summary>
    ///     If the tile provided is the same type (references the same block) as the current tile.
    /// </summary>
    /// <param name="otherTile">The other tile to check.</param>
    /// <returns>Whether or not the other block is the same type as the current tile</returns>
    private bool IsSameTileType(TileShared otherTile) => otherTile.SourceBlock == SourceBlock;

    [Flags]
    private enum DirectionalBitmask {
        None = 0,
        Up = 1,
        Down = 2,
        Right = 4,
        Left = 8
    }
}