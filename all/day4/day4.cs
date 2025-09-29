using System.ComponentModel;
using System.Text.RegularExpressions;
using static HelperExtensions;

public static class day4
{
    public static void Run()
    {
        var scratchoffs = System.IO.File.ReadAllLines(@$"day4/input.txt").ToList();

        (List<Scratchoff> scratchoffData, Dictionary<int, int> scratchoffCardCounts) = GetScratchoffData(scratchoffs);

        Part1(scratchoffData);
        Part2(scratchoffCardCounts);
    }

    private static (List<Scratchoff>, Dictionary<int, int>) GetScratchoffData(List<string> scratchoffs)
    {
        var scratchoffCardCounts = new Dictionary<int, int>();
        Enumerable
            .Range(1, scratchoffs.Count)
            .ToList()
            .ForEach(n => { scratchoffCardCounts[n] = 1; });

        var scratchoffData = scratchoffs.Select(s => {
            var match = Regex.Match(s, @$"Card\s+(\d+):(?:\s+(\d+))+\s\|(?:\s+(\d+))+");
            var cardNumber = int.Parse(match.Groups[1].Value);
            var winningNumbers = match.Groups[2].Captures.Select(c => int.Parse(c.Value)).ToList();
            var myNumbers = match.Groups[3].Captures.Select(c => int.Parse(c.Value)).ToList();
            var scratchoff = new Scratchoff { CardNumber = cardNumber, WinningNumbers = winningNumbers, MyNumbers = myNumbers };
            Enumerable
                .Range(cardNumber + 1, scratchoff.MatchingNumbers.Count)
                .ToList()
                .ForEach(n => { scratchoffCardCounts[n] += 1 * scratchoffCardCounts[cardNumber]; });
            return new Scratchoff { CardNumber = cardNumber, WinningNumbers = winningNumbers, MyNumbers = myNumbers };
        }).ToList();

        return (scratchoffData, scratchoffCardCounts);
    }

    private static void Part1(List<Scratchoff> scratchoffData)
    {
        // final solution
        Console.WriteLine(@$"Part 1: {
            scratchoffData
                .Where(s => s.MatchingNumbers.Count > 0)
                .Sum(s => Math.Pow(2, s.MatchingNumbers.Count - 1))
        }");
    }

    private static void Part2(Dictionary<int, int> scratchoffCardCounts)
    {
        // final solution
        Console.WriteLine(@$"Part 2: {
            scratchoffCardCounts
                .Sum(s => s.Value)
        }");
    }

    private class Scratchoff
    {
        public int CardNumber { get; set; }
        public List<int> WinningNumbers { get; set; } = new List<int>();
        public List<int> MyNumbers { get; set; } = new List<int>();
        public List<int> MatchingNumbers
        {
            get
            {
                return MyNumbers.Intersect(WinningNumbers).ToList();
            }
        } 
    }
}