using System.Diagnostics;
using System.Linq;
using System.Net;
using Blocktest.Block_System;
using Blocktest.Misc;
using Blocktest.Networking;
using Blocktest.Rendering;
using Blocktest.UI;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using Shared.Code;
using Shared.Code.Block_System;
using Shared.Code.Components;
using Shared.Code.Packets;
namespace Blocktest.Scenes;

public sealed class GameScene : IScene {
    private readonly RenderableTilemap _backgroundTilemapSprites;
    private readonly string[] _blockStrings;

    private readonly Camera _camera;
    private readonly bool _connect;
    private readonly RenderableTilemap _foregroundTilemapSprites;
    private readonly FrameCounter _frameCounter = new();
    private readonly BlocktestGame _game;
    private readonly Desktop _gameDesktop;
    private readonly GameUI _gameUi;
    private readonly Client _networkingClient;
    private readonly SpriteBatch _spriteBatch;

    private readonly Vector2 _cameraPosition;
    private Vector2 _cameraStayPosition;

    private readonly WorldState _worldState = new();

    private KeyboardState _previousKeyboardState;
    public int BlockSelected = 1; //ID of the block to place

    public GameScene(BlocktestGame game, bool doConnect, IPEndPoint? ip) {
        _connect = doConnect;
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _game = game;

        _cameraPosition = Vector2.Zero;
        _camera = new Camera(_cameraPosition, new Vector2(640, 360), game.GraphicsDevice);

        _backgroundTilemapSprites = new RenderableTilemap(_worldState.Foreground, _camera);
        _foregroundTilemapSprites = new RenderableTilemap(_worldState.Background, _camera);
        _networkingClient = new Client(_worldState, _camera, game);

        _blockStrings = BlockManagerShared.AllBlocks.Keys.ToArray();

        _gameUi = new GameUI(this);
        _gameDesktop = new Desktop { Root = _gameUi };

        if (_connect && ip != null) {
            _networkingClient.Connect(ip);
            return;
        }

        //Add player to world in singleplayer
        Transform newTransform = new(new Vector2Int(256, 128));
        Renderable newRenderable = new(newTransform, Layer.Player, Drawable.PlaceholderDrawable);
        _worldState.PlayerPositions.Add(0, newTransform);
        _camera.RenderedComponents.Add(newRenderable);

        WorldDownload testDownload = WorldDownload.Default();
        testDownload.Process(_worldState);
    }

    public bool BuildMode { get; private set; } = true; //true for build, false for destroy

    public void Update(GameTime gameTime) {
        if (_connect) {
            _networkingClient.Update();
        }

        if (!_connect || _networkingClient.WorldDownloaded)
        {
            HandleInput();
        }

        _networkingClient.LocalTickBuffer.IncrCurrTick(_worldState);
    }

