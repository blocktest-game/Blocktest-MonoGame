using Microsoft.Xna.Framework.Input;
namespace Blocktest.Scenes; 

public class GameScene : Scene {
    private readonly BlocktestGame _game;
    private readonly SpriteBatch _spriteBatch;
    
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
	            if (blockSelected >= BlockManager.AllBlocks.Length)
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
	                BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[blockSelected], true,
	                    new Vector2Int(MathHelper.Clamp(currentState.X / Globals.gridSize.X, 0, Globals.maxX), 
		                    MathHelper.Clamp(currentState.Y / Globals.gridSize.Y, 0, Globals.maxY)));
	            } else if (currentState.RightButton == ButtonState.Pressed) {
		            BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[blockSelected], false,
			            new Vector2Int(MathHelper.Clamp(currentState.X / Globals.gridSize.X, 0, Globals.maxX), 
				            MathHelper.Clamp(currentState.Y / Globals.gridSize.Y, 0, Globals.maxY)));
	            }
            }
            else 
            {
	            if(currentState.LeftButton == ButtonState.Pressed)
	            {
		            BuildSystem.BreakBlockCell( true,
			            new Vector2Int(MathHelper.Clamp(currentState.X / Globals.gridSize.X, 0, Globals.maxX), 
				            MathHelper.Clamp(currentState.Y / Globals.gridSize.Y, 0, Globals.maxY)));
	            } else if (currentState.RightButton == ButtonState.Pressed) {
		            BuildSystem.BreakBlockCell( false,
			            new Vector2Int(MathHelper.Clamp(currentState.X / Globals.gridSize.X, 0, Globals.maxX), 
				            MathHelper.Clamp(currentState.Y / Globals.gridSize.Y, 0, Globals.maxY)));
	            }
            }

    }

    public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice) {
        graphicsDevice.Clear(Color.CornflowerBlue);
        
        _spriteBatch.Begin();
        Globals.BackgroundTilemap.Draw(_spriteBatch);
        Globals.ForegroundTilemap.Draw(_spriteBatch);
        // placement preview
        if (buildMode)
            _spriteBatch.Draw(BlockManager.AllBlocks[blockSelected].blockSprite.Texture,
            new Vector2Int(Mouse.GetState().X - (Mouse.GetState().X % 8),
                (Mouse.GetState().Y - Mouse.GetState().Y % 8)),
            new Rectangle(1, 1, 10, 10), Color.DimGray);

        _spriteBatch.End();
    }

    public GameScene(BlocktestGame game) {
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _game = game;
        
        Globals.BackgroundTilemap = new Tilemap(Globals.maxX, Globals.maxY);
        Globals.ForegroundTilemap = new Tilemap(Globals.maxX, Globals.maxY);

        for (int i = 0; i < Globals.maxX; i++) {
            BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[2], true, new Vector2Int(i, 5));
            BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[0], true, new Vector2Int(i, 4));
            BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[0], true, new Vector2Int(i, 3));
            BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[0], true, new Vector2Int(i, 2));
            BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[0], true, new Vector2Int(i, 1));
            BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[1], true, new Vector2Int(i, 0));
        }
            
        BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[0], true, new Vector2Int(20, 20));
    }
}