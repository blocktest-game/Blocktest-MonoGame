namespace Blocktest.Scenes; 

public interface Scene {
    public void Update(GameTime gameTime);

    public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice);
}