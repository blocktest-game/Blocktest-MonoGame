namespace Blocktest.Rendering; 

public sealed class Transform {
    public Vector2 Position;
    public float Rotation;
    public Vector2 Scale;
    
    public Transform(Vector2 position, Vector2? scale = null, float rotation = 0) {
        Position = position;
        Scale = scale ?? Vector2.One;
        Rotation = rotation;
    }

    public Vector2 Origin => new(Scale.X / 2, Scale.Y / 2);
}