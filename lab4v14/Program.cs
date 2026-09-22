using System;

public class Vector2D
{
    private double _x;
    private double _y;

    public double X
    {
        get => _x;
        set => _x = value;
    }

    public double Y
    {
        get => _y;
        set => _y = value;
    }

    public Vector2D(double x, double y)
    {
        X = x;
        Y = y;
    }

    public static Vector2D UnitX => new Vector2D(1, 0);

    public static Vector2D operator +(Vector2D a, Vector2D b)
    {
        return new Vector2D(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2D operator -(Vector2D a, Vector2D b)
    {
        return new Vector2D(a.X - b.X, a.Y - b.Y);
    }

    public static bool operator ==(Vector2D? a, Vector2D? b)
    {
        if (ReferenceEquals(a, b))
            return true;

        if (a is null || b is null)
            return false;

        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Vector2D? a, Vector2D? b)
    {
        return !(a == b);
    }

    public override string ToString()
    {
        return $"({X}; {Y})";
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Vector2D other)
            return false;

        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}

class Program
{
    static void Main()
    {
        Vector2D vector1 = new Vector2D(3, 4);
        Vector2D vector2 = new Vector2D(1, 2);
        Vector2D vector3 = new Vector2D(5, 6);

        Console.WriteLine($"Vector 1: {vector1}");
        Console.WriteLine($"Vector 2: {vector2}");

        vector1.X = 5;
        vector1.Y = 6;

        Console.WriteLine($"Змінений Vector 1: {vector1}");
        Console.WriteLine($"UnitX: {Vector2D.UnitX}");

        Console.WriteLine($"{vector1} + {vector2} = {vector1 + vector2}");
        Console.WriteLine($"{vector1} - {vector2} = {vector1 - vector2}");

        Console.WriteLine($"vector1 == vector3: {vector1 == vector3}");
        Console.WriteLine($"vector1 != vector2: {vector1 != vector2}");
        Console.WriteLine($"vector1.Equals(vector3): {vector1.Equals(vector3)}");
        Console.WriteLine($"HashCode vector1: {vector1.GetHashCode()}");
    }
}
