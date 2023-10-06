using Blocktest.Block_System;
using Blocktest.Scenes;
using Shared.Code.Block_System;
namespace Blocktest;

/// <inheritdoc />
public sealed class BlocktestGame : Game {
    private readonly bool _connect;
    private readonly string? _ip;
    private IScene? _currentScene;
    private GraphicsDeviceManager _graphics;

    /// <inheritdoc />
    public BlocktestGame() {
        _connect = false;
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        TargetElapsedTime = TimeSpan.FromMilliseconds(16);
        Window.AllowUserResizing = true;
    }

    public BlocktestGame(string newIp) {
        _connect = true;
        _ip = newIp;
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        TargetElapsedTime = TimeSpan.FromMilliseconds(16);
    }

    public static ContentManager? ContentManager { get; private set; }

    /// <inheritdoc />
    protected override void Initialize() {
        BlockManagerShared.Initialize();
        base.Initialize();
    }

    /// <inheritdoc />
    protected override void LoadContent() {
        ContentManager = Content;
        BlockSpritesManager.LoadBlockSprites();
        _currentScene = new GameScene(this, _connect, _ip);
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