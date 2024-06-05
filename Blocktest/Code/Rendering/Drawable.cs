using Myra.Graphics2D;

namespace Blocktest.Rendering;

public sealed class Drawable : IImage {
    public readonly Rectangle Bounds;
    public readonly Texture2D Texture;

    public Drawable(string fileName, Rectangle? bounds = null) {
        // The content manager caches the textures, so we don't need to worry about loading the same texture multiple times.
        Texture = BlocktestGame.ContentManager?.Load<Texture2D>(fileName) ??
                  throw new Exception($"Could not load drawable {fileName}, content manager not initialized.");
        Bounds = bounds ?? Texture.Bounds;
    }

    public static Drawable ErrorDrawable { get; } = new(@"Graphics\Blocks\error");
    public static Drawable PlaceholderDrawable { get; } = new(@"Graphics\Player\placeholder-base");

    public void Draw(RenderContext context, Rectangle dest, Color color) {
        context.Draw(Texture, dest, Bounds, color);
    }

    public Point Size => Bounds.Size;
}