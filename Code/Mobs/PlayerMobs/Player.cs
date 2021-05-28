using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

namespace Blocktest
{
    public partial class PlayerMob : Mob
    {

        protected override Point HitSize
        {
            get { return new Point(9, 27); }
        }

        protected override Point SpriteSize
        {
            get { return new Point(16, 32); }
        }

        protected override Point SpriteLoc
        {
            get { return new Point(-3, -4); }
        }
        protected override Point HitAdjust
        {
            get { return new Point(-1, -5); }
        }

        protected override String SpritePath
        {
            get { return "Mob\\player"; }
        }

        protected override float JumpForce
        {
            get { return -120; }
        }

        protected override float WalkAccel
        {
            get { return 200f; }
        }

        public PlayerMob(Point newPosition, Vector2 vel, ContentManager content, ref EventHandler<InputEventArgs> iHandler) : base(newPosition, vel, content)
        {
            iHandler += InputHandler;
        }

        /// <summary>
        /// Handles input from the player
        /// </summary>
        protected virtual void InputHandler(object sender, InputEventArgs e)
        {
            Vector2 accel = new();

            if (onGround && e.userInput.IsKeyDown(Keys.Space)) {
                Jump(JumpForce);
                onGround = false;
            }

            if (e.userInput.IsKeyDown(Keys.Left) ^ e.userInput.IsKeyDown(Keys.Right)) {     // If both, no acceleration
                if (e.userInput.IsKeyDown(Keys.Left)) {
                    accel.X = -1 * WalkAccel;
                } else {
                    accel.X = WalkAccel;
                }
            }
            Accelerate(accel, e.timePassed);
        }

    }
}
