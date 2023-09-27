using Blocktest.Rendering;
namespace Blocktest
{
    /// <summary>
    /// A grid filled with <see cref="Tile"/>s, usually representing terrain.
    /// </summary>
    public class Tilemap
    {
        /// <summary>
        /// The 2D array of all tiles in the tilemap.
        /// </summary>
        public Tile?[,] tileGrid;
        /// <summary>
        /// A list of all the tiles currently on the tilemap.
        /// </summary>
        private readonly List<Tile> allTiles = new();
        /// <summary>
        /// The size of the tilemap in tiles.
        /// </summary>
        public readonly Vector2Int tilemapSize;
        /// <summary>
        /// A list of <see cref="Vector2Int"/>s that specify which blocks should be refreshed when a tile is placed/destroyed. Defaults to the changed block and all cardinal directions.
        /// </summary>
        private static readonly List<Vector2Int> adjacencies = new() { Vector2Int.Zero, Vector2Int.Up, Vector2Int.Down, Vector2Int.Left, Vector2Int.Right };

        private readonly Camera _camera;

        /// <summary>
        /// Creates a <see cref="Tilemap"/>.
        /// </summary>
        /// <param name="sizeX">The width of the tilemap in tiles.</param>
        /// <param name="sizeY">The height of the tilemap in tiles.</param>
        /// <param name="camera">The camera to render tiles on</param>
        public Tilemap(int sizeX, int sizeY, Camera camera)
        {
            _camera = camera;
            tilemapSize = new(sizeX, sizeY);
            tileGrid = new Tile[sizeX, sizeY];
        }

        /// <summary>
        /// Sets a Tile at the given XYZ coordinates of a cell in the tile map to a specific <see cref="Block"/> type.
        /// </summary>
        /// <param name="location">Location the new Block will be placed.</param>
        /// <param name="newTile">Block type to be placed in the cell.</param>
        public Tile SetTile(Vector2Int location, Tile newTile)
        {
            Tile oldTile = GetTile(location);
            if (oldTile != null) {
                allTiles.Remove(oldTile);
                _camera.RenderedComponents.Remove(oldTile.Renderable);
            }

            tileGrid[location.X, location.Y] = newTile;

            if (newTile != null) {
                allTiles.Add(newTile);
                _camera.RenderedComponents.Add(newTile.Renderable);
            }

            foreach (Vector2Int dir in adjacencies) {
                if (location.X + dir.X < 0 || location.X + dir.X >= tilemapSize.X || location.Y + dir.Y < 0 || location.Y + dir.Y >= tilemapSize.Y) { continue; }
                tileGrid[location.X + dir.X, location.Y + dir.Y]?.UpdateAdjacencies(location + dir, this);
            }

            return newTile;
        }

        /// <summary>
        /// Deletes a <see cref="Tile"/> at a specific location(sets value to null).
        /// </summary>
        /// <param name="location"></param>
        public void DeleteTile(Vector2Int location) => SetTile(location, null);

        /// <summary>
        /// Gets the <see cref="Tile"/> at a specific location on a <see cref="Tilemap"/>.
        /// </summary>
        /// <param name="location">Location of the Tile on the Tilemap to check.</param>
        /// <returns><see cref="Tile"/> placed at the cell.</returns>
        public Tile? GetTile(Vector2Int location)  {
            if (location.X < 0 || location.Y < 0 || location.X >= tilemapSize.X || location.Y >= tilemapSize.Y) {
                return null;
            }
            return tileGrid[location.X, location.Y];
        }

        /// <summary>
        /// Returns whether there is a <see cref="Tile"/> at the location specified.
        /// </summary>
        /// <param name="location">Location to check.</param>
        /// <returns>Returns true if there is a Tile at the position. Returns false otherwise.</returns>
        public bool HasTile(Vector2Int location) => tileGrid[location.X, location.Y] != null;
    }

    /// <summary>
    /// A <see cref="Tilemap"/> is filled with tile instances, one for each grid square. 
    /// They contain basic information such as name and sprite, but the behaviours and more advanced properties are found in the correlating Block classes.
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// The type of block this tile is.
        /// </summary>
        public Block SourceBlock;
        /// <summary>
        /// Sprite to be rendered at the Tile.
        /// </summary>
        protected Drawable sprite;
        /// <summary>
        /// The size of the tile square's edges, in pixels (Default 8) 
        /// </summary>
        protected int size = 8;

        public Renderable Renderable;

        /// <summary>
        /// Creates a <see cref="Tile"/>.
        /// </summary>
        /// <param name="newBlock">The type of block the new tile should be.</param>
        /// <param name="position">The position in a tilemap the tile will be.</param>
        public Tile(Block newBlock, Vector2Int position, Layer layer = Layer.ForegroundBlocks)
        {
            SourceBlock = newBlock;
            Renderable = new Renderable(new Transform(new Vector2(Globals.gridSize.X * position.X, Globals.gridSize.Y * position.Y)), layer, SourceBlock.blockSprite);
        }

        /// <summary>
        /// This method is called whenever an adjacent (according to a tilemap's adjacency variable) tile is placed or removed. Used for smoothing.
        /// </summary>
        /// <param name="position">The position of the current tile.</param>
        /// <param name="tilemap">The tilemap the tile is on.</param>
        public void UpdateAdjacencies(Vector2Int position, Tilemap tilemap)
        {
            if (!SourceBlock.blockSmoothing || (SourceBlock.spriteSheet == null)) { return; } // If the tile doesn't or can't smooth, don't even try

            int bitmask = 0; // Using bitmask smoothing, look it up

            if (HasSmoothableTile(position + Vector2Int.Up, tilemap)) {
                bitmask += 1;
            }
            if (HasSmoothableTile(position + Vector2Int.Down, tilemap)) {
                bitmask += 2;
            }
            if (HasSmoothableTile(position + Vector2Int.Right, tilemap)) {
                bitmask += 4;
            }
            if (HasSmoothableTile(position + Vector2Int.Left, tilemap)) {
                bitmask += 8;
            }

            Renderable.Appearance = SourceBlock.spriteSheet.OrderedSprites[bitmask];
        }

        /// <summary>
        /// Whether or not the tile at a certain <paramref name="position"/> can smooth with this tile.
        /// </summary>
        /// <param name="position">The position of the tile to check for smoothing.</param>
        /// <param name="tilemap">The tilemap on which the tile you want to check for smoothing is.</param>
        /// <returns>Whether or not the tile can smooth with this tile.</returns>
        private bool HasSmoothableTile(Vector2Int position, Tilemap tilemap)
        {
            Tile otherTile = tilemap.GetTile(position);
            if (SourceBlock.smoothSelf) { return IsSameTileType(otherTile); }
            return otherTile != null;
        }

        /// <summary>
        /// If the tile provided is the same type (references the same block) as the current tile.
        /// </summary>
        /// <param name="otherTile">The other tile to check.</param>
        /// <returns>Whether or not the other block is the same type as the current tile</returns>
        private bool IsSameTileType(Tile otherTile) => otherTile?.SourceBlock == SourceBlock;
    }

}
