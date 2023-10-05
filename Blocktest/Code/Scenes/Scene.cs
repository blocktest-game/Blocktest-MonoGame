namespace Blocktest.Scenes;

public interface IScene {
    public void Update(GameTime gameTime);

    public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice);

    public void EndScene();
}