using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using static HelperExtensions;

public static class day6
{
    public static void Run()
    {
        var raceData = System.IO.File.ReadAllLines(@$"day6/input.txt").ToList();

        Part1(raceData);
        Part2(raceData);
    }

    private static void Part1(List<string> raceData)
    {
        // final solution
        Console.WriteLine(@$"Part 1: {GetNumWinningWays(ReadRaceData1(raceData))}");
    }

    private static List<Race> ReadRaceData1(List<string> raceData)
    {
        var raceLengths = Regex.Matches(raceData[0], @"\d+");
        var raceRecords = Regex.Matches(raceData[1], @"\d+");

        return
            Enumerable
                .Range(0, raceLengths.Count)
                .ToList()
                .Select(i => new Race { RaceLength = int.Parse(raceLengths[i].Value), RecordDistance = int.Parse(raceRecords[i].Value) })
                .ToList();
    }

    private static void Part2(List<string> raceData)
    {
        // final solution
        Console.WriteLine(@$"Part 2: {GetNumWinningWays(ReadRaceData2(raceData))}");
    }

    private static List<Race> ReadRaceData2(List<string> raceData)
    {
        var raceLengths = Regex.Matches(raceData[0].Replace(" ", ""), @"\d+");
        var raceRecords = Regex.Matches(raceData[1].Replace(" ", ""), @"\d+");

        return
            Enumerable
                .Range(0, raceLengths.Count)
                .ToList()
                .Select(i => new Race { RaceLength = long.Parse(raceLengths[i].Value), RecordDistance = long.Parse(raceRecords[i].Value) })
                .ToList();
    }

    private static double GetNumWinningWays(List<Race> races)
    {
        return races
            .Select(r => {
                var midpoint = r.RaceLength / 2.0;

                // find the offset of the record distance from the midpoint distance
                var offset = Math.Floor(Math.Sqrt(Math.Pow(midpoint, 2) - r.RecordDistance));

                // find min/max button hold lengths, accounting for halves in the midpoint
                var minLength = Math.Floor(midpoint) - offset;
                var maxLength = Math.Ceiling(midpoint) + offset;

                // number of winning ways is the difference plus one (e.g. 5 winning ways between 2 and 6 - 2,3,4,5,6)
                return maxLength - minLength + 1;
            })
            .Aggregate((curr, next) => curr * next);
    }

    private class Race
    {
        public long RaceLength { get; set; }
        public long RecordDistance { get; set; }
    }
}