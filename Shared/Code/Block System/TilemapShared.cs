using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
namespace Shared.Code.Block_System;

/// <summary>
///     A grid filled with <see cref="TileShared" />s, usually representing terrain.
/// </summary>
public sealed class TilemapShared {
    /// <summary>
    ///     A list of <see cref="Vector2Int" />s that specify which blocks should be refreshed when a tile is placed/destroyed.
    ///     Defaults to the changed block and all cardinal directions.
    /// </summary>
    private readonly List<Vector2Int> _adjacencies = new()
        { Vector2Int.Zero, Vector2Int.Up, Vector2Int.Down, Vector2Int.Left, Vector2Int.Right };

    /// <summary>
    ///     The size of each cell (in pixels) in the tilemap's grid.
    /// </summary>
    public readonly Vector2Int GridSize = new(8, 8);

    /// <summary>
    ///     The size of the tilemap in tiles.
    /// </summary>
    public readonly Vector2Int TilemapSize;

    /// <summary>
    ///     The 2D array of all tiles in the tilemap.
    /// </summary>
    public TileShared?[,] TileGrid;


    /// <summary>
    ///     Creates a <see cref="Tilemap" />.
    /// </summary>
    /// <param name="sizeX">The width of the tilemap in tiles.</param>
    /// <param name="sizeY">The height of the tilemap in tiles.</param>
    public TilemapShared(int sizeX, int sizeY) {
        TilemapSize = new Vector2Int(sizeX, sizeY);
        TileGrid = new TileShared[sizeX, sizeY];
        for (int x = 0; x < sizeX; x++) {
            for (int y = 0; y < sizeY; y++) {
                TileGrid[x, y] = new TileShared(BlockManagerShared.AllBlocks[0], new Vector2Int(x, y)); //Fill with air
            }
        }
    }

    /// <summary>
    ///     Sets a Tile at the given XYZ coordinates of a cell in the tile map to a specific <see cref="Block" /> type.
    /// </summary>
    /// <param name="location">Location the new Block will be placed.</param>
    /// <param name="newBlock">Block type to be placed in the cell.</param>
    public TileShared SetBlock(Vector2Int location, BlockShared newBlock) =>
        SetTile(location, new TileShared(newBlock, location));

    /// <summary>
    ///     Sets a Tile at the given XYZ coordinates of a cell in the tile map to a specific <see cref="Block" /> type.
    /// </summary>
    /// <param name="location">Location the new Block will be placed.</param>
    /// <param name="newTile">Block type to be placed in the cell.</param>
    public TileShared SetTile(Vector2Int location, TileShared newTile) {
        TileShared oldTile = GetTile(location);

        TileGrid[location.X, location.Y] = newTile;

        foreach (Vector2Int dir in _adjacencies) {
            if (location.X + dir.X < 0 ||
                location.X + dir.X >= TilemapSize.X ||
                location.Y + dir.Y < 0 ||
                location.Y + dir.Y >= TilemapSize.Y) {
                continue;
            }
            TileGrid[location.X + dir.X, location.Y + dir.Y]?.UpdateAdjacencies(location + dir, this);
        }

        return newTile;
    }

    /// <summary>
    ///     Deletes a <see cref="TileShared" /> at a specific location(sets value to null).
    /// </summary>
    /// <param name="location"></param>
    public void DeleteTile(Vector2Int location) => SetTile(location, null);

    /// <summary>
    ///     Gets the <see cref="TileShared" /> at a specific location on a <see cref="Tilemap" />.
    /// </summary>
    /// <param name="location">Location of the Tile on the Tilemap to check.</param>
    /// <returns><see cref="TileShared" /> placed at the cell.</returns>
    public TileShared? GetTile(Vector2Int location) => GetTile(location.X, location.Y);

    /// <summary>
    ///     Gets the <see cref="TileShared" /> at a specific location on a <see cref="Tilemap" />.
    /// </summary>
    /// <param name="x">X position of the Tile on the Tilemap to check.</param>
    /// <param name="y">Y position of the Tile on the Tilemap to check.</param>
    /// <returns><see cref="TileShared" /> placed at the cell.</returns>
    public TileShared? GetTile(int x, int y) {
        if (x < 0 || y < 0 || x >= TilemapSize.X || y >= TilemapSize.Y) {
            return null;
        }
        return TileGrid[x, y];
    }

