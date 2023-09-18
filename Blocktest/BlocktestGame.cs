using Blocktest.Rendering;
using Blocktest.Scenes;

namespace Blocktest
{
    /// <inheritdoc />
    public class BlocktestGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private Scene? _currentScene;

        /// <inheritdoc />
        public BlocktestGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        /// <inheritdoc />
        protected override void Initialize() {
            BlockManager.Initialize();
            base.Initialize();
        }

        /// <inheritdoc />
        protected override void LoadContent()
        {
            Drawable.ContentManager = Content;
            BlockManager.LoadBlockSprites(Content);
            _currentScene = new GameScene(this);
        }

        /// <inheritdoc />
        protected override void Update(GameTime gameTime)
        {
            _currentScene?.Update(gameTime);
            
            base.Update(gameTime);
        }

        /// <inheritdoc />
        protected override void Draw(GameTime gameTime)
        {
            _currentScene?.Draw(gameTime, GraphicsDevice);
            
            base.Draw(gameTime);
        }
    }
}