    public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice) {
        graphicsDevice.Clear(Color.CornflowerBlue);
        _camera.Draw(graphicsDevice, _spriteBatch);

        const bool pixelPerfect = true;

        Rectangle destinationRectangle = pixelPerfect ? GetPixelPerfectRect() : GetFitRect();
        _camera.RenderLocation = destinationRectangle;

        graphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_camera.RenderTarget, destinationRectangle, Color.White);

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _frameCounter.Update(deltaTime);

        _spriteBatch.End();

        _gameDesktop.Render();
    }

    public void EndScene() {
        _networkingClient.Stop();
    }

    private void HandleInput() {
        if (!_game.IsActive) {
            return;
        }

        MouseState currentMouseState = Mouse.GetState();
        KeyboardState currentKeyboardState = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            currentKeyboardState.IsKeyDown(Keys.Escape)) {
            _game.Exit();
        }

        //press E to toggle build/destroy
        if (currentKeyboardState.IsKeyDown(Keys.E) &&
            _previousKeyboardState.IsKeyUp(Keys.E)) {
            BuildMode = !BuildMode;
        }

        //Q changes which block you have selected
        if (currentKeyboardState.IsKeyDown(Keys.Q) &&
            _previousKeyboardState.IsKeyUp(Keys.Q)) {

            BlockSelected = (BlockSelected + 1) % BlockManagerShared.AllBlocks.Count;
            _gameUi.BlockSelector.SelectedIndex = BlockSelected;
        }

        float moveValue = 2;
        if (currentKeyboardState.IsKeyDown(Keys.LeftShift) || currentKeyboardState.IsKeyDown(Keys.RightShift)) {
            moveValue *= 4;
        }

        Vector2 moveVector = Vector2.Zero;
        if (currentKeyboardState.IsKeyDown(Keys.A)) {
            moveVector.X -= moveValue;
        } else if (currentKeyboardState.IsKeyDown(Keys.D)) {
            moveVector.X += moveValue;
        }

        if (currentKeyboardState.IsKeyDown(Keys.W)) {
            moveVector.Y += moveValue;
        } else if (currentKeyboardState.IsKeyDown(Keys.S)) {
            moveVector.Y -= moveValue;
        }

        int selfId = _networkingClient.Server?.RemoteId ?? 0;
        if (moveVector != Vector2.Zero) {
            //_camera.Position += moveVector;
            Debug.WriteLine(selfId);

            MovePlayer movementPacket = new() {
                TickNum = _networkingClient.LocalTickBuffer.CurrTick,
                //Position = (Vector2Int)_camera.Position,
                AddToPosition = (Vector2Int)moveVector,
                SourceId = selfId
            };
            _networkingClient.LocalTickBuffer.AddPacket(movementPacket);
            if (_connect) {
                _networkingClient.SendPacket(movementPacket);
            }
        }

        // allows free camera movement with lctrl, returns to player
        Vector2 cameraMoveVector = Vector2.Zero;
        if (currentKeyboardState.IsKeyDown(Keys.LeftControl))
        {
            if (_camera.RenderLocation.Contains(currentMouseState.Position))
            {
                cameraMoveVector.X = (currentMouseState.Position.X - _camera.RenderLocation.Center.X) / 10;
                cameraMoveVector.Y = -(currentMouseState.Position.Y - _camera.RenderLocation.Center.Y) / 10;
            }
            if (cameraMoveVector != Vector2.Zero)
            {
                _camera.Position += cameraMoveVector;
            }
        }
        else
        {
            _camera.Position.X = _worldState.PlayerPositions[selfId].Position.X - _camera.RenderTarget.Width / 2;
            _camera.Position.Y = _worldState.PlayerPositions[selfId].Position.Y - _camera.RenderTarget.Height / 2;
        }

        _previousKeyboardState = currentKeyboardState;

        if (currentMouseState.LeftButton != ButtonState.Pressed &&
            currentMouseState.RightButton != ButtonState.Pressed ||
            !_camera.RenderLocation.Contains(currentMouseState.Position) ||
            _gameDesktop.IsMouseOverGUI) {
            return;
        }

        bool foreground = currentMouseState.LeftButton == ButtonState.Pressed;
        Vector2 mousePos = _camera.CameraToWorldPos(currentMouseState.Position.ToVector2());
        Vector2Int tilePos = new(
            Math.Clamp((int)mousePos.X / GlobalsShared.GridSize.X, 0, GlobalsShared.MaxX),
            Math.Clamp((int)mousePos.Y / GlobalsShared.GridSize.Y, 0, GlobalsShared.MaxY));

        if (BuildMode) {
            TileChange testChange = new() {
                TickNum = _networkingClient.LocalTickBuffer.CurrTick,
                Position = tilePos,
                Foreground = foreground,
                BlockUid = _blockStrings[BlockSelected],
                SourceId = _networkingClient.Server?.RemoteId ?? 0
            };

            _networkingClient.LocalTickBuffer.AddPacket(testChange);
            if (_connect) {
                _networkingClient.SendPacket(testChange);
            }
        } else {
            BreakTile testBreak = new() {
                TickNum = _networkingClient.LocalTickBuffer.CurrTick,
                Position = tilePos,
                Foreground = foreground,
                SourceId = _networkingClient.Server?.RemoteId ?? 0
            };

            _networkingClient.LocalTickBuffer.AddPacket(testBreak);
            if (_connect) {
                _networkingClient.SendPacket(testBreak);
            }
        }
    }

    private Rectangle GetPixelPerfectRect() {
        int multiplier = int.Min(_game.GraphicsDevice.Viewport.Height / _camera.RenderTarget.Height,
            _game.GraphicsDevice.Viewport.Width / _camera.RenderTarget.Width);

        int width = _camera.RenderTarget.Width * multiplier;
        int height = _camera.RenderTarget.Height * multiplier;

        int x = (_game.GraphicsDevice.Viewport.Width - width) / 2;
        int y = (_game.GraphicsDevice.Viewport.Height - height) / 2;

        return new Rectangle(x, y, width, height);
    }

    private Rectangle GetFitRect() {
        float aspectRatio = (float)_game.GraphicsDevice.Viewport.Width / _game.GraphicsDevice.Viewport.Height;
        float renderTargetAspectRatio = (float)_camera.RenderTarget.Width / _camera.RenderTarget.Height;

        int width, height;
        if (aspectRatio > renderTargetAspectRatio) {
            width = (int)(_game.GraphicsDevice.Viewport.Height * renderTargetAspectRatio);
            height = _game.GraphicsDevice.Viewport.Height;
        } else {
            width = _game.GraphicsDevice.Viewport.Width;
            height = (int)(_game.GraphicsDevice.Viewport.Width / renderTargetAspectRatio);
        }

        int x = (_game.GraphicsDevice.Viewport.Width - width) / 2;
        int y = (_game.GraphicsDevice.Viewport.Height - height) / 2;

        return new Rectangle(x, y, width, height);
    }

    public void SetBuildMode(bool buildMode) {
        BuildMode = buildMode;
    }
}