using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Blocktest
{
    public class Mob
    {
        private Vector2 velocity;
        private Rectangle mainHitbox;
        private Texture2D blockSprite;

        /// <summary>
        /// Creates a new <see cref="Mob"/> at the input <see cref="Point"/>
        /// </summary>
        /// <param name="newPosition">The location where this mob will be spawned in.</param>
        public Mob(Point newPosition)
        {
            mainHitbox = new(newPosition, Point.Zero);
            velocity = Vector2.Zero;
        }

        /// <summary>
        /// Creates a new <see cref="Mob"/> at the input <see cref="Point"/> with the input <paramref name="size"/>
        /// </summary>
        /// <param name="newPosition">The location where this <see cref="Mob"/> will be spawned in.</param>
        /// <param name="size">The size of this <see cref="Mob"/></param>
        public Mob(Point newPosition, Point size)
        {
            mainHitbox = new(newPosition, size);
            velocity = Vector2.Zero;
        }

        public void Update(GameTime gameTime)        // TODO - Replace testcode
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) {
                velocity.Y = 5 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else {
                velocity.Y += -1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left) ^ Keyboard.GetState().IsKeyDown(Keys.Right)) {     // This is just testing code pls don't kill me
                if (Keyboard.GetState().IsKeyDown(Keys.Left)) {
                    velocity.X -= 2 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else {
                    velocity.X += 2 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            Point futurePosition = mainHitbox.Location;
            futurePosition.X += (int)Math.Round(velocity.X);
            futurePosition.Y += (int)Math.Round(velocity.Y);
        }
    }
}