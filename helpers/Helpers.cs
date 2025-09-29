using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

public static class HelperExtensions
{
    public static IEnumerable<char> NeighborsOf(this List<string> stringMatrix, Coordinate coordinate)
    {
        for (int row = Math.Max(coordinate.Row - 1, 0); row <= Math.Min(coordinate.Row + 1, stringMatrix.Count - 1); row++)
        {
            for (int col = Math.Max(coordinate.Col - 1, 0); col <= Math.Min(coordinate.Col + 1, stringMatrix[row].Length - 1); col++)
            {
                if ((row,col) == (coordinate.Row, coordinate.Col)) continue;

                yield return stringMatrix[row][col];
            }
        }
    }

    public static IEnumerable<Coordinate> NeighborCoordinates(this Coordinate coordinate, Size size)
    {
        for (int row = Math.Max(coordinate.Row - 1, 0); row <= Math.Min(coordinate.Row + 1, size.Rows - 1); row++)
        {
            for (int col = Math.Max(coordinate.Col - 1, 0); col <= Math.Min(coordinate.Col + 1, size.Cols - 1); col++)
            {
                if ((row,col) == (coordinate.Row, coordinate.Col)) continue;

                yield return new Coordinate(row, col);
            }
        }
    }

    public static T[] GetColumn<T>(this T[,] matrix, int columnNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(0))
                .Select(x => matrix[x, columnNumber])
                .ToArray();
    }

    public static T[] GetRow<T>(T[,] matrix, int rowNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[rowNumber, x])
                .ToArray();
    }

    public static bool IsNumeric(this char value)
    {
        return char.IsNumber(value);
    }

    public static bool IsNumeric(this string value)
    {
        return value.All(char.IsNumber);
    }

    public static bool IsSymbol(this char value)
    {
        return Regex.Match($"{value}", @"[^A-Za-z0-9\.]").Success;
    }

    public static bool Between(this int value, int minValue, int maxValue)
    {
        return (value >= minValue && value <= maxValue);
    }

    public static bool Between(this long value, long minValue, long maxValue)
    {
        return (value >= minValue && value <= maxValue);
    }
}

public static class Helpers
{
    public static string GetNumberFromWord(string word)
    {
        if (word.IsNumeric()) { return word; }
        var numbersAsWords = new Dictionary<string, string> {
            { "one", "1" },
            { "two", "2" },
            { "three", "3" },
            { "four", "4" },
            { "five", "5" },
            { "six", "6" },
            { "seven", "7" },
            { "eight", "8" },
            { "nine", "9" }
        };
        var compiledRegex = new Regex(@"\w+", RegexOptions.Compiled);
        return compiledRegex.Replace(word, match => numbersAsWords[match.Value]);
    }
}

public record Coordinate(int Row, int Col);

public record Size(int Rows, int Cols);

public record Cell<T>(Coordinate Coordinate, T Value);

public class Matrix<T>
{
    protected readonly Size _size;
    protected readonly Cell<T>[,] _matrix;
    protected readonly T _initValue;

    public Matrix(Size size, T initValue)
    {
        _size = size;
        _matrix = new Cell<T>[size.Rows, size.Cols];
        _initValue = initValue;
        Initialize();
    }

    private void Initialize()
    { 
        for (int m = 0; m < _size.Rows; m++)
        {
            for (int n = 0; n < _size.Cols; n++)
            {
                _matrix[m, n] = new Cell<T> (new Coordinate(m, n), _initValue);
            }
        }
    }
    
    public Cell<T> At(Coordinate coordinate) => _matrix[coordinate.Row, coordinate.Col];
    
    public void ChangeValue(Coordinate coordinate, T value) => _matrix[coordinate.Row, coordinate.Col] = new Cell<T> (coordinate, value);
}