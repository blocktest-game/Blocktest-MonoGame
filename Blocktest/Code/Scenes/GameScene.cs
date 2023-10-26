using System.Linq;
using Blocktest.Block_System;
using Blocktest.Misc;
using Blocktest.Networking;
using Blocktest.Rendering;
using Microsoft.Xna.Framework.Input;
using Shared.Code;
using Shared.Code.Block_System;
using Shared.Code.Packets;
namespace Blocktest.Scenes;

public sealed class GameScene : IScene {
    private readonly RenderableTilemap _backgroundTilemapSprites;

    private readonly Camera _camera;
    private readonly bool _connect;
    private readonly RenderableTilemap _foregroundTilemapSprites;
    private readonly FrameCounter _frameCounter = new();
    private readonly BlocktestGame _game;
    private readonly Client _networkingClient;
    private readonly SpriteBatch _spriteBatch;

    private readonly WorldState _worldState = new();
    private int _blockSelected = 1; //ID of the block to place
    private readonly string[] _blockStrings;

    private bool _buildMode = true; //true for build, false for destroy

    private KeyboardState _previousKeyboardState;

    public GameScene(BlocktestGame game, bool doConnect, string? ip) {
        _connect = doConnect;
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _game = game;

        _camera = new Camera(Vector2.Zero, new Vector2(512, 256), game.GraphicsDevice);

        _backgroundTilemapSprites = new RenderableTilemap(_worldState.Foreground, _camera);
        _foregroundTilemapSprites = new RenderableTilemap(_worldState.Background, _camera);
        _networkingClient = new Client(_worldState, _camera);

        _blockStrings = BlockManagerShared.AllBlocks.Keys.ToArray();

        if (_connect && ip != null) {
            _networkingClient.Start(ip, 9050, "testKey");
            return;
        }

        WorldDownload testDownload = WorldDownload.Default();
        testDownload.Process(_worldState);
    }

    public void Update(GameTime gameTime) {
        if (_connect) {
            _networkingClient.Update();
        }

        HandleInput();

        _networkingClient.ClientTickBuffer.IncrCurrTick(_worldState);
    }

    public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice) {
        graphicsDevice.Clear(Color.CornflowerBlue);
        _camera.Draw(graphicsDevice, _spriteBatch);

        const bool pixelPerfect = false;

        Rectangle destinationRectangle = pixelPerfect ? GetPixelPerfectRect() : GetFitRect();
        _camera.RenderLocation = destinationRectangle;

        graphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(samplerState: pixelPerfect ? SamplerState.PointClamp : null);
        _spriteBatch.Draw(_camera.RenderTarget, destinationRectangle, Color.White);

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _frameCounter.Update(deltaTime);

        _spriteBatch.End();
    }

    public void EndScene() {
        _networkingClient.Stop();
    }

    public void HandleInput() {
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
            _buildMode = !_buildMode;
        }

        //Q changes which block you have selected
        if (currentKeyboardState.IsKeyDown(Keys.Q) &&
            _previousKeyboardState.IsKeyUp(Keys.Q)) {
            _blockSelected++;
            if (_blockSelected >= BlockManagerShared.AllBlocks.Count) {
                _blockSelected = 1;
            }
        }

        float moveValue = 2.5f;
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

        if (moveVector != Vector2.Zero) {
            _camera.Position += moveVector;

            MovePlayer movementPacket = new() {
                TickNum = _networkingClient.ClientTickBuffer.CurrTick,
                Position = (Vector2Int)_camera.Position,
                SourceId = _networkingClient.Server?.RemoteId ?? 0
            };
            _networkingClient.ClientTickBuffer.AddPacket(movementPacket);
            if (_connect) {
                _networkingClient.SendPacket(movementPacket);
            }
        }

        _previousKeyboardState = currentKeyboardState;

        if (currentMouseState.LeftButton != ButtonState.Pressed &&
            currentMouseState.RightButton != ButtonState.Pressed ||
            !_camera.RenderLocation.Contains(currentMouseState.Position)) {
            return;
        }

        bool foreground = currentMouseState.LeftButton == ButtonState.Pressed;
        Vector2 mousePos = _camera.CameraToWorldPos(currentMouseState.Position.ToVector2());
        Vector2Int tilePos = new(
            Math.Clamp((int)mousePos.X / GlobalsShared.GridSize.X, 0, GlobalsShared.MaxX),
            Math.Clamp((int)mousePos.Y / GlobalsShared.GridSize.Y, 0, GlobalsShared.MaxY));

        if (_buildMode) {
            TileChange testChange = new() {
                TickNum = _networkingClient.ClientTickBuffer.CurrTick,
                Position = tilePos,
                Foreground = foreground,
                BlockUid = _blockStrings[_blockSelected],
                SourceId = _networkingClient.Server?.RemoteId ?? 0
            };

            _networkingClient.ClientTickBuffer.AddPacket(testChange);
            if (_connect) {
                _networkingClient.SendPacket(testChange);
            }
        } else {
            BreakTile testBreak = new() {
                TickNum = _networkingClient.ClientTickBuffer.CurrTick,
                Position = tilePos,
                Foreground = foreground,
                SourceId = _networkingClient.Server?.RemoteId ?? 0
            };

            _networkingClient.ClientTickBuffer.AddPacket(testBreak);
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
}