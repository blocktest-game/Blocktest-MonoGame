using System.Net;
using Blocktest.Block_System;
using Blocktest.Scenes;
using Myra;
using Shared.Code.Block_System;
namespace Blocktest;

/// <inheritdoc />
public sealed class BlocktestGame : Game {
    private readonly bool _connect;
    private readonly IPEndPoint? _ip;
    private IScene? _currentScene;
    private GraphicsDeviceManager _graphics;

    public readonly Version? BlocktestVersion = typeof(BlocktestGame).Assembly.GetName().Version;

    /// <inheritdoc />
    public BlocktestGame(IPEndPoint? newIp = null) {
        _connect = newIp != null;
        _ip = newIp;
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        TargetElapsedTime = TimeSpan.FromMilliseconds(16);
        MyraEnvironment.Game = this;
    }

    public static ContentManager? ContentManager { get; private set; }

    public void SetScene(IScene newScene) {
        _currentScene?.EndScene();
        _currentScene = newScene;
    }

    /// <inheritdoc />
    protected override void Initialize() {
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.ApplyChanges();
        
        BlockManagerShared.Initialize();
        base.Initialize();
    }

    /// <inheritdoc />
    protected override void LoadContent() {
        ContentManager = Content;
        BlockSpritesManager.LoadBlockSprites();

        if (_ip != null) {
            SetScene(new GameScene(this, true, _ip));
            return;
        }

#if DEBUG
        SetScene(new GameScene(this, _connect, _ip));
#else
        SetScene(new MainMenuScene(this));
#endif
    }

    /// <inheritdoc />
    protected override void Update(GameTime gameTime) {
        _currentScene?.Update(gameTime);

        base.Update(gameTime);
    }

    /// <inheritdoc />
    protected override void Draw(GameTime gameTime) {
        _currentScene?.Draw(gameTime, GraphicsDevice);

        base.Draw(gameTime);
    }

    protected override void OnExiting(object sender, EventArgs args) {
        _currentScene?.EndScene();

        base.OnExiting(sender, args);
    }
}