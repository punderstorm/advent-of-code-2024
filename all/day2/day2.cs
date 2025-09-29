using System.Text.RegularExpressions;

public static class day2
{
    public static void Run()
    {
        var levelData = System.IO.File.ReadAllLines(@$"day2/input.txt")
            .Select(x =>
                Regex
                    .Matches(x, "\\d+")
                    .Select(x => int.Parse(x.Value))
                    .ToList()
            )
            .ToList();

        Part1(levelData);
        Part2(levelData);
    }

    private static void Part1(List<List<int>> levelData)
    {
        // final solution
        Console.WriteLine(@$"Part 1: {levelData
                .Where(levels => VerifyLevels(levels))
                .Count()}");
    }

    private static void Part2(List<List<int>> levelData)
    {
        // final solution
        Console.WriteLine(@$"Part 2: {levelData
                .Where(levels =>
                {
                    if (VerifyLevels(levels)) { return true; }

                    for (int i = 0; i < levels.Count(); i++)
                    {
                        if (VerifyLevels(levels.Where((x, j) => i != j).ToList())) { return true; }
                    }

                    return false;
                })
                .Count()}");
    }

    private static bool VerifyLevels(List<int> levels)
    {
        bool? increaseExpectation = null;
        for (int i = 1; i < levels.Count(); i++)
        {
            var difference = (levels[i] - levels[i - 1]);
            if ((increaseExpectation ??= (difference > 0)) ^ (difference > 0) || Math.Abs(difference) is < 1 or > 3)
            {
                return false;
            }
        }

        return true;
    }
}