using Microsoft.Xna.Framework;
using System;

namespace Blocktest
{
    /// <summary>
    /// Represents a vector with two integer values rather than two floating-point values.
    /// </summary>
    /// <seealso cref="Vector2"/>
    public struct Vector2Int
    {
        public int X;
        public int Y;

        public Vector2Int(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public Vector2Int(float X, float Y)
        {
            this.X = (int)Math.Round(X);
            this.Y = (int)Math.Round(Y);
        }

        public override bool Equals(object obj)
        {
            return (obj is Vector2Int @int) && (Equals(@int));
        }

        public bool Equals(Vector2Int other)
        {
            return (X == other.X) && (Y == other.Y);
        }

        /// <summary>
        /// Checks if both the X or Y value of two Vector2Int-s are the same.
        /// </summary>
        public static bool operator ==(Vector2Int value1, Vector2Int value2)
        {
            return (value1.X == value2.X) && (value1.Y == value2.Y);
        }

        /// <summary>
        /// Checks if either the X or Y value of two Vector2Int-s are different.
        /// </summary>
        public static bool operator !=(Vector2Int value1, Vector2Int value2)
        {
            if (value1.X == value2.X) {
                return value1.Y != value2.Y;
            }

            return true;
        }

        /// <summary>
        /// Adds the values of two Vector2Ints together and returns a Vector2Int with the value.
        /// </summary>
        public static Vector2Int operator +(Vector2Int value1, Vector2Int value2)
        {
            return new Vector2Int(value1.X + value2.X, value1.Y + value2.Y);
        }

        public override int GetHashCode()
        {
            return (X.GetHashCode() + Y.GetHashCode());
        }

        public override string ToString()
        {
            return string.Format("{{X:{0} Y:{1}}}", X, Y);
        }

        // These allow you to use Vector2Ints as Vector2s, and explicitly (by putting (Vector2Int) in front of the Vector2) use Vector2 as Vector2Int
        public static implicit operator Vector2(Vector2Int vector2Int) => new(vector2Int.X, vector2Int.Y);
        public static explicit operator Vector2Int(Vector2 vector2) => new((int)Math.Round(vector2.X), (int)Math.Round(vector2.Y));

        // Preset values
        public static readonly Vector2Int Zero = new(0, 0);
        public static readonly Vector2Int One = new(1, 1);
        public static readonly Vector2Int Up = new(0, 1);
        public static readonly Vector2Int Down = new(0, -1);
        public static readonly Vector2Int Left = new(-1, 0);
        public static readonly Vector2Int Right = new(1, 0);
    }
}
