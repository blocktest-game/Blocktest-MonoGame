using Blocktest.Rendering;
using Shared.Code;
using Shared.Code.Block_System;
namespace Blocktest.Block_System;

public class RenderableTilemap {
    private readonly TilemapShared _tilemap;

    private readonly RenderableTile[,] _renderables;
    private readonly Camera _camera;

    /// <summary>
    ///     A list of <see cref="Vector2Int" />s that specify which blocks should be refreshed when a tile is placed/destroyed.
    ///     Defaults to the changed block and all cardinal directions.
    /// </summary>
    private static readonly List<Vector2Int> Adjacencies = new()
        { Vector2Int.Zero, Vector2Int.Up, Vector2Int.Down, Vector2Int.Left, Vector2Int.Right };


    public RenderableTilemap(TilemapShared newTilemap, Camera camera) {
        _tilemap = newTilemap;
        _camera = camera;
        _renderables = new RenderableTile[_tilemap.TilemapSize.X, _tilemap.TilemapSize.Y];
        UpdateRenderables();
        newTilemap.OnTileChanged += OnTilemapChanged;
    }

    private void OnTilemapChanged(TileShared tile, Vector2Int location) {
        _camera.RenderedComponents.Remove(_renderables[location.X, location.Y].Renderable);

        foreach (Vector2Int dir in Adjacencies) {
            if (location.X + dir.X < 0 ||
                location.X + dir.X >= _tilemap.TilemapSize.X ||
                location.Y + dir.Y < 0 ||
                location.Y + dir.Y >= _tilemap.TilemapSize.Y) {
                continue;
            }
            _renderables[location.X + dir.X, location.Y + dir.Y].UpdateAdjacencies(location + dir, _tilemap);
        }

        RenderableTile newTile = new RenderableTile(tile, _tilemap.Background);
        _renderables[location.X, location.Y] = newTile;
        newTile.UpdateAdjacencies(location, _tilemap);
        _camera.RenderedComponents.Add(newTile.Renderable);
    }

    private void UpdateRenderables() {
        _camera.RenderedComponents.Clear();
        for (int x = 0; x < _tilemap.TilemapSize.X; x++)
        for (int y = 0; y < _tilemap.TilemapSize.Y; y++) {
            if (!_tilemap.TryGetTile(new Vector2Int(x, y), out TileShared? tile)) {
                continue;
            }
            RenderableTile newTile = new RenderableTile(tile, _tilemap.Background);
            _renderables[x, y] = newTile;
            _camera.RenderedComponents.Add(newTile.Renderable);
            newTile.UpdateAdjacencies(new Vector2Int(x, y), _tilemap);
        }
    }
}