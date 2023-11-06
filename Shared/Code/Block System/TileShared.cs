using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using Shared.Code.Components;
namespace Shared.Code.Block_System;

/// <summary>
///     A <see cref="TilemapShared" /> is filled with tile instances, one for each grid square.
///     They contain basic information such as name and sprite, but the behaviours and more advanced properties are found
///     in the correlating Block classes.
/// </summary>
public class TileShared : INetSerializable {
    public readonly Transform Transform;

    /// <summary>
    ///     Color of the tile.
    /// </summary>
    public Color Color = Color.White;

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
        Transform = new Transform(new Vector2Int(GlobalsShared.GridSize.X * position.X,
            GlobalsShared.GridSize.Y * position.Y));
    }

    public void Serialize(NetDataWriter writer) {
        writer.Put(Color.R);
        writer.Put(Color.G);
        writer.Put(Color.B);
        writer.Put(Transform);
        writer.Put(SourceBlock.BlockUid);
    }

    public void Deserialize(NetDataReader reader) {
        Color = new Color(reader.GetByte(), reader.GetByte(), reader.GetByte());
        Transform.Deserialize(reader);
        SourceBlock = BlockManagerShared.AllBlocks[reader.GetString()];
    }
}