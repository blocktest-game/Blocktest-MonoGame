using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
namespace Shared.Code;

/// <summary>
///     Represents a vector with two integer values rather than two floating-point values.
/// </summary>
/// <seealso cref="Vector2" />
public struct Vector2Int : INetSerializable {
    /// <summary>
    ///     The X coordinate of this <see cref="Vector2Int" />.
    /// </summary>
    public int X { get; private set; }

    /// <summary>
    ///     The Y coordinate of this <see cref="Vector2Int" />.
    /// </summary>
    public int Y { get; private set; }

    /// <summary>
    ///     Create a <see cref="Vector2Int" />.
    /// </summary>
    public Vector2Int(int x, int y) {
        X = x;
        Y = y;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Vector2Int @int && Equals(@int);

    /// <summary>
    ///     Indicates whether this Vector2Int and another's X and Y values are the same.
    /// </summary>
    /// <param name="other">The Vector2Int to compare this to.</param>
    /// <returns>true if <paramref name="other" />'s values are the same as this instance's, otherwise, false.</returns>
    public bool Equals(Vector2Int other) => X == other.X && Y == other.Y;

    /// <summary>
    ///     Checks if both the X or Y value of two Vector2Int-s are the same.
    /// </summary>
    public static bool operator ==(Vector2Int value1, Vector2Int value2) =>
        value1.X == value2.X && value1.Y == value2.Y;

    /// <summary>
    ///     Checks if either the X or Y value of two Vector2Int-s are different.
    /// </summary>
    public static bool operator !=(Vector2Int value1, Vector2Int value2) =>
        value1.X != value2.X || value1.Y != value2.Y;

    /// <summary>
    ///     Adds the values of two Vector2Ints together and returns a Vector2Int with the value.
    /// </summary>
    public static Vector2Int operator +(Vector2Int value1, Vector2Int value2) =>
        new(value1.X + value2.X, value1.Y + value2.Y);

    /// <summary>
    ///     Subtracts the second value from the first, and returns a Vector2Int with the result.
    /// </summary>
    public static Vector2Int operator -(Vector2Int value1, Vector2Int value2) =>
        new(value1.X - value2.X, value1.Y - value2.Y);

    public static Vector2Int operator -(Vector2Int value1) =>
        new(-value1.X, -value1.Y);

    public static Vector2Int operator *(Vector2Int value1, Vector2Int value2) =>
        new(value1.X * value2.X, value1.Y * value2.Y);

    public static Vector2Int operator /(Vector2Int value1, Vector2Int value2) =>
        new(value1.X / value2.X, value1.Y / value2.Y);

    public static Vector2Int operator *(Vector2Int value, int multiplier) =>
        new(value.X * multiplier, value.Y * multiplier);

    /// <inheritdoc />
    public override int GetHashCode() => X.GetHashCode() + Y.GetHashCode();

    /// <inheritdoc />
    public override string ToString() => $"{{X:{X} Y:{Y}}}";

    // These allow you to use Vector2Ints as Vector2s, and explicitly (by putting (Vector2Int) in front of the Vector2) use Vector2 as Vector2Int
    public static implicit operator Vector2(Vector2Int vector2Int) => new(vector2Int.X, vector2Int.Y);

    public static explicit operator Vector2Int(Vector2 vector2) =>
        new((int)Math.Round(vector2.X), (int)Math.Round(vector2.Y));

    public static implicit operator Point(Vector2Int vector2Int) => new(vector2Int.X, vector2Int.Y);

    public static implicit operator Vector2Int(Point point) => new(point.X, point.Y);

    // Preset values
    /// <summary>Returns a <see cref="Vector2Int" /> with values (0, 0).</summary>
    public static Vector2Int Zero { get; } = new(0, 0);

    /// <summary>Returns a <see cref="Vector2Int" /> with values (1, 1).</summary>
    public static Vector2Int One { get; } = new(1, 1);

    /// <summary>Returns a <see cref="Vector2Int" /> with values (0, 1).</summary>
    public static Vector2Int Up { get; } = new(0, 1);

    /// <summary>Returns a <see cref="Vector2Int" /> with values (0, -1).</summary>
    public static Vector2Int Down { get; } = new(0, -1);

    /// <summary>Returns a <see cref="Vector2Int" /> with values (-1, 0).</summary>
    public static Vector2Int Left { get; } = new(-1, 0);

    /// <summary>Returns a <see cref="Vector2Int" /> with values (1, 0).</summary>
    public static Vector2Int Right { get; } = new(1, 0);

    public void Serialize(NetDataWriter writer) {
        writer.Put(X);
        writer.Put(Y);
    }

    public void Deserialize(NetDataReader reader) {
        X = reader.GetInt();
        Y = reader.GetInt();
    }
}