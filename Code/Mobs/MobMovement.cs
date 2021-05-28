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
        /// <summary>
        /// Changes the velocity of the mob with a given acceleration and time
        /// </summary>
        /// <param name="accel">The acceleration on the mob</param>
        /// <param name="time">The time the acceleration takes place over</param>
        protected void Accelerate(Vector2 accel, double time)
        {
            velocity.X += (float)((double)accel.X * time);
            velocity.Y += (float)((double)accel.Y * time);
        }

        /// <summary>
        /// Changes the velocity of the mob without regards to time
        /// </summary>
        /// <param name="accel">The acceleration on the mob</param>
        protected void Jump(float accel)
        {
            velocity.Y += accel;
        }

        /// <summary>
        /// Handles the effect of gravity on the mob
        /// </summary>
        /// <param name="time">Amount of time that has passed since last frame(last time gravity acted)</param>
        protected virtual void HandleGrav(double time)
        {
                Vector2 accel = new(0f, 100f);
                Accelerate(accel, time);
        }

        /// <summary>
        /// Handles the movement of the mob
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void HandleMovement(GameTime gameTime)
        {
            HandleGrav(gameTime.ElapsedGameTime.TotalSeconds);

            Point futurePosition = mainHitbox.Location;

            posMod.X += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            posMod.Y += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Point posModHelper = new((int)posMod.X, (int)posMod.Y);
            futurePosition += posModHelper;

            posMod.X -= posModHelper.X;
            posMod.Y -= posModHelper.Y;

            if (isSolid) {
                HandleHitTest(ref futurePosition);
            }

            mainHitbox.Location = futurePosition;

            groundDetector.Location = futurePosition;
            groundDetector.Y += mainHitbox.Height;

            if (groundCheck()) {
                posMod.Y = 0;
                velocity.Y = 0;
            }
            else {
                onGround = false;
            }
        }

        /// <summary>
        /// Calls the hittest and adjusts the future position of the mob as required
        /// </summary>
        /// <param name="futurePosition">The initial future position of the mob</param>
        protected virtual void HandleHitTest(ref Point futurePosition)
        {
            Point contactPoint;
            Point contactNormal;
            float nearContactTime;

            List<(int rectNum, float contactTime, Rectangle target)> rectInfo = new();        // Stores information on rectangles and the time to contact

            int i = 0;      // Searched for about an hour, didn't find a better method

            foreach (Tile tile in Globals.ForegroundTilemap.allTiles) {
                if (HitTests.MoverRectangleToRectangle(mainHitbox, futurePosition, tile.rectangle, out contactPoint, out contactNormal, out nearContactTime)) {
                    rectInfo.Add((i, nearContactTime, new Rectangle(tile.rectangle.Location, tile.rectangle.Size)));

                    i++;
                }
            }

            rectInfo.Sort((y, x) => y.contactTime.CompareTo(x.contactTime));        // Sorts rectInfo by contactTime

            foreach ((int rectNum, float contactTime, Rectangle target) test in rectInfo) {

                if (HitTests.MoverRectangleToRectangle(mainHitbox, futurePosition, test.target, out contactPoint, out contactNormal, out nearContactTime)) {
                    
                    if (contactNormal.X != 0) {
                        velocity.X = 0;
                        futurePosition.X = (contactPoint.X - mainHitbox.Width / 2) - ((mainHitbox.Width / 4 + HitAdjust.X) * contactNormal.X);
                    }
                    if (contactNormal.Y != 0) {
                        velocity.Y = 0;
                        futurePosition.Y = (contactPoint.Y - mainHitbox.Height / 2) - ((mainHitbox.Height / 4 + HitAdjust.Y) * contactNormal.Y);

                        if (contactNormal.Y == 1) {
                            onGround = true;
                        }
                    }

                }
            }

        }

        /// <summary>
        /// Checks whether the mob is touching the ground
        /// </summary>
        /// <returns>true if the mob is touching the ground</returns>
        protected virtual bool groundCheck()
        {
            if (onGround && velocity.Y >= 0) {         // Make sure the mob has actually touched the ground at some point and is not flying away from the ground
                foreach (Tile tile in Globals.ForegroundTilemap.allTiles) {
                    if (HitTests.RectangleToRectangle(groundDetector, tile.rectangle)) {
                        return true;
                    }
                }
            }
            return false;
        }

        
    }
}
