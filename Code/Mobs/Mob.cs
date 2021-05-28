using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

namespace Blocktest
{
    public partial class Mob
    {
        protected Vector2 posMod;
        protected Vector2 velocity;

        protected Rectangle mainHitbox;
        protected Rectangle groundDetector;

        protected Rectangle spriteBox;
        protected Texture2D sprite;
        protected Color color = Color.White;

        protected bool isSolid = true;

        protected bool onGround;

        /// <summary>
        /// Size of the mob's hitbox
        /// </summary>
        protected virtual Point HitSize
        {
            get { return new Point(6, 6); }
        }

        /// <summary>
        /// Size of the mob's sprite
        /// </summary>
        protected virtual Point SpriteSize
        {
            get { return new Point(8, 8); }
        }

        /// <summary>
        /// Sprite offset from the hitbox's location
        /// </summary>
        protected virtual Point SpriteLoc
        {
            get { return new Point(-1, -1); }
        }

        /// <summary>
        /// Number of pixels to adjust the future position of the mob, counters rounding error
        /// </summary>
        protected virtual Point HitAdjust
        {
            get { return new Point(-1, -1); }
        }

        /// <summary>
        /// Filepath for the mob's sprite
        /// </summary>
        protected virtual String SpritePath
        {
            get { return "Blocks\\error"; }
        }

        /// <summary>
        /// The force applied to a mob when jumping
        /// </summary>
        protected virtual float JumpForce
        {
            get { return -10f; }
        }

        /// <summary>
        /// The acceleration from walking
        /// </summary>
        protected virtual float WalkAccel
        {
            get { return 20f; }
        }

        /// <summary>
        /// Creates a new <see cref="Mob"/> at the input <see cref="Point"/>
        /// </summary>
        /// <param name="newPosition">The location where this mob will be spawned in.</param>
        /// <param name="vel">The initial velocity of the mob upon spawning in</param>
        /// <param name="content">The <see cref="ContentManager"/> of the game</param>
        public Mob(Point newPosition, Vector2 vel, ContentManager content)
        {
            mainHitbox = new(newPosition, HitSize);

            newPosition.Y += HitSize.Y;
            groundDetector = new(mainHitbox.X, mainHitbox.Bottom - 1, mainHitbox.Width, 1);

            spriteBox = new((newPosition + SpriteLoc), SpriteSize);

            velocity = vel;

            LoadSprite(content);
        }

        /// <summary>
        /// Inititializes a mob's sprite
        /// </summary>
        /// <remarks>
        /// DO NOT FORGET TO CALL THE BASE METHOD IF YOU OVERRIDE THIS.
        /// </remarks>
        public virtual void LoadSprite(ContentManager content)
        {
            try {
                sprite = content.Load<Texture2D>(SpritePath);
            }
            catch (ContentLoadException) {
                sprite = content.Load<Texture2D>("Blocks\\error");
                Console.WriteLine("Block " + this + " does not have an icon at " + SpritePath + "!");
            }
        }

        public void Update(GameTime gameTime)
        {
            HandleMovement(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBox.Location = mainHitbox.Location + SpriteLoc;

            spriteBatch.Draw(sprite, spriteBox, color);
        }
    }
}
