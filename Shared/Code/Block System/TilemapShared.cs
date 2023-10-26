using System.Diagnostics.CodeAnalysis;
namespace Shared.Code.Block_System;

/// <summary>
///     A grid filled with <see cref="TileShared" />s, usually representing terrain.
/// </summary>
public sealed class TilemapShared {
    public readonly bool Background;

    /// <summary>
    ///     The size of each cell (in pixels) in the tilemap's grid.
    /// </summary>
    public readonly Vector2Int GridSize = new(8, 8);

    /// <summary>
    ///     The 2D array of all tiles in the tilemap.
    /// </summary>
    public readonly TileShared?[,] TileGrid;

    /// <summary>
    ///     The size of the tilemap in tiles.
    /// </summary>
    public readonly Vector2Int TilemapSize;


    /// <summary>
    ///     Creates a <see cref="Tilemap" />.
    /// </summary>
    /// <param name="sizeX">The width of the tilemap in tiles.</param>
    /// <param name="sizeY">The height of the tilemap in tiles.</param>
    public TilemapShared(int sizeX, int sizeY, bool background) {
        TilemapSize = new Vector2Int(sizeX, sizeY);
        TileGrid = new TileShared[sizeX, sizeY];
        for (int x = 0; x < sizeX; x++) {
            for (int y = 0; y < sizeY; y++) {
                TileGrid[x, y] =
                    new TileShared(BlockManagerShared.AllBlocks["air"], new Vector2Int(x, y)); //Fill with air
            }
        }
        Background = background;
    }

    public event Action<TileShared, Vector2Int>? OnTileChanged;

    /// <summary>
    ///     Sets a Tile at the given XYZ coordinates of a cell in the tile map to a specific <see cref="Block" /> type.
    /// </summary>
    /// <param name="location">Location the new Block will be placed.</param>
    /// <param name="newBlock">Block type to be placed in the cell.</param>
    public void SetBlock(Vector2Int location, BlockShared newBlock) =>
        SetTile(location, new TileShared(newBlock, location));

    public void SetBlock(Vector2Int location, string blockUid) =>
        SetTile(location, new TileShared(BlockManagerShared.AllBlocks[blockUid], location));

    /// <summary>
    ///     Sets a Tile at the given XYZ coordinates of a cell in the tile map to a specific <see cref="Block" /> type.
    /// </summary>
    /// <param name="location">Location the new Block will be placed.</param>
    /// <param name="newTile">Block type to be placed in the cell.</param>
    public void SetTile(Vector2Int location, TileShared newTile) {
        if (location.X < 0 || location.Y < 0 || location.X >= TilemapSize.X || location.Y >= TilemapSize.Y) {
            return;
        }

        if (TryGetTile(location, out TileShared? oldTile)) {
            oldTile.SourceBlock.OnBreak(location, Background);
        }

        TileGrid[location.X, location.Y] = newTile;

        newTile.SourceBlock.OnPlace(location, Background);

        OnTileChanged?.Invoke(newTile, location);
    }

    /// <summary>
    ///     Deletes a <see cref="TileShared" /> at a specific location (sets block to air).
    /// </summary>
    /// <param name="location"></param>
    public void DeleteTile(Vector2Int location) => SetBlock(location, BlockManagerShared.AllBlocks["air"]);

    public bool TryGetTile<T>(Vector2Int location, [NotNullWhen(true)] out T? result) where T : TileShared {
        result = null;
        if (location.X < 0 || location.Y < 0 || location.X >= TilemapSize.X || location.Y >= TilemapSize.Y) {
            return false;
        }
        result = TileGrid[location.X, location.Y] as T;
        return result != null;
    }
}