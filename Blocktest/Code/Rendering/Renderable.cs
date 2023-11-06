using Shared.Code.Components;
namespace Blocktest.Rendering;

public enum Layer {
    Top = 0,
    Player = 1,
    Default = 2,
    ForegroundBlocks = 3,
    BackgroundBlocks = 4
}

public sealed class Renderable {
    public readonly Transform Transform;
    public Drawable? Appearance;
    public Layer Layer;
    public Color RenderColor;

    public Renderable(Transform transform, Layer layer = Layer.Default, Drawable? appearance = null,
                      Color? renderColor = null) {
        Transform = transform;
        Layer = layer;
        Appearance = appearance;
        RenderColor = renderColor ?? Color.White;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition) {
        if (Appearance == null) {
            return;
        }

        spriteBatch.Draw(Appearance.Texture, Transform.Position - cameraPosition, Appearance.Bounds, RenderColor,
            Transform.Rotation, Transform.Origin, Transform.Scale,
            SpriteEffects.None, 0);
    }
}