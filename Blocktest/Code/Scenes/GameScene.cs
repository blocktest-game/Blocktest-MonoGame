using Blocktest.Block_System;
using Blocktest.Misc;
using Blocktest.Networking;
using Microsoft.Xna.Framework.Input;
using Shared.Code;
using Shared.Code.Block_System;
using Shared.Code.Packets;
namespace Blocktest.Scenes;

public sealed class GameScene : IScene {
    private readonly TilemapSprites _backgroundTilemapSprites;
    private readonly bool _connect;

    private readonly TilemapSprites _foregroundTilemapSprites;
    private readonly FrameCounter _frameCounter = new();
    private readonly BlocktestGame _game;
    private readonly Client _networkingClient;
    private readonly SpriteBatch _spriteBatch;
    private readonly SpriteFont _spriteFont;
    private int _blockSelected = 1; //ID of the block to place

    private bool _buildMode = true; //true for build, false for destroy

    private KeyboardState _previousKeyboardState;

    public GameScene(BlocktestGame game, bool doConnect, string? ip) {
        _connect = doConnect;
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _spriteFont = game.Content.Load<SpriteFont>("Fonts/OpenSans");
        _game = game;

        GlobalsShared.BackgroundTilemap = new TilemapShared(GlobalsShared.MaxX, GlobalsShared.MaxY);
        GlobalsShared.ForegroundTilemap = new TilemapShared(GlobalsShared.MaxX, GlobalsShared.MaxY);
        _backgroundTilemapSprites = new TilemapSprites(GlobalsShared.BackgroundTilemap);
        _foregroundTilemapSprites = new TilemapSprites(GlobalsShared.ForegroundTilemap);
        _networkingClient = new Client();

        if (_connect && ip != null) {
            _networkingClient.Start(ip, 9050, "testKey");
            return;
        }

        WorldDownload testDownload = new();

        int[,,] newWorld = new int[GlobalsShared.MaxX, GlobalsShared.MaxY, 2];
        for (int i = 0; i < GlobalsShared.MaxX; i++) {
            newWorld[i, 59, 1] = 4;
            newWorld[i, 58, 1] = 2;
            newWorld[i, 57, 1] = 2;
            newWorld[i, 56, 1] = 2;
            newWorld[i, 55, 1] = 2;
            newWorld[i, 54, 1] = 3;
        }
        testDownload.World = newWorld;
        testDownload.TickNum = 1;
        testDownload.Process();

    }

    public void Update(GameTime gameTime) {
        MouseState currentMouseState = Mouse.GetState();
        KeyboardState currentKeyboardState = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            currentKeyboardState.IsKeyDown(Keys.Escape)) {
            _game.Exit();
        }

        if (_connect) {
            _networkingClient.Update();
        }
        
        _networkingClient.ClientTickBuffer.IncrCurrTick();

        if (!CheckWindowActive(currentMouseState)) {
            return;
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
            if (_blockSelected >= BlockManagerShared.AllBlocks.Length) {
                _blockSelected = 1;
            }
        }

        

        _previousKeyboardState = currentKeyboardState;

        if (currentMouseState.LeftButton != ButtonState.Pressed &&
            currentMouseState.RightButton != ButtonState.Pressed) {
            return;
        }

        bool foreground = currentMouseState.LeftButton == ButtonState.Pressed;
        Vector2Int tilePos = new Vector2Int(
            MathHelper.Clamp(currentMouseState.X / GlobalsShared.GridSize.X, 0, GlobalsShared.MaxX),
            MathHelper.Clamp(currentMouseState.Y / GlobalsShared.GridSize.Y, 0, GlobalsShared.MaxY));

        if (_buildMode) {
            TileChange testChange = new() {
                TickNum = _networkingClient.ClientTickBuffer.CurrTick,
                Position = tilePos,
                Foreground = foreground,
                BlockId = _blockSelected
            };

            _networkingClient.ClientTickBuffer.AddPacket(testChange);
            if (_connect) {
                _networkingClient.SendTileChange(testChange);
            }
        } else {
            BreakTile testBreak = new() {
                TickNum = _networkingClient.ClientTickBuffer.CurrTick,
                Position = tilePos,
                Foreground = foreground
            };

            _networkingClient.ClientTickBuffer.AddPacket(testBreak);
            if (_connect) {
                _networkingClient.SendBreakTile(testBreak);
            }
        }
    }

    public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice) {
        graphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _backgroundTilemapSprites.DrawAllTiles(_spriteBatch);
        _foregroundTilemapSprites.DrawAllTiles(_spriteBatch);

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _frameCounter.Update(deltaTime);

        string fps = $"FPS: {_frameCounter.CurrentFramesPerSecond:##0.00}";
        _spriteBatch.DrawString(_spriteFont, fps, new Vector2(10, 10), Color.Black);

        if (_buildMode) {
            _spriteBatch.Draw(BlockSpritesManager.AllBlocksSprites[_blockSelected].BlockSprite.Texture,
                new Vector2Int(Mouse.GetState().X - Mouse.GetState().X % 8,
                    Mouse.GetState().Y - Mouse.GetState().Y % 8),
                new Rectangle(1, 1, 10, 10), Color.DimGray);
        }

        _spriteBatch.End();
    }

    public void EndScene() {
        _networkingClient.Stop();
    }

    /// <summary>
    ///     Checks if the window is active and the mouse is within the window.
    /// </summary>
    /// <param name="mouse">The current state of the mouse</param>
    /// <returns>True if the window is active and the mouse is within the window.</returns>
    private bool CheckWindowActive(MouseState mouse) {
        return _game.IsActive && _game.GraphicsDevice.Viewport.Bounds.Contains(mouse.Position);
    }
}