using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using static HelperExtensions;

public static class day5
{
    public static void Run()
    {
        var mappingData = System.IO.File.ReadAllLines(@$"day5/input.txt").ToList();

        (var almanac, var seedData) = ReadAlmanac(mappingData);

        Part1(seedData, almanac);
        Part2(seedData, almanac);
    }

    private static (List<MappingType>, string) ReadAlmanac(List<string> mappingData)
    {
        var seedData = mappingData.First();
        var almanac = new List<MappingType>();
        MappingType? mappingType = null;

        for(var i = 2; i < mappingData.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(mappingData[i])) { continue; }

            var matchMapping = Regex.Match(mappingData[i], @"(\d+) (\d+) (\d+)");
            if (matchMapping.Success)
            {
                mappingType!.Mappings.Add(new Mapping { DestinationStart = long.Parse(matchMapping.Groups[1].Value), SourceStart = long.Parse(matchMapping.Groups[2].Value), Length = long.Parse(matchMapping.Groups[3].Value)});
            }

            var matchMappingType = Regex.Match(mappingData[i], @"(\w+)-to-(\w+)");
            if (matchMappingType.Success || i == mappingData.Count - 1)
            {
                if (mappingType != null) { almanac.Add(mappingType); }
                mappingType = new MappingType { SourceType = matchMappingType.Groups[1].Value, DestinationType = matchMappingType.Groups[2].Value };
                continue;
            }
        }

        return (almanac, seedData);
    }

    private static void Part1(string seedData, List<MappingType> almanac)
    {
        var seeds = Regex.Matches(seedData, @"\d+").Select(m => long.Parse(m.Value)).ToList();

        // final solution
        Console.WriteLine(@$"Part 1: {
            seeds
                .Select(s => {
                    return MapSeedToLocation(s, almanac);
                })
                .Min()
        }");
    }

    private static void Part2(string seedData, List<MappingType> almanac)
    {
        var seeds = Regex.Matches(seedData, @"(?:(\d+)\s(\d+))").Select(m => new SeedRange { Start = long.Parse(m.Groups[1].Value), Length = long.Parse(m.Groups[2].Value) }).ToList();

        // final solution
        Console.WriteLine(@$"Part 2: {
            seeds
                .Select(s => {
                    var minLocationForSeedRange = FindMinInRange(s, almanac);
                    return minLocationForSeedRange;
                })
                .Min()
        }");
    }

    private static long FindMinInRange(SeedRange seedRange, List<MappingType> almanac)
    {
        var rangeStartLocation = MapSeedToLocation(seedRange.Start, almanac);
        var rangeEndLocation = MapSeedToLocation(seedRange.End, almanac);
        var seedDiff = seedRange.End - seedRange.Start;
        var locationDiff = rangeEndLocation - rangeStartLocation;

        if (seedRange.Length == 1)
        {
            return Math.Min(MapSeedToLocation(seedRange.Start, almanac), MapSeedToLocation(seedRange.Start + 1, almanac));
        }
        
        var middle = (seedRange.Start + seedRange.End) / 2;
        var rangeMiddleLocation = MapSeedToLocation(middle, almanac);

        if (seedDiff == locationDiff) { return rangeStartLocation; }

        var minLocation = long.MaxValue;
        if (rangeStartLocation + (seedRange.Start - middle) != rangeMiddleLocation)
        {
            minLocation = Math.Min(minLocation, FindMinInRange(new SeedRange { Start = seedRange.Start, Length = middle - seedRange.Start }, almanac));
        }
        if (rangeMiddleLocation + (seedRange.End - middle) != rangeEndLocation)
        {
            minLocation = Math.Min(minLocation, FindMinInRange(new SeedRange { Start = middle + 1, Length = seedRange.End - middle + 1 }, almanac));
        }
        
        return minLocation;
    }

    private static long MapSeedToLocation(long seed, List<MappingType> almanac)
    {
        return MapSourceToDestination("seed", seed, "location", almanac);
    }

    private static long MapSourceToDestination(string sourceType, long sourceValue, string destinationType, List<MappingType> almanac)
    {
        var mappingType = almanac.Where(m => m.SourceType == sourceType).First();
        var destinationValue = mappingType.GetMappedValue(sourceValue);
        if (mappingType.DestinationType == destinationType) { return destinationValue; }
        return MapSourceToDestination(mappingType.DestinationType, destinationValue, destinationType, almanac);
    }

    private class MappingType
    {
        public string SourceType { get; set; } = "";
        public string DestinationType { get; set; } = "";
        public List<Mapping> Mappings { get; set; } = new List<Mapping>();
        public long GetMappedValue(long sourceValue)
        {
            var mapping = Mappings.Where(m => m.ContainsSource(sourceValue)).FirstOrDefault();
            if (mapping == null) { return sourceValue; }
            return (sourceValue - mapping.SourceStart) + mapping.DestinationStart;
        }
    }

    private class Mapping
    {
        public long SourceStart { get; set; }
        public long SourceEnd
        {
            get
            {
                return SourceStart + Length - 1;
            }
        }
        public long DestinationStart { get; set; }
        public long DestinationEnd
        {
            get
            {
                return DestinationStart + Length - 1;
            }
        }
        public long Length { get; set; }
        public bool ContainsSource(long sourceValue)
        {
            return sourceValue.Between(SourceStart, SourceEnd);
        }
        public bool ContainsDestination(long destinationValue)
        {
            return destinationValue.Between(DestinationStart, DestinationEnd);
        }
    }

    private class SeedRange
    {
        public long Start { get; set; }
        public long End
        {
            get
            {
                return Start + Length - 1;
            }
        }
        public long Length { get; set; }
    }
}