using System.Collections.Immutable;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.RegularExpressions;
using static HelperExtensions;

public static class day7
{
    public static void Run()
    {
        var input = System.IO.File.ReadAllLines(@$"day7/input.txt").ToList();
        var hands = ParseHands(input);

        Part1(hands);
        Part2(hands);
    }

    private static List<Hand> ParseHands(List<string> input)
    {
        return input.Select(i => {
            var data = Regex.Match(i, @"([AKQJT98765432]{5}) (\d+)");
            var hand = new Hand { HandCards = data.Groups[1].Value, Bid = long.Parse(data.Groups[2].Value) };
            return hand;
        })
        .ToList();
    }

    private static void Part1(List<Hand> hands)
    {
        hands
            .Sort(new HandRankingComparer());

        // final solution
        Console.WriteLine(@$"Part 1: {
            hands
                .Select((hand, rank) => hand.Bid * (rank+1))
                .Sum()
        }");
    }

    private static void Part2(List<Hand> hands)
    {
        hands
            .Sort(new JokerHandRankingComparer());

        // final solution
        Console.WriteLine(@$"Part 1: {
            hands
                .Select((hand, rank) => hand.Bid * (rank+1))
                .Sum()
        }");
    }


    private class Hand
    {
        public string HandCards { get; set; }
        public long Bid { get; set; }
        private HandType? _handType;
        public HandType HandType
        {
            get
            {
                if (_handType != null) { return _handType.Value; }

                var cardGroups = HandCards.GroupBy(group => group, group => group, (group, count) => new { Key = group, Value = HandCards.Count(card => card == group) });
                if (cardGroups.Where(group => group.Value == 5).Count() > 0) { return (_handType = HandType.FiveOfAKind).Value; }
                if (cardGroups.Where(group => group.Value == 4).Count() > 0) { return (_handType = HandType.FourOfAKind).Value; }
                if (cardGroups.Where(group => group.Value == 3).Count() > 0 && cardGroups.Where(group => group.Value == 2).Count() > 0) { return (_handType = HandType.FullHouse).Value; }
                if (cardGroups.Where(group => group.Value == 3).Count() > 0) { return (_handType = HandType.ThreeOfAKind).Value; }
                if (cardGroups.Where(group => group.Value == 2).Count() > 1) { return (_handType = HandType.TwoPair).Value; }
                if (cardGroups.Where(group => group.Value == 2).Count() > 0) { return (_handType = HandType.OnePair).Value; }

                return (_handType = HandType.HighCard).Value;
            }
        }
        private HandType? _jokerHandType;
        public HandType JokerHandType
        {
            get
            {
                if (_jokerHandType != null) { return _jokerHandType.Value; }

                var cardGroups = HandCards.GroupBy(group => group, group => group, (group, count) => new { Key = group, Value = HandCards.Count(card => card == group) });
                if (cardGroups.Where(group => group.Value == 5).Count() > 0) { return (_jokerHandType = HandType.FiveOfAKind).Value; }
                if (cardGroups.Where(group => group.Value == 4).Count() > 0) { return (_jokerHandType = HandType.FourOfAKind).Value; }
                if (cardGroups.Where(group => group.Value == 3).Count() > 0 && cardGroups.Where(group => group.Value == 2).Count() > 0) { return (_jokerHandType = HandType.FullHouse).Value; }
                if (cardGroups.Where(group => group.Value == 3).Count() > 0) { return (_jokerHandType = HandType.ThreeOfAKind).Value; }
                if (cardGroups.Where(group => group.Value == 2).Count() > 1) { return (_jokerHandType = HandType.TwoPair).Value; }
                if (cardGroups.Where(group => group.Value == 2).Count() > 0) { return (_jokerHandType = HandType.OnePair).Value; }

                return (_jokerHandType = HandType.HighCard).Value;
            }
        }
        public List<byte> HandCardRanks
        {
            get
            {
                return HandCards.Select(c => AllCards[c]).ToList();
            }
        }
    }

    private enum HandType
    {
        HighCard=1,
        OnePair=2,
        TwoPair=3,
        ThreeOfAKind=4,
        FullHouse=5,
        FourOfAKind=6,
        FiveOfAKind=7
    }

    private record Card (char Value, byte Rank);

    private static readonly Dictionary<char, byte> AllCards = new Dictionary<char, byte> {
        {'A', 13},
        {'K', 12},
        {'Q', 11},
        {'J', 10},
        {'T', 9},
        {'9', 8},
        {'8', 7},
        {'7', 6},
        {'6', 5},
        {'5', 4},
        {'4', 3},
        {'3', 2},
        {'2', 1}
    };

    private class HandRankingComparer : Comparer<Hand>
    {
        public override int Compare(Hand first, Hand second)
        {
            if (first.HandType < second.HandType) { return -1; }
            if (second.HandType < first.HandType) { return 1; }

            if (first.HandCardRanks.Count != second.HandCardRanks.Count) { throw new Exception("Hand size mismatch."); }

            var handSize = first.HandCardRanks.Count;
            foreach(var i in Enumerable.Range(0, handSize))
            {
                if (first.HandCardRanks[i] < second.HandCardRanks[i]) { return -1; }
                if (second.HandCardRanks[i] < first.HandCardRanks[i]) { return 1; }
            }

            return 0;
        }
    }
    private class JokerHandRankingComparer : Comparer<Hand>
    {
        public override int Compare(Hand first, Hand second)
        {
            if (first.JokerHandType < second.JokerHandType) { return -1; }
            if (second.JokerHandType < first.JokerHandType) { return 1; }

            if (first.HandCardRanks.Count != second.HandCardRanks.Count) { throw new Exception("Hand size mismatch."); }

            var handSize = first.HandCardRanks.Count;
            foreach(var i in Enumerable.Range(0, handSize))
            {
                if (first.HandCardRanks[i] < second.HandCardRanks[i]) { return -1; }
                if (second.HandCardRanks[i] < first.HandCardRanks[i]) { return 1; }
            }

            return 0;
        }
    }
}