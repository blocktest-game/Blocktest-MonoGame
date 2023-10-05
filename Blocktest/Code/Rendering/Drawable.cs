namespace Blocktest.Rendering;

public sealed class Drawable {
	public readonly Rectangle Bounds;
	public readonly Texture2D Texture;

	public Drawable(string fileName, Rectangle? bounds = null) {
		// The content manager caches the textures, so we don't need to worry about loading the same texture multiple times.
		Texture = BlocktestGame.ContentManager?.Load<Texture2D>(fileName) ?? throw new Exception($"Could not load drawable {fileName}, content manager not initialized.");
		Bounds = bounds ?? Texture.Bounds;
	}
}