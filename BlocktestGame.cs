using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Blocktest
{
    /// <inheritdoc />
    public class BlocktestGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        /// <inheritdoc />
        public BlocktestGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            Globals.Game = this;

            BlockManager.Initialize();

            Globals.foreground = new Tilemap(Globals.maxX, Globals.maxY);
            Globals.background = new Tilemap(Globals.maxX, Globals.maxY);

            base.Initialize();
        }

        /// <inheritdoc />
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <inheritdoc />
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Exit();
            }

            base.Update(gameTime);
        }

        /// <inheritdoc />
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            Globals.foreground.Draw(_spriteBatch);
            Globals.background.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
