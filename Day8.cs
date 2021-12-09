using System.Data;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Advent_of_Code_2021
{
    public class Day8 : Day
    {
        // Idea Explanation
        // We can easily find 1,4,7,8 
        // For other digits we will create mapping based on how many lines they have common with digits we already know    
        //       1   4   7   8   SUM
        //    --------------------------
        // 0     2   3   3   6   14
        // 2     1   2   2   5   10
        // 3     2   3   3   5   13
        // 5     1   3   2   5   11
        // 6     1   3   2   6   12
        // 9     2   4   3   6   15
        // 
        // this mapping will be in 'SumWithEasyDigitsDictionary' collection
        public static class DigitsMapper
        {
            public static Dictionary<int, int> DigitsWithUniqueLinesDictionary = new Dictionary<int, int>
            {
                {2, 1},
                {3, 7},
                {4 ,4},
                {7, 8}
            };

            public static Dictionary<int, int> SumWithEasyDigitsDictionary = new Dictionary<int, int>
            {
                {14, 0},
                {10, 2},
                {13, 3},
                {11, 5},
                {12, 6},
                {15, 9}
            };
            public static int GetResultOfPartOne(List<DataRow> rows)
            {
                var result = 0;

                foreach (var row in rows)
                {
                    foreach (var outputValue in row.OutputValues)
                    {
                        if (DigitsWithUniqueLinesDictionary.ContainsKey(outputValue.Length)) result++;
                    }
                }

                return result;
            }

            private static int CountCommonChars(string s1, string s2)
            {
                var charsSet = new HashSet<char>();
                foreach (var c in s1)
                {
                    charsSet.Add(c);
                }
                return s2.Count(c => charsSet.Contains(c));
            }

            public static int GetResultOfPartTwo(List<DataRow> rows)
            {
                int result = 0;
                foreach (var row in rows)
                {
                    var mappings = new Dictionary<int, string>();
                    FindMappingForRow(row, mappings);
                    var currentNumber = CountResultForRow(row, mappings);
                    result += currentNumber;
                }

                return result;
            }

            private static int CountResultForRow(DataRow row, Dictionary<int, string> mappings)
            {
                var currentNumber = 0;
                foreach (var v in row.OutputValues)
                {
                    foreach (var mapping in mappings)
                    {
                        if (IsTheSameStringDifferentOrder(v, mapping.Value))
                        {
                            currentNumber *= 10;
                            currentNumber += mapping.Key;
                        }
                    }
                }

                return currentNumber;
            }

            private static void FindMappingForRow(DataRow row, Dictionary<int, string> mappings)
            {
                var valuesToMap = new List<string>(row.SignalPatterns);
                // add easy digits to mapping
                for (int i = 0; i < valuesToMap.Count; i++)
                {
                    var currentPattern = valuesToMap[i];
                    if (DigitsWithUniqueLinesDictionary.ContainsKey(currentPattern.Length))
                    {
                        mappings.Add(DigitsWithUniqueLinesDictionary[currentPattern.Length], currentPattern);
                    }
                }

                // remove easy digits from valuesToMap 
                foreach (var mapping in mappings)
                {
                    valuesToMap.Remove(mapping.Value);
                }

                // map others
                foreach (var v in valuesToMap)
                {
                    var sum = CountSumWithAllEasyDigits(v, mappings);
                    var digit = SumWithEasyDigitsDictionary[sum];
                    mappings.Add(digit, v);
                }
            }

            public static bool IsTheSameStringDifferentOrder(string s1, string s2)
            {
                return s1.Length == s2.Length && CountCommonChars(s1, s2) == s1.Length;
            }

            public static int CountSumWithAllEasyDigits(string s, Dictionary<int, string> mapping)
            {
                var sum = 0;
                foreach (var x in DigitsWithUniqueLinesDictionary.Values)
                {
                    sum += CountCommonChars(s, mapping[x]);
                }
                return sum;
            }
        }

        public struct DataRow
        {
            public List<string> SignalPatterns;
            public List<string> OutputValues;
        }

        public override void PrintOutput()
        {
            var rows = ReadData();
            Console.WriteLine($"Day 8 - part one: {DigitsMapper.GetResultOfPartOne(rows)}");
            Console.WriteLine($"Day 8 - part two: {DigitsMapper.GetResultOfPartTwo(rows)}");
        }

        public List<DataRow> ReadData()
        {
            var lines = Properties.Resources.DataDay8.Split("\r\n");
            var result = new List<DataRow>();

            foreach (var line in lines)
            {
                var lineParts = line.Split(" | ");

                result.Add(new DataRow
                {
                    SignalPatterns = lineParts[0].Split(' ').ToList(),
                    OutputValues = lineParts[1].Split(' ').ToList()
                }
                );
            }
            return result;
        }


    }
}