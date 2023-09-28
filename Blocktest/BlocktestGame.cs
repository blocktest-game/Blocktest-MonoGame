using Blocktest.Rendering;
using Blocktest.Scenes;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Blocktest
{
    /// <inheritdoc />
    public class BlocktestGame : Game
    {
        private OrthographicCamera _camera;
        private GraphicsDeviceManager _graphics;
        private Scene? _currentScene;
        private bool connect;
        private string ip;



        /// <inheritdoc />
        public BlocktestGame()
        {
            connect = false;
            ip = "";
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

            BoxingViewportAdapter viewportAdapter = new(Window, GraphicsDevice, 800, 480);
            _camera = new(viewportAdapter);
            Console.WriteLine("Camera init");
            Drawable.ContentManager = Content;
            BlockSpritesManager.LoadBlockSprites(Content);
            Console.WriteLine("LoadContent");
            _currentScene = new GameScene(this, _camera, connect, ip);
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
