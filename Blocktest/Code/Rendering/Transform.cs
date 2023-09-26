namespace Blocktest.Rendering; 

public sealed class Transform {
    public Vector2 Position;
    public float Rotation;
    public Vector2 Scale;
    public Vector2 Origin;
    
    public Transform(Vector2 position, Vector2? scale = null, float rotation = 0, Vector2? origin = null) {
        Position = position;
        Scale = scale ?? Vector2.One;
        Rotation = rotation;
        Origin = origin ?? Vector2.Zero;
    }
}