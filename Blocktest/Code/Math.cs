namespace Blocktest
{
    /// <summary>
    /// Represents a vector with two integer values rather than two floating-point values.
    /// </summary>
    /// <seealso cref="Vector2"/>
    public struct Vector2Int
    {
        /// <summary>
        /// The X coordinate of this <see cref="Vector2Int"/>.
        /// </summary>
        public int X;
        /// <summary>
        /// The Y coordinate of this <see cref="Vector2Int"/>.
        /// </summary>
        public int Y;

        /// <summary>
        /// Create a <see cref="Vector2Int"/>.
        /// </summary>
        public Vector2Int(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        /// <summary>
        /// Create a <see cref="Vector2Int"/> with two floats. Floats will be rounded with <see cref="Math.Round(float)"/>.
        /// </summary>
        public Vector2Int(float X, float Y)
        {
            this.X = (int)Math.Round(X);
            this.Y = (int)Math.Round(Y);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => (obj is Vector2Int @int) && (Equals(@int));

        /// <summary>
        /// Indicates whether this Vector2Int and another's X and Y values are the same.
        /// </summary>
        /// <param name="other">The Vector2Int to compare this to.</param>
        /// <returns>true if <paramref name="other"/>'s values are the same as this instance's, otherwise, false.</returns>
        public bool Equals(Vector2Int other) => (X == other.X) && (Y == other.Y);

        /// <summary>
        /// Checks if both the X or Y value of two Vector2Int-s are the same.
        /// </summary>
        public static bool operator ==(Vector2Int value1, Vector2Int value2) => (value1.X == value2.X) && (value1.Y == value2.Y);

        /// <summary>
        /// Checks if either the X or Y value of two Vector2Int-s are different.
        /// </summary>
        public static bool operator !=(Vector2Int value1, Vector2Int value2) => (value1.X != value2.X) || (value1.Y != value2.Y);

        /// <summary>
        /// Adds the values of two Vector2Ints together and returns a Vector2Int with the value.
        /// </summary>
        public static Vector2Int operator +(Vector2Int value1, Vector2Int value2) => new(value1.X + value2.X, value1.Y + value2.Y);

        /// <summary>
        /// Subtracts the second value from the first, and returns a Vector2Int with the result.
        /// </summary>
        public static Vector2Int operator -(Vector2Int value1, Vector2Int value2) => new(value1.X - value2.X, value1.Y - value2.Y);

        /// <inheritdoc/>
        public override int GetHashCode() => (X.GetHashCode() + Y.GetHashCode());

        /// <inheritdoc/>
        public override string ToString() => string.Format("{{X:{0} Y:{1}}}", X, Y);

        // These allow you to use Vector2Ints as Vector2s, and explicitly (by putting (Vector2Int) in front of the Vector2) use Vector2 as Vector2Int
        public static implicit operator Vector2(Vector2Int vector2Int) => new(vector2Int.X, vector2Int.Y);
        public static explicit operator Vector2Int(Vector2 vector2) => new((int)Math.Round(vector2.X), (int)Math.Round(vector2.Y));

        // Preset values
        /// <summary>Returns a <see cref="Vector2Int"/> with values (0, 0).</summary>
        public static readonly Vector2Int Zero = new(0, 0);
        /// <summary>Returns a <see cref="Vector2Int"/> with values (1, 1).</summary>
        public static readonly Vector2Int One = new(1, 1);
        /// <summary>Returns a <see cref="Vector2Int"/> with values (0, 1).</summary>
        public static readonly Vector2Int Up = new(0, 1);
        /// <summary>Returns a <see cref="Vector2Int"/> with values (0, -1).</summary>
        public static readonly Vector2Int Down = new(0, -1);
        /// <summary>Returns a <see cref="Vector2Int"/> with values (-1, 0).</summary>
        public static readonly Vector2Int Left = new(-1, 0);
        /// <summary>Returns a <see cref="Vector2Int"/> with values (1, 0).</summary>
        public static readonly Vector2Int Right = new(1, 0);
    }
}
