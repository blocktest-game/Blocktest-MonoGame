using Blocktest.Rendering;
using Blocktest.Scenes;

namespace Blocktest
{
    /// <inheritdoc />
    public class BlocktestGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private Scene? _currentScene;
        private bool connect;
        private string? ip;
        public static ContentManager? ContentManager { get; private set; }



        /// <inheritdoc />
        public BlocktestGame()
        {
            connect = false;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(16);
        }

        public BlocktestGame(string newIp)
        {
            connect = true;
            ip = newIp;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(16);
        }

        /// <inheritdoc />
        protected override void Initialize() {
            BlockManagerShared.Initialize();
            base.Initialize();
        }

        /// <inheritdoc />
        protected override void LoadContent()
        {
            ContentManager = Content;
            BlockSpritesManager.LoadBlockSprites(Content);
            _currentScene = new GameScene(this, connect, ip);
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
