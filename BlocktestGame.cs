using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Utilities;
using System;

namespace Blocktest
{
    /// <inheritdoc />
    public class BlocktestGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public event EventHandler<InputEventArgs> InputHandler;
        private InputEventArgs inputArgs;

        private PlayerMob testPlayer;

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

            Globals.ForegroundTilemap = new Tilemap(Globals.maxX, Globals.maxY);
            Globals.BackgroundTilemap = new Tilemap(Globals.maxX, Globals.maxY);

            inputArgs = new();

            base.Initialize();

            for (int i = 0; i < Globals.maxX; i++) {
                /*BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[2], true, new Vector2Int(i, 0));
                BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[0], true, new Vector2Int(i, 1));
                BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[0], true, new Vector2Int(i, 2));
                BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[0], true, new Vector2Int(i, 3));
                BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[0], true, new Vector2Int(i, 4));*/
                BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[1], true, new Vector2Int(i, 5));
            }

            BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[1], true, new Vector2Int(10, 10));
            BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[1], true, new Vector2Int(10, 11));
            BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[1], true, new Vector2Int(10, 12));

            for (int i = 20; i < Globals.maxX; i++) {
                BuildSystem.PlaceBlockCell(BlockManager.AllBlocks[1], true, new Vector2Int(i, 15));
            }

            Point testPt = new(20);

            testPlayer = new(testPt, Vector2.Zero, Content, ref InputHandler);
        }

        /// <inheritdoc />
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            BlockManager.LoadBlockSprites(Content);
        }

        /// <inheritdoc />
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                
            }

            KeyboardState inputState = Keyboard.GetState();

            if (inputState.GetPressedKeyCount() > 0) {
                inputArgs.userInput = inputState;
                inputArgs.timePassed = gameTime.ElapsedGameTime.TotalSeconds;

                if (InputHandler != null) {
                    InputHandler(this, inputArgs);
                }
            }

            testPlayer.Update(gameTime);

            base.Update(gameTime);
        }

        /// <inheritdoc />
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            Globals.ForegroundTilemap.Draw(_spriteBatch);
            Globals.BackgroundTilemap.Draw(_spriteBatch);

            testPlayer.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
