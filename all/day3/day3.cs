using System.ComponentModel;
using System.Text.RegularExpressions;
using static HelperExtensions;

public static class day3
{
    public static void Run()
    {
        var schematics = System.IO.File.ReadAllLines(@$"day3/input.txt").ToList();

        List<PartNumber> schematicPartNumbers;
        List<Gear> schematicGears;
        (schematicPartNumbers, schematicGears) = ReadSchematics(schematics);

        Part1(schematicPartNumbers);
        Part2(schematicGears);
    }

    private static (List<PartNumber>, List<Gear>) ReadSchematics(List<string> schematics)
    {
        var numRows = schematics.Count;
        var numCols = schematics.First().Length;
        PartNumber? currentSchematicPartNumber = null;
        var schematicPartNumbers = new List<PartNumber>();
        var schematicGears = new List<Gear>();

        for(var row = 0; row < numRows; row++)
        {
            for(var col = 0; col < numCols; col++)
            {
                var colValue = schematics[row][col];
                var isPartNumber = colValue.IsNumeric();
                var isSymbol = colValue.IsSymbol();
                var isGear = (colValue == '*');

                if (isPartNumber && currentSchematicPartNumber == null)
                {
                    currentSchematicPartNumber = new PartNumber { Value = $"{colValue}", StartCoordinate = new Coordinate(row, col) };
                }
                else if (currentSchematicPartNumber != null && isPartNumber)
                {
                    currentSchematicPartNumber.Value += $"{colValue}";
                }
                else if (currentSchematicPartNumber != null && (!isPartNumber || col == (numCols - 1)))
                {
                    var startCol = currentSchematicPartNumber.StartCoordinate?.Col ?? 0;
                    var hasSymbolNeighbor = false;
                    Enumerable.Range(startCol, currentSchematicPartNumber.Value.Length).ToList().ForEach(c => {
                        hasSymbolNeighbor |= (schematics.NeighborsOf(new Coordinate(row, c)).Where(s => s.IsSymbol()).Count() > 0);
                    });
                    
                    currentSchematicPartNumber.EndCoordinate = new Coordinate(row, col);
                    currentSchematicPartNumber.HasSymbolNeighbor = hasSymbolNeighbor;
                    schematicPartNumbers.Add(currentSchematicPartNumber);
                    currentSchematicPartNumber = null;
                }

                if (isGear)
                {
                    schematicGears.Add(new Gear { Position = new Coordinate(row, col) });
                }
            }
        }

        schematicGears.ForEach(g => {
            g.NeighborParts = schematicPartNumbers.Where(p => p.AllCoordinates.Intersect(g.Position?.NeighborCoordinates(new Size(numRows, numCols)) ?? new List<Coordinate>()).Count() > 0).ToList();
        });
        
        return (schematicPartNumbers, schematicGears);
    }
    

    private static void Part1(List<PartNumber> schematicPartNumbers)
    {
        // final solution
        Console.WriteLine(@$"Part 1: {
            schematicPartNumbers
                .Where(p => p.HasSymbolNeighbor)
                .Sum(p => int.Parse(p.Value))
        }");
    }

    private static void Part2(List<Gear> schematicGears)
    {
        // final solution
        Console.WriteLine(@$"Part 2: {
            schematicGears
                .Where(g => g.NeighborParts.Count == 2)
                .Sum(g => int.Parse(g.NeighborParts[0].Value) * int.Parse(g.NeighborParts[1].Value))
        }");
    }

    private class PartNumber
    {
        public string Value { get; set; } = "";
        public Coordinate? StartCoordinate { get; set; } = null;
        public Coordinate? EndCoordinate { get; set; } = null;
        public IEnumerable<Coordinate> AllCoordinates 
        {
            get
            {
                return Enumerable
                    .Range(StartCoordinate?.Col ?? 0, Value.Length)
                    .Select(c => new Coordinate(StartCoordinate?.Row ?? 0, c));
            }
        } 
        public bool HasSymbolNeighbor { get; set; } = false;
    }

    private class Gear
    {
        public Coordinate? Position { get; set; } = null;
        public List<PartNumber> NeighborParts { get; set; } = new List<PartNumber>();
    }
}