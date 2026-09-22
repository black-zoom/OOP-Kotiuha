using System;

public class Grid<T>
{
    private readonly T[,] _grid;

    public int Width => _grid.GetLength(0);
    public int Height => _grid.GetLength(1);

    public Grid(int width, int height)
    {
        if (width <= 0 || height <= 0)
            throw new ArgumentException("Розміри сітки повинні бути більшими за 0.");

        _grid = new T[width, height];
    }

    public T this[int x, int y]
    {
        get => _grid[x, y];
        set => _grid[x, y] = value;
    }

    public void Fill(T value)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                _grid[x, y] = value;
            }
        }
    }

    public static bool operator ==(Grid<T>? a, Grid<T>? b)
    {
        if (ReferenceEquals(a, b))
            return true;

        if (a is null || b is null)
            return false;

        if (a.Width != b.Width || a.Height != b.Height)
            return false;

        for (int x = 0; x < a.Width; x++)
        {
            for (int y = 0; y < a.Height; y++)
            {
                if (!Equals(a[x, y], b[x, y]))
                    return false;
            }
        }

        return true;
    }

    public static bool operator !=(Grid<T>? a, Grid<T>? b)
    {
        return !(a == b);
    }

    public override bool Equals(object? obj)
    {
        return obj is Grid<T> other && this == other;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();

        hash.Add(Width);
        hash.Add(Height);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                hash.Add(_grid[x, y]);
            }
        }

        return hash.ToHashCode();
    }

    public override string ToString()
    {
        string result = "";

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                result += $"{_grid[x, y]} ";
            }

            result += Environment.NewLine;
        }

        return result;
    }
}

class Program
{
    static void Main()
    {
        Grid<int> grid1 = new Grid<int>(2, 2);
        Grid<int> grid2 = new Grid<int>(2, 2);

        grid1[0, 0] = 1;
        grid1[1, 0] = 2;
        grid1[0, 1] = 3;
        grid1[1, 1] = 4;

        grid2.Fill(1);
        grid2[1, 0] = 2;
        grid2[0, 1] = 3;
        grid2[1, 1] = 4;

        Console.WriteLine(grid1);
        Console.WriteLine($"grid1[1, 1] = {grid1[1, 1]}");
        Console.WriteLine($"Width: {grid1.Width}, Height: {grid1.Height}");
        Console.WriteLine($"grid1 == grid2: {grid1 == grid2}");
        Console.WriteLine($"grid1 != grid2: {grid1 != grid2}");
        Console.WriteLine($"grid1.Equals(grid2): {grid1.Equals(grid2)}");
    }
}