    /// <summary>
    ///     Returns whether there is a <see cref="TileShared" /> at the location specified.
    /// </summary>
    /// <param name="location">Location to check.</param>
    /// <returns>Returns true if there is a Tile at the position. Returns false otherwise.</returns>
    public bool HasTile(Vector2Int location) => TileGrid[location.X, location.Y] != null;

    public bool TryGetTile<T>(Vector2Int location, [NotNullWhen(true)] out T? result) where T : TileShared {
        result = null;
        if (location.X < 0 || location.Y < 0 || location.X >= TilemapSize.X || location.Y >= TilemapSize.Y) {
            return false;
        }
        result = TileGrid[location.X, location.Y] as T;
        return result != null;
    }
}

/// <summary>
///     A <see cref="TilemapShared" /> is filled with tile instances, one for each grid square.
///     They contain basic information such as name and sprite, but the behaviours and more advanced properties are found
///     in the correlating Block classes.
/// </summary>
public class TileShared {
    /// <summary>
    ///     Used for bitmask smoothing, should MAYBE not be here.
    /// </summary>
    public byte Bitmask;

    /// <summary>
    ///     Color of the tile.
    /// </summary>
    public Color Color = Color.White;

    /// <summary>
    ///     The rectangle of the tile, used for sprite rendering and collisions.
    /// </summary>
    public Rectangle Rectangle;

    /// <summary>
    ///     The size of the tile square's edges, in pixels (Default 8)
    /// </summary>
    protected byte Size = 8;

    /// <summary>
    ///     The type of block this tile is.
    /// </summary>
    public BlockShared SourceBlock;

    /// <summary>
    ///     Creates a <see cref="TileShared" />.
    /// </summary>
    /// <param name="newBlock">The type of block the new tile should be.</param>
    /// <param name="position">The position in a tilemap the tile will be.</param>
    public TileShared(BlockShared newBlock, Vector2Int position) {
        SourceBlock = newBlock;
        Rectangle = new Rectangle(GlobalsShared.GridSize.X * position.X, GlobalsShared.GridSize.Y * position.Y, Size,
            Size); // HACK: This can probably be done better
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

        Bitmask = 0; // Using bitmask smoothing, look it up

        if (HasSmoothableTile(position + Vector2Int.Up, tilemap)) {
            Bitmask += 2;
        }
        if (HasSmoothableTile(position + Vector2Int.Down, tilemap)) {
            Bitmask += 1;
        }
        if (HasSmoothableTile(position + Vector2Int.Right, tilemap)) {
            Bitmask += 4;
        }
        if (HasSmoothableTile(position + Vector2Int.Left, tilemap)) {
            Bitmask += 8;
        }
    }

    /// <summary>
    ///     Whether or not the tile at a certain <paramref name="position" /> can smooth with this tile.
    /// </summary>
    /// <param name="position">The position of the tile to check for smoothing.</param>
    /// <param name="tilemap">The tilemap on which the tile you want to check for smoothing is.</param>
    /// <returns>Whether or not the tile can smooth with this tile.</returns>
    private bool HasSmoothableTile(Vector2Int position, TilemapShared tilemap) {
        TileShared otherTile = tilemap.GetTile(position);
        if (SourceBlock.SmoothSelf) {
            return IsSameTileType(otherTile);
        }
        return
            otherTile != null &&
            otherTile.SourceBlock.BlockId != 0; // Don't smooth with air, possibly find nicer way to do this later.
    }

    /// <summary>
    ///     If the tile provided is the same type (references the same block) as the current tile.
    /// </summary>
    /// <param name="otherTile">The other tile to check.</param>
    /// <returns>Whether or not the other block is the same type as the current tile</returns>
    private bool IsSameTileType(TileShared otherTile) => otherTile?.SourceBlock == SourceBlock;
}