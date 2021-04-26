using Microsoft.Xna.Framework;
using System;

namespace Blocktest
{
    public struct HitTests
    {
        /// <summary>
        /// Hit test for a <see cref="Point"/> and a <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="point">The <see cref="Point"/> being tested.</param>
        /// <param name="rect">The <see cref="Rectangle"/> being tested.</param>
        /// <returns>Returns true if the input <paramref name="point"/> is inside the input <paramref name="rect"/>.</returns>
        public static bool PointToRectangle(Point point, Rectangle rect)
        {
            return (point.X >= rect.X                       // point is to the right of rect's left side
                    && point.X <= rect.X + rect.Width           // point is to the left of rect's right side
                    && point.Y >= rect.Y                        // point is beneath the top of rect
                    && point.Y <= rect.Y + rect.Height);        // point is above the bottom of rect
        }

        /// <summary>
        /// Hit test for two <see cref="Rectangle"/>s.
        /// </summary>
        /// <param name="tester">The first <see cref="Rectangle"/> being tested.</param>
        /// <param name="target">The second <see cref="Rectangle"/> being tested.</param>
        /// <returns>Returns true if the two input <see cref="Rectangle"/>s intersect.</returns>
        public static bool RectangleToRectangle(Rectangle tester, Rectangle target)
        {
            return (target.X <= tester.X + tester.Width         // target's left side must be to the left of rect's right side
                && tester.X <= target.X + target.Width        // target's right side must be to the right of rect's left side
                && target.Y <= tester.Y + tester.Height         // target's top must be above the bottom of rect
                && tester.Y <= target.Y + target.Height);     // target's bottom must be beneath rect's top.
        }

        /// <summary>
        /// Hit test for a ray and a <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="start">The starting <see cref="Point"/> of the ray.</param>
        /// <param name="end">The ending <see cref="Point"/> of the ray.</param>
        /// <param name="target">The <see cref="Rectangle"/> being tested.</param>
        /// <param name="contactPoint">Outputs the <see cref="Point"/> at which the ray intersects the <paramref name="target"/>.</param>
        /// <param name="contactNormal">Outputs the side at at which the ray intersects the <paramref name="target"/>.</param>
        /// <param name="nearContactTime">Outputs the time when the ray contacts the <paramref name="target"/>.</param>
        /// <returns>Returns true if the ray intersects the <paramref name="target"/>.</returns>
        public static bool RayToRectangle(Point start, Point end, Rectangle target, out Point contactPoint, out Point contactNormal, out float nearContactTime)
        {
            Point rayDelta = end - start;

            contactPoint.X = 0;
            contactPoint.Y = 0;
            contactNormal.X = 0;
            contactNormal.Y = 0;
            nearContactTime = 0;

            if (rayDelta.X == 0 || rayDelta.Y == 0) {
                return false;
            }

            Point nearContact;
            Point farContact;

            Vector2 nearIntersectTime;
            Vector2 farIntersectTime;

            Vector2 slope = new(1.0f / rayDelta.X, 1.0f / rayDelta.Y);

            Point rayLength = end - start;

            nearContact.X = target.X - start.X;
            nearContact.Y = target.Y - start.Y;
            farContact.X = target.X + target.Width - start.X;
            farContact.Y = target.Y + target.Height - start.Y;

            nearIntersectTime.X = (float)nearContact.X * slope.X;
            nearIntersectTime.Y = (float)nearContact.Y * slope.Y;
            farIntersectTime.X = (float)farContact.X * slope.X;
            farIntersectTime.Y = (float)farContact.Y * slope.Y;

            if (nearIntersectTime.X > farIntersectTime.X) {
                Utilities.Algorithms.swap(ref nearIntersectTime.X, ref farIntersectTime.X);
            }
            if (nearIntersectTime.Y > farIntersectTime.Y) {
                Utilities.Algorithms.swap(ref nearIntersectTime.Y, ref farIntersectTime.Y);
            }

            if (nearIntersectTime.X > farIntersectTime.Y || nearIntersectTime.Y > farIntersectTime.X) {
                return false;
            }

            nearContactTime = Math.Max(nearIntersectTime.Y, nearIntersectTime.X);
            float farContactTime = Math.Min(farIntersectTime.Y, farIntersectTime.X);

            if (farContactTime < 0 || nearContactTime > 1) {       // Check if the rectangle is behind the ray
                return false;
            }

            contactPoint.X = (int)Math.Round((float)start.X + nearContactTime * (float)rayLength.X);
            contactPoint.Y = (int)Math.Round((float)start.Y + nearContactTime * (float)rayLength.Y);

            if (nearIntersectTime.X > nearIntersectTime.Y) {
                if (slope.X > 0) {
                    contactNormal.X = 1;
                }
                else {
                    contactNormal.X = -1;
                }
            }
            else if (nearIntersectTime.X < nearIntersectTime.Y) {
                if (slope.Y > 0) {
                    contactNormal.Y = 1;
                }
                else {
                    contactNormal.Y = -1;
                }
            }

            return true;
        }

        /// <summary>
        /// Hit test for a moving <see cref="Rectangle" and a stationary <see cref="Rectangle"/>/>
        /// </summary>
        /// <param name="tester">The moving <see cref="Rectangle"/>.</param>
        /// <param name="endPoint"><paramref name="tester"/>'s location <see cref="Point"/> after movement.</param>
        /// <param name="target">The stationary <see cref="Rectangle"/>.</param>
        /// <param name="contactPoint">Outputs the <see cref="Point"/> at which the ray intersects the <paramref name="target"/>.</param>
        /// <param name="contactNormal">Outputs the side at at which the ray intersects the <paramref name="target"/>.</param>
        /// <param name="nearContactTime">Outputs the time when the ray contacts the <paramref name="target"/>.</param>
        /// <returns>Returns true if <paramref name="tester"/> will intersect <paramref name="target"/> at some point during its movement.</returns>
        public static bool MoverRectangleToRectangle (Rectangle tester, Point endPoint, Rectangle target, out Point contactPoint, out Point contactNormal, out float nearContactTime)
        {
            contactPoint.X = 0;
            contactPoint.Y = 0;
            contactNormal.X = 0;
            contactNormal.Y = 0;
            nearContactTime = 0;

            if (tester.Location == endPoint) {      // No movement
                return false;
            }

            target.Inflate(tester.Width, tester.Height);

            endPoint.X += tester.Width;
            endPoint.Y += tester.Height;

            if (RayToRectangle(tester.Center, endPoint, target, out contactPoint, out contactNormal, out nearContactTime)) {
                return true;
            }
            return false;
        }

    }
}