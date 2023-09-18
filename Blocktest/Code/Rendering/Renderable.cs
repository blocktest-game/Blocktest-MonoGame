namespace Blocktest.Rendering; 

public sealed class Renderable {
    public readonly Transform Transform;
    public Drawable? Appearance;
    public Color RenderColor;

    public Renderable(Transform transform, Drawable? appearance = null, Color? renderColor = null) {
        Transform = transform;
        Appearance = appearance;
        RenderColor = renderColor ?? Color.White;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition) {
        if (Appearance == null) {
            return;
        }

        spriteBatch.Draw(Appearance.Texture, Transform.Position - cameraPosition, Appearance.Bounds, RenderColor, Transform.Rotation, Transform.Origin, Transform.Scale,
            SpriteEffects.None, 0);
    }
}