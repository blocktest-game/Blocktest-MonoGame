using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
namespace Shared.Code.Components;

public sealed class Transform : INetSerializable {
    public Vector2Int Origin;
    public Vector2Int Position;
    public float Rotation;
    public Vector2 Scale;

    public Transform(Vector2Int position, Vector2? scale = null, Vector2Int? origin = null, float rotation = 0) {
        Position = position;
        Origin = origin ?? Vector2Int.Zero;
        Scale = scale ?? Vector2.One;
        Rotation = rotation;
    }

    public void Serialize(NetDataWriter writer) {
        writer.Put(Position);
        writer.Put(Rotation);
        writer.Put(Scale.X);
        writer.Put(Scale.Y);
    }

    public void Deserialize(NetDataReader reader) {
        Position = reader.Get<Vector2Int>();
        Rotation = reader.GetFloat();
        Scale = new Vector2(reader.GetFloat(), reader.GetFloat());
    }
}