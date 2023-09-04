using Microsoft.Xna.Framework.Input;
namespace Blocktest.Scenes; 

public class GameScene : Scene {
    private readonly BlocktestGame _game;
    private readonly SpriteBatch _spriteBatch;
    private FrameCounter _frameCounter = new FrameCounter();
    private readonly SpriteFont _spriteFont;
    
    bool latch = false; //latch for button pressing
    private bool latchBlockSelect = false; //same but for block selection
    bool buildMode = true; //true for build, false for destroy
    private int blockSelected = 0; //ID of the block to place
    
    public void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                _game.Exit();
            }

            //press E to toggle build/destroy
            if (Keyboard.GetState().IsKeyUp(Keys.E))
            {
	            latch = false;
            } 
            else if (latch == false)
            {
	            buildMode = !buildMode;
	            latch = true;
            }

            //for block placement
            MouseState currentState = Mouse.GetState();

            //Q changes which block you have selected
            if (Keyboard.GetState().IsKeyUp(Keys.Q))
            {
	            latchBlockSelect = false;
            }
            else if (latchBlockSelect == false)
            {
	            blockSelected++;
	            if (blockSelected >= BlockManagerShared.AllBlocks.Length)
	            {
		            blockSelected = 0;
	            }

	            latchBlockSelect = true;
            }

            //build and destroy mode
            if (buildMode)
            {
	            if(currentState.LeftButton == ButtonState.Pressed)
	            {
	                BuildSystem.PlaceBlockCell(BlockManagerShared.AllBlocks[blockSelected], true,
	                    new Vector2Int(MathHelper.Clamp(currentState.X / GlobalsShared.gridSize.X, 0, GlobalsShared.maxX), 
		                    MathHelper.Clamp(currentState.Y / GlobalsShared.gridSize.Y, 0, GlobalsShared.maxY)));
	            } else if (currentState.RightButton == ButtonState.Pressed) {
		            BuildSystem.PlaceBlockCell(BlockManagerShared.AllBlocks[blockSelected], false,
			            new Vector2Int(MathHelper.Clamp(currentState.X / GlobalsShared.gridSize.X, 0, GlobalsShared.maxX), 
				            MathHelper.Clamp(currentState.Y / GlobalsShared.gridSize.Y, 0, GlobalsShared.maxY)));
	            }
            }
            else 
            {
	            if(currentState.LeftButton == ButtonState.Pressed)
	            {
		            BuildSystem.BreakBlockCell( true,
			            new Vector2Int(MathHelper.Clamp(currentState.X / GlobalsShared.gridSize.X, 0, GlobalsShared.maxX), 
				            MathHelper.Clamp(currentState.Y / GlobalsShared.gridSize.Y, 0, GlobalsShared.maxY)));
	            } else if (currentState.RightButton == ButtonState.Pressed) {
		            BuildSystem.BreakBlockCell( false,
			            new Vector2Int(MathHelper.Clamp(currentState.X / GlobalsShared.gridSize.X, 0, GlobalsShared.maxX), 
				            MathHelper.Clamp(currentState.Y / GlobalsShared.gridSize.Y, 0, GlobalsShared.maxY)));
	            }
            }

    }

    public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice) {
        graphicsDevice.Clear(Color.CornflowerBlue);
        
        _spriteBatch.Begin();

        Globals.BackgroundTilemapSprites.Draw(_spriteBatch);
        Globals.ForegroundTilemapSprites.Draw(_spriteBatch);

        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _frameCounter.Update(deltaTime);

        String fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);

        Console.WriteLine(fps);

        if (buildMode)
            _spriteBatch.Draw(BlockSpritesManager.AllBlocksSprites[blockSelected].blockSprite.Texture,
            new Vector2Int(Mouse.GetState().X - (Mouse.GetState().X % 8),
                (Mouse.GetState().Y - Mouse.GetState().Y % 8)),
            new Rectangle(1, 1, 10, 10), Color.DimGray);

        _spriteBatch.End();
    }

    public GameScene(BlocktestGame game) {
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _game = game;
        
        GlobalsShared.BackgroundTilemap = new TilemapShared(GlobalsShared.maxX, GlobalsShared.maxY);
        GlobalsShared.ForegroundTilemap = new TilemapShared(GlobalsShared.maxX, GlobalsShared.maxY);
        Globals.BackgroundTilemapSprites = new(GlobalsShared.BackgroundTilemap);
        Globals.ForegroundTilemapSprites = new(GlobalsShared.ForegroundTilemap);

        for (int i = 0; i < GlobalsShared.maxX; i++) {
            BuildSystem.PlaceBlockCell(BlockManagerShared.AllBlocks[2], true, new Vector2Int(i, 5));
            BuildSystem.PlaceBlockCell(BlockManagerShared.AllBlocks[0], true, new Vector2Int(i, 4));
            BuildSystem.PlaceBlockCell(BlockManagerShared.AllBlocks[0], true, new Vector2Int(i, 3));
            BuildSystem.PlaceBlockCell(BlockManagerShared.AllBlocks[0], true, new Vector2Int(i, 2));
            BuildSystem.PlaceBlockCell(BlockManagerShared.AllBlocks[0], true, new Vector2Int(i, 1));
            BuildSystem.PlaceBlockCell(BlockManagerShared.AllBlocks[1], true, new Vector2Int(i, 0));
        }
            
        BuildSystem.PlaceBlockCell(BlockManagerShared.AllBlocks[0], true, new Vector2Int(20, 20));
    }
}