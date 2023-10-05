using Blocktest.Networking;
using LiteNetLib;
using Microsoft.Xna.Framework.Input;
using Shared.Networking;
namespace Blocktest.Scenes; 

public class GameScene : Scene {
    private readonly BlocktestGame _game;
    private readonly SpriteBatch _spriteBatch;
    private FrameCounter _frameCounter = new();
    private readonly SpriteFont _spriteFont;
    private readonly bool _connect;
    private Client _networkingClient;

    private bool _buildMode = true; //true for build, false for destroy
    private int _blockSelected = 1; //ID of the block to place

    private readonly TilemapSprites _foregroundTilemapSprites;
    private readonly TilemapSprites _backgroundTilemapSprites;
    
    private KeyboardState _previousKeyboardState;
    
    public GameScene(BlocktestGame game, bool doConnect, string? ip) {
        _connect = doConnect;
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _spriteFont = game.Content.Load<SpriteFont>("Fonts/OpenSans");
        _game = game;

        GlobalsShared.BackgroundTilemap = new TilemapShared(GlobalsShared.maxX, GlobalsShared.maxY);
        GlobalsShared.ForegroundTilemap = new TilemapShared(GlobalsShared.maxX, GlobalsShared.maxY);
        _backgroundTilemapSprites = new(GlobalsShared.BackgroundTilemap);
        _foregroundTilemapSprites = new(GlobalsShared.ForegroundTilemap);
        _networkingClient = new Client();

        if(_connect && ip != null)
        {
            _networkingClient.Start(ip, 9050, "testKey");
            return;
        }
        
        WorldDownload testDownload = new();

        int[,,] newWorld = new int[GlobalsShared.maxX, GlobalsShared.maxY, 2];
        for (int i = 0; i < GlobalsShared.maxX; i++) {
            newWorld[i, 59, 1] = 4;
            newWorld[i, 58, 1] = 2;
            newWorld[i, 57, 1] = 2;
            newWorld[i, 56, 1] = 2;
            newWorld[i, 55, 1] = 2;
            newWorld[i, 54, 1] = 3;
        }
        testDownload.world = newWorld;
        testDownload.tickNum = 1;
        testDownload.Process();

    }
    
    public void Update(GameTime gameTime) {
        var currentMouseState = Mouse.GetState();
        var currentKeyboardState = Keyboard.GetState();
    
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.Escape)) {
            _game.Exit();
        }

        if(_connect)
        {
            _networkingClient.Update();
        }


        //press E to toggle build/destroy
        if (currentKeyboardState.IsKeyDown(Keys.E) && _previousKeyboardState.IsKeyUp(Keys.E) && CheckWindowActive(currentMouseState)) {
	        _buildMode = !_buildMode;
        }

        //Q changes which block you have selected
        if (currentKeyboardState.IsKeyDown(Keys.Q) && _previousKeyboardState.IsKeyUp(Keys.Q) && CheckWindowActive(currentMouseState))
        {
	        _blockSelected++;
	        if (_blockSelected >= BlockManagerShared.AllBlocks.Length)
	        {
		        _blockSelected = 1;
	        }
        }

        //build and destroy mode
        if (_buildMode)
        {
	        if(currentMouseState.LeftButton == ButtonState.Pressed && CheckWindowActive(currentMouseState))
	        {
                TileChange testChange = new()
                {
                    tickNum = _networkingClient.clientTickBuffer.currTick,
                    position = new Vector2Int(MathHelper.Clamp(currentMouseState.X / GlobalsShared.gridSize.X, 0, GlobalsShared.maxX), MathHelper.Clamp(currentMouseState.Y / GlobalsShared.gridSize.Y, 0, GlobalsShared.maxY)),
                    foreground = true,
                    blockId = _blockSelected
                };
                _networkingClient.clientTickBuffer.AddPacket(testChange);
                if(_connect)
                {
                    _networkingClient.SendTileChange(testChange);
                }
	        } else if (currentMouseState.RightButton == ButtonState.Pressed && CheckWindowActive(currentMouseState)) {
                TileChange testChange = new()
                {
                    tickNum = _networkingClient.clientTickBuffer.currTick,
                    position = new Vector2Int(MathHelper.Clamp(currentMouseState.X / GlobalsShared.gridSize.X, 0, GlobalsShared.maxX), MathHelper.Clamp(currentMouseState.Y / GlobalsShared.gridSize.Y, 0, GlobalsShared.maxY)),
                    foreground = false,
                    blockId = _blockSelected
                };
                _networkingClient.clientTickBuffer.AddPacket(testChange);
                if(_connect)
                {
                    _networkingClient.SendTileChange(testChange);
                }
	        }
        }
        else 
        {
	        if(currentMouseState.LeftButton == ButtonState.Pressed && CheckWindowActive(currentMouseState))
	        {
                BreakTile testBreak = new()
                {
                    tickNum = _networkingClient.clientTickBuffer.currTick,
                    position = new Vector2Int(MathHelper.Clamp(currentMouseState.X / GlobalsShared.gridSize.X, 0, GlobalsShared.maxX), MathHelper.Clamp(currentMouseState.Y / GlobalsShared.gridSize.Y, 0, GlobalsShared.maxY)),
                    foreground = true
                };
                _networkingClient.clientTickBuffer.AddPacket(testBreak);
                if(_connect)
                {
                    _networkingClient.SendBreakTile(testBreak);
                }
	        } else if (currentMouseState.RightButton == ButtonState.Pressed && CheckWindowActive(currentMouseState)) {
                BreakTile testBreak = new()
                {
                    tickNum = _networkingClient.clientTickBuffer.currTick,
                    position = new Vector2Int(MathHelper.Clamp(currentMouseState.X / GlobalsShared.gridSize.X, 0, GlobalsShared.maxX), MathHelper.Clamp(currentMouseState.Y / GlobalsShared.gridSize.Y, 0, GlobalsShared.maxY)),
                    foreground = false
                };
                _networkingClient.clientTickBuffer.AddPacket(testBreak);
                if(_connect)
                {
                    _networkingClient.SendBreakTile(testBreak);
                }
	        }
        }
        _networkingClient.clientTickBuffer.IncrCurrTick();
        
        _previousKeyboardState = currentKeyboardState;
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

        if (_buildMode)
            _spriteBatch.Draw(BlockSpritesManager.AllBlocksSprites[_blockSelected].blockSprite.Texture,
            new Vector2Int(Mouse.GetState().X - (Mouse.GetState().X % 8),
                (Mouse.GetState().Y - Mouse.GetState().Y % 8)),
            new Rectangle(1, 1, 10, 10), Color.DimGray);

        _spriteBatch.End();
    }

    /// <summary>
    /// Checks if the window is active and the mouse is within the window.
    /// </summary>
    /// <param name="mouse">The current state of the mouse</param>
    /// <returns>True if the window is active and the mouse is within the window.</returns>
    private bool CheckWindowActive(MouseState mouse)
    {
        Point pos = new(mouse.X, mouse.Y);
        return _game.IsActive && _game.GraphicsDevice.Viewport.Bounds.Contains(pos);
    }
}