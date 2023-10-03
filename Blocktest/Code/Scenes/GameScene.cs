using Blocktest.Networking;
using LiteNetLib;
using Microsoft.Xna.Framework.Input;
using Shared.Networking;
namespace Blocktest.Scenes; 

public class GameScene : Scene {
    private readonly BlocktestGame _game;
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

            //press E to toggle build/destroy
            if (Keyboard.GetState().IsKeyUp(Keys.E))
            {
	            latch = false;
            } 
            else if (latch == false && CheckWindowActive(currentState))
            {
	            buildMode = !buildMode;
	            latch = true;
            }

            //Q changes which block you have selected
            if (Keyboard.GetState().IsKeyUp(Keys.Q))
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
                        position = new Vector2Int(MathHelper.Clamp(currentState.X / GlobalsShared.gridSize.X, 0, GlobalsShared.maxX), MathHelper.Clamp(currentState.Y / GlobalsShared.gridSize.Y, 0, GlobalsShared.maxY)),
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
                        position = new Vector2Int(MathHelper.Clamp(currentState.X / GlobalsShared.gridSize.X, 0, GlobalsShared.maxX), MathHelper.Clamp(currentState.Y / GlobalsShared.gridSize.Y, 0, GlobalsShared.maxY)),
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
                        position = new Vector2Int(MathHelper.Clamp(currentState.X / GlobalsShared.gridSize.X, 0, GlobalsShared.maxX), MathHelper.Clamp(currentState.Y / GlobalsShared.gridSize.Y, 0, GlobalsShared.maxY)),
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
                        position = new Vector2Int(MathHelper.Clamp(currentState.X / GlobalsShared.gridSize.X, 0, GlobalsShared.maxX), MathHelper.Clamp(currentState.Y / GlobalsShared.gridSize.Y, 0, GlobalsShared.maxY)),
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
        
        _spriteBatch.Begin();

        Globals.BackgroundTilemapSprites.DrawAllTiles(_spriteBatch);
        Globals.ForegroundTilemapSprites.DrawAllTiles(_spriteBatch);

        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _frameCounter.Update(deltaTime);

        String fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);

        //Console.WriteLine(fps);

        if (buildMode)
            _spriteBatch.Draw(BlockSpritesManager.AllBlocksSprites[blockSelected].blockSprite.Texture,
            new Vector2Int(Mouse.GetState().X - (Mouse.GetState().X % 8),
                (Mouse.GetState().Y - Mouse.GetState().Y % 8)),
            new Rectangle(1, 1, 10, 10), Color.DimGray);

        _spriteBatch.End();
    }

    public GameScene(BlocktestGame game, bool doConnect, string ip) {
        connect = doConnect;
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _game = game;

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
}