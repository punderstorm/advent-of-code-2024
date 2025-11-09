using System.ComponentModel;
using System.Drawing;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using static HelperExtensions;

public static class day4
{
    private static readonly string XMAS = "XMAS";

    public static void Run()
    {
        var wordSearch = System.IO.File.ReadAllLines(@$"day4/input.txt").ToList();

        Console.WriteLine($"Part 1: {FindXMASCount(wordSearch)}");

        Console.WriteLine($"Part 2: {FindX_MASCount(wordSearch)}");
    }

    private static List<int> FindAllIndexesOf(string value, char letter)
    {
        var foundIndexes = new List<int>();

        for (int i = value.IndexOf(letter); i > -1; i = value.IndexOf(letter, i + 1))
        {
            foundIndexes.Add(i);
        }

        return foundIndexes;
    } 

    private static int FindXMASCount(List<string> wordSearch)
    {
        var indexesOfX = wordSearch.Select(line => FindAllIndexesOf(line, 'X')).ToArray();

        var xmasCount = 0;
        var lineLength = wordSearch[0].Length;
        var directions = new List<Point> {
            new Point(-1, 0),
            new Point(-1, 1),
            new Point(0, 1),
            new Point(1, 1),
            new Point(1, 0),
            new Point(1, -1),
            new Point(0, -1),
            new Point(-1, -1)
        };
        for (int row = 0; row < indexesOfX.Count(); row++)
        {
            foreach (int col in indexesOfX[row])
            {
                foreach (Point direction in directions)
                {
                    for (int i = 1; i < XMAS.Count(); i++)
                    {
                        var rowToCheck = row + direction.X * i;
                        var colToCheck = col + direction.Y * i;

                        if (rowToCheck < 0 || colToCheck < 0 || rowToCheck >= indexesOfX.Count() || colToCheck >= lineLength)
                        {
                            break;
                        }

                        if (wordSearch[rowToCheck][colToCheck] != XMAS[i])
                        {
                            break;
                        }

                        if (i == XMAS.Count() - 1)
                        {
                            xmasCount++;
                        }
                    }
                }
            }
        }

        return xmasCount;
    }

    private static int FindX_MASCount(List<string> wordSearch)
    {
        var indexesOfA = wordSearch.Select(line => FindAllIndexesOf(line, 'A')).ToArray();

        var matches = new List<string>
        {
            "MAS",
            "SAM"
        };

        var x_masCount = 0;
        var lineLength = wordSearch[0].Length;

        for (int row = 0; row < wordSearch.Count; row++)
        {
            foreach (int col in indexesOfA[row])
            {
                if (row - 1 < 0 || col - 1 < 0 || row + 1 >= wordSearch.Count || col + 1 >= lineLength)
                {
                    continue;
                }

                var nwtose = $"{wordSearch[row - 1][col - 1]}A{wordSearch[row + 1][col + 1]}";
                var netosw = $"{wordSearch[row - 1][col + 1]}A{wordSearch[row + 1][col - 1]}";

                if (matches.Contains(nwtose) && matches.Contains(netosw))
                {
                    x_masCount++;
                }
            }
        }

        return x_masCount;
    }
}