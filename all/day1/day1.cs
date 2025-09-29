using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.VisualBasic;

public static class day1
{
    public static void Run()
    {
        var locationData = System.IO.File.ReadAllLines(@$"day1/input.txt").ToList();
        var locationIDs = locationData.Select<string, (int location1, int location2)>(locationIDPair =>
        {
            var matches = Regex.Matches(locationIDPair, "\\d+");
            return (int.Parse(matches[0].Value), int.Parse(matches[1].Value));
        });
        Part1(locationIDs.Select(x => x.location1).Order().ToList(), locationIDs.Select(x => x.location2).Order().ToList());
        Part2(locationIDs.Select(x => x.location1).ToList(), locationIDs.Select(x => x.location2).ToList());
    }

    private static void Part1(List<int> locationID1s, List<int> locationID2s)
    {
        // final solution
        Console.WriteLine(@$"Part 1: {Enumerable.Range(0, locationID1s.Count()).Select(i => Math.Abs(locationID1s[i] - locationID2s[i])).Sum()}");
    }

    private static void Part2(List<int> locationID1s, List<int> locationID2s)
    {
        // final solution
        Console.WriteLine(@$"Part 2: {
            locationID1s
                .Distinct()
                .Select(x => x * locationID1s.Where(l1 => l1 == x).Count() * locationID2s.Where(l2 => l2 == x).Count())
                .Sum()
        }");
    }
}