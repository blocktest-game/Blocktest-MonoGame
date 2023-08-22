using Blocktest.Rendering;
using Microsoft.Xna.Framework.Input;

namespace Blocktest
{
    /// <inheritdoc />
    public class BlocktestGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        /// <inheritdoc />
        public BlocktestGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            Globals.Game = this;

            BlockManager.Initialize();

            Globals.BackgroundTilemap = new Tilemap(Globals.maxX, Globals.maxY);
            Globals.ForegroundTilemap = new Tilemap(Globals.maxX, Globals.maxY);

            base.Initialize();

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

        /// <inheritdoc />
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Drawable.ContentManager = Content;
            BlockManager.LoadBlockSprites(Content);
        }

        /// <inheritdoc />
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Exit();
            }

            MouseState currentState = Mouse.GetState();
            if (currentState.LeftButton == ButtonState.Pressed)
            {
                BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[0], true,
                    new Vector2(MathHelper.Clamp(currentState.X / Globals.gridSize.X, 0, Globals.maxX), 
	                    MathHelper.Clamp(currentState.Y / Globals.gridSize.Y, 0, Globals.maxY)));
            } else if (currentState.RightButton == ButtonState.Pressed) {
	            BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[0], false,
		            new Vector2(MathHelper.Clamp(currentState.X / Globals.gridSize.X, 0, Globals.maxX), 
			            MathHelper.Clamp(currentState.Y / Globals.gridSize.Y, 0, Globals.maxY)));
            }

            base.Update(gameTime);
        }

        /// <inheritdoc />
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            Globals.BackgroundTilemap.Draw(_spriteBatch);
            Globals.ForegroundTilemap.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
