using Blocktest.Networking;
using LiteNetLib;
using Microsoft.Xna.Framework.Input;
using Shared.Networking;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
namespace Blocktest.Scenes; 

public class GameScene : Scene {
    private readonly BlocktestGame _game;
    private OrthographicCamera _camera;
    private readonly SpriteBatch _spriteBatch;
    private FrameCounter _frameCounter = new FrameCounter();
    private readonly SpriteFont _spriteFont;
    private bool connect;
    private Client networkingClient = new();
    
    bool latch = false; //latch for button pressing
    private bool latchBlockSelect = false; //same but for block selection
    bool buildMode = true; //true for build, false for destroy
    private int blockSelected = 1; //ID of the block to place
    
    public void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                _game.Exit();
            }

            if(connect)
            {
                networkingClient.Update();
            }

            //for block placement
            MouseState currentState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
            Vector2 _worldPosition = _camera.ScreenToWorld(new Vector2(currentState.X, currentState.Y));

            _camera.Move(CameraMovement(keyboardState));

            //press E to toggle build/destroy
            if (keyboardState.IsKeyUp(Keys.E))
            {
	            latch = false;
            } 
            else if (latch == false && CheckWindowActive(currentState))
            {
	            buildMode = !buildMode;
	            latch = true;
            }

            //Q changes which block you have selected
            if (keyboardState.IsKeyUp(Keys.Q))
            {
	            latchBlockSelect = false;
            }
            else if (latchBlockSelect == false && CheckWindowActive(currentState))
            {
	            blockSelected++;
	            if (blockSelected >= BlockManagerShared.AllBlocks.Length)
	            {
		            blockSelected = 1;
	            }

	            latchBlockSelect = true;
            }

            //build and destroy mode
            if (buildMode)
            {
	            if(currentState.LeftButton == ButtonState.Pressed && CheckWindowActive(currentState))
	            {
                    TileChange testChange = new()
                    {
                        tickNum = Globals.clientTickBuffer.currTick,
                        position = new Vector2Int(MathHelper.Clamp(_worldPosition.X / GlobalsShared.gridSize.X, 0, GlobalsShared.maxX), MathHelper.Clamp(_worldPosition.Y / GlobalsShared.gridSize.Y, 0, GlobalsShared.maxY)),
                        foreground = true,
                        blockId = blockSelected
                    };
                    Globals.clientTickBuffer.AddPacket(testChange);
                    if(connect)
                    {
                        networkingClient.SendTileChange(testChange);
                    }
	            } else if (currentState.RightButton == ButtonState.Pressed && CheckWindowActive(currentState)) {
                    TileChange testChange = new()
                    {
                        tickNum = Globals.clientTickBuffer.currTick,
                        position = new Vector2Int(MathHelper.Clamp(_worldPosition.X / GlobalsShared.gridSize.X, 0, GlobalsShared.maxX), MathHelper.Clamp(_worldPosition.Y / GlobalsShared.gridSize.Y, 0, GlobalsShared.maxY)),
                        foreground = false,
                        blockId = blockSelected
                    };
                    Globals.clientTickBuffer.AddPacket(testChange);
                    if(connect)
                    {
                        networkingClient.SendTileChange(testChange);
                    }
	            }
            }
            else 
            {
	            if(currentState.LeftButton == ButtonState.Pressed && CheckWindowActive(currentState))
	            {
                    BreakTile testBreak = new()
                    {
                        tickNum = Globals.clientTickBuffer.currTick,
                        position = new Vector2Int(MathHelper.Clamp(_worldPosition.X / GlobalsShared.gridSize.X, 0, GlobalsShared.maxX), MathHelper.Clamp(_worldPosition.Y / GlobalsShared.gridSize.Y, 0, GlobalsShared.maxY)),
                        foreground = true
                    };
                    Globals.clientTickBuffer.AddPacket(testBreak);
                    if(connect)
                    {
                        networkingClient.SendBreakTile(testBreak);
                    }
	            } else if (currentState.RightButton == ButtonState.Pressed && CheckWindowActive(currentState)) {
                    BreakTile testBreak = new()
                    {
                        tickNum = Globals.clientTickBuffer.currTick,
                        position = new Vector2Int(MathHelper.Clamp(_worldPosition.X / GlobalsShared.gridSize.X, 0, GlobalsShared.maxX), MathHelper.Clamp(_worldPosition.Y / GlobalsShared.gridSize.Y, 0, GlobalsShared.maxY)),
                        foreground = false
                    };
                    Globals.clientTickBuffer.AddPacket(testBreak);
                    if(connect)
                    {
                        networkingClient.SendBreakTile(testBreak);
                    }
	            }
            }
        Globals.clientTickBuffer.IncrCurrTick();
    }

    public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice) {
        graphicsDevice.Clear(Color.CornflowerBlue);

        if(_camera == null)
        {
            Console.WriteLine("Camera is null");
        }
        if(_spriteBatch == null)
        {
            Console.WriteLine("SpriteBatch is null");
        }
        
        _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());

        Globals.BackgroundTilemapSprites.DrawAllTiles(_spriteBatch);
        Globals.ForegroundTilemapSprites.DrawAllTiles(_spriteBatch);

        /*
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _frameCounter.Update(deltaTime);

        String fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);

        //Console.WriteLine(fps);
        */

        MouseState currentState = Mouse.GetState();
        Vector2 _worldPosition = _camera.ScreenToWorld(new Vector2(currentState.X, currentState.Y));

        if (buildMode)
            _spriteBatch.Draw(BlockSpritesManager.AllBlocksSprites[blockSelected].blockSprite.Texture,
            new Vector2Int(_worldPosition.X - (_worldPosition.X % 8),
                (_worldPosition.Y - _worldPosition.Y % 8)),
            new Rectangle(1, 1, 10, 10), Color.DimGray);

        _spriteBatch.End();
    }

    public GameScene(BlocktestGame game, OrthographicCamera newCamera, bool doConnect, string ip) {
        connect = doConnect;
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _game = game;
        _camera = newCamera;

        GlobalsShared.BackgroundTilemap = new TilemapShared(GlobalsShared.maxX, GlobalsShared.maxY);
        GlobalsShared.ForegroundTilemap = new TilemapShared(GlobalsShared.maxX, GlobalsShared.maxY);
        Globals.BackgroundTilemapSprites = new(GlobalsShared.BackgroundTilemap);
        Globals.ForegroundTilemapSprites = new(GlobalsShared.ForegroundTilemap);

        if(connect)
        {
            //networkingClient.Start("localhost", 9050, "testKey");
            networkingClient.Start(ip, 9050, "testKey");
        }
        else
        {
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
        
    }

    /// <summary>
    /// Checks if the window is active and the mouse is within the window.
    /// </summary>
    /// <param name="mouse">The current state of the mouse</param>
    /// <returns>True if the window is active and the mouse is within the window.</returns>
    private bool CheckWindowActive(MouseState mouse)
    {
        Point pos = new(mouse.X, mouse.Y);
        if(_game.IsActive && _game.GraphicsDevice.Viewport.Bounds.Contains(pos))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Vector2 CameraMovement(KeyboardState keyboardState)
    {
        Vector2 movementDirection = Vector2.Zero;
        if (keyboardState.IsKeyDown(Keys.S))
        {
            movementDirection += Vector2.UnitY;
        }
        if (keyboardState.IsKeyDown(Keys.W))
        {
            movementDirection -= Vector2.UnitY;
        }
        if (keyboardState.IsKeyDown(Keys.A))
        {
            movementDirection -= Vector2.UnitX;
        }
        if (keyboardState.IsKeyDown(Keys.D))
        {
            movementDirection += Vector2.UnitX;
        }
        if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift)) {
            movementDirection *= 4;
        }
        return movementDirection;
    }
}