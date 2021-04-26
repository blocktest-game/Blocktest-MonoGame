using Microsoft.Xna.Framework;
using System;

namespace Blocktest
{
    /// <summary>
    /// Holds a rectangle and allows checking for contacts with other hitboxes
    /// </summary>
    struct Hitbox
    {
        public Rectangle rect;

        /// <summary>
        /// Hit test for a single <see cref="Point"/>.
        /// </summary>
        /// <param name="point">The <see cref="Point"/> being tested.</param>
        /// <seealso cref="HitTests.PointToRectangle"/>
        public bool PointTest(Point point)
        {
            return HitTests.PointToRectangle(point, rect);
        }

        /// <summary>
        /// Hit test for a <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="target">The <see cref="Rectangle"/> being tested.</param>
        /// <seealso cref="HitTests.RectangleToRectangle"/>
        public bool RectTest(Rectangle target)
        {
            return HitTests.RectangleToRectangle(rect, target);
        }

        /// <summary>
        /// Hit test for a <see cref="Hitbox"/>.
        /// </summary>
        public bool HitboxTest(Hitbox target)
        {
            return RectTest(target.rect);       // lmao just use rectTest
        }
    }
}