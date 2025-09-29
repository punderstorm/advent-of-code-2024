using System.ComponentModel;
using System.Text.RegularExpressions;
using static HelperExtensions;

public static class day3
{
    public static void Run()
    {
        var corruptMemory = string.Join("", System.IO.File.ReadAllLines(@$"day3/input.txt"));

        Part1(corruptMemory);
        Part2(corruptMemory);
    }
    
    private static void Part1(string corruptMemory)
    {
        var instructions = Regex.Matches(corruptMemory, "mul\\((\\d{1,3}),(\\d{1,3})\\)");

        // final solution
        Console.WriteLine(@$"Part 1: {instructions
                .Sum(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value))}");
    }

    private static void Part2(string corruptMemory)
    {
        var instructions = Regex.Matches(corruptMemory, "(?:do\\(\\)|don't\\(\\)|mul\\((\\d{1,3}),(\\d{1,3})\\))");
        var enabled = true;

        // final solution
        Console.WriteLine(@$"Part 2: {instructions
                .Sum(match =>
                {
                    switch (match.Groups[0].Value)
                    {
                        case "do()":
                            enabled = true;
                            break;
                        case "don't()":
                            enabled = false;
                            break;
                        default:
                            return enabled.ToInt() * int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
                    }

                    return 0;
                })}");
    }
}