using System.ComponentModel.Design;
using System.Data;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Xml.Schema;

namespace Advent_of_Code_2021
{
    public class Day10 : Day
    {
        // 3 cases:
        // < correctString >
        // correctString <>
        // <> correctString
        // correct-substring.Length + 2 is the length of new possible correct string
        // 
        // Visualization of idea:
        //
        //    {()()()}
        // 2  -X-X-X--
        // 4  -X-X----
        // 6  -X------
        // 8  X-------
        // 
        // X - means that on this position the correct string with length (specified as row) starts
        private readonly Dictionary<char, char> _closingBrackets = new()
        {
            { '(', ')' },
            { '<', '>' },
            { '{', '}' },
            { '[', ']' },
        };

        public override void PrintOutput()
        {
            List<string> lines;
            ReadData(out lines, Properties.Resources.DataDay10);
            var listOfCorrectSubstrings = FindCorrectSubstringTables(lines);
            
            List<(string line, int corruptedIndex)> corruptedStrings;
            List<(string line, bool[,] table)> notCorruptedStrings;
            var resp1 = FindResultOfPartOne(listOfCorrectSubstrings, out corruptedStrings, out notCorruptedStrings);
            
            
            PrintCorruptedString(corruptedStrings[0].line, corruptedStrings[0].corruptedIndex);
            Console.WriteLine($"Day 10 - part 1: {resp1}");

            Dictionary<string, (string, List<int>)> missingBrackets;
            var resp2 = FindResultOfPartTwo(notCorruptedStrings, out missingBrackets);
            DrawBoard(notCorruptedStrings[0].line, notCorruptedStrings[0].table);
            PrintStringWithMissingBrackets(notCorruptedStrings[0].line, missingBrackets[notCorruptedStrings[0].line]);
            Console.WriteLine($"Day 10 - part 2: {resp2}");

        }

        private void PrintCorruptedString(string corruptedString, int index)
        {
            for (var i = 0; i < corruptedString.Length; i++)
            {
                if (index == i)
                {
                    ConsoleExtensions.ColorConsoleWrite(corruptedString[i].ToString(), ConsoleColor.Red);
                }
                else
                {
                    Console.Write(corruptedString[i]);
                }
            }
            Console.Write('\n');
        }

        private void PrintStringWithMissingBrackets(string stringWithMissingBrackets, (string, List<int>) bracketsWithInd)
        {
            Console.Write(new string(' ', 4));
            for (int i = 0; i < stringWithMissingBrackets.Length; i++)
            {
                if (bracketsWithInd.Item2.Contains(i))
                {
                    ConsoleExtensions.ColorConsoleWrite(stringWithMissingBrackets[i].ToString(), ConsoleColor.Blue);
                }
                else
                {
                    Console.Write(stringWithMissingBrackets[i].ToString());
                }
            }
            ConsoleExtensions.ColorConsoleWrite(bracketsWithInd.Item1, ConsoleColor.Green);
            Console.Write('\n');
        }

        public long FindResultOfPartOne(
            List<(string line, bool[,] table)> listOfCorrectSubstrings,
            out List<(string line, int corruptedIndex)> corruptedStrings,
            out List<(string line, bool[,] table)> notCorruptedStrings)
        {
            corruptedStrings = new List<(string line, int corruptedIndex)>();
            notCorruptedStrings = new List<(string, bool[,])>();

            var scoringDict = new Dictionary<char, int>
            {
                {')', 3},
                {']', 57},
                {'}', 1197},
                {'>', 25137},
            };
            var result = 0;

            foreach (var lineWithTable in listOfCorrectSubstrings)
            {
                var line = lineWithTable.line;
                var table = lineWithTable.table;

                var indexOfIllegalChar = int.MaxValue;

                for (var i = 0; i < line.Length; i++)
                {
                    for (var row = 0; row < table.GetLength(0); row++)
                    {
                        var currentStringLen = row * 2 + 2;
                        if (!table[row, i]) continue;
                        if (i == 0) continue;
                        if (i + currentStringLen >= line.Length) continue;
                        var bracketBefore = line[i - 1];
                        var bracketAfter = line[i + currentStringLen];
                        if (_closingBrackets.ContainsKey(bracketBefore) && !_closingBrackets.ContainsKey(bracketAfter) && _closingBrackets[bracketBefore] != bracketAfter)
                        {
                            if (indexOfIllegalChar >= i + currentStringLen)
                            {
                                indexOfIllegalChar = i + currentStringLen;
                                break;
                            }
                        }
                    }
                    if(indexOfIllegalChar != int.MaxValue) break;
                }
                //DrawBoard(line, table);

                if (indexOfIllegalChar != int.MaxValue)
                {
                    result += scoringDict[line[indexOfIllegalChar]];
                    corruptedStrings.Add((line, indexOfIllegalChar));
                }
                else
                {
                    notCorruptedStrings.Add((line, table));
                }
            }

            return result;
        }


        public ulong FindResultOfPartTwo(
            List<(string line, bool[,] table)> notCorruptedStrings, 
            out Dictionary<string, (string, List<int>)> missingBrackets)
        {
            missingBrackets = new Dictionary<string, (string, List<int>)>();
            var scores = new List<ulong>();

            foreach (var notCorrupted in notCorruptedStrings)
            {
                var line = notCorrupted.line;
                var table = notCorrupted.table;
                var closingBrackets = FindClosingBrackets(line, table);
                missingBrackets.Add(line, closingBrackets);
                scores.Add(CountSecondPartBracketsScore(closingBrackets.Item1));
            }

            scores.Sort();

            var middleIndx = scores.Count / 2;

            return scores[middleIndx];
        }

        private (string, List<int>) FindClosingBrackets(string line, bool[,] table)
        {
            var bracketsToClose = new List<char>();
            List<int> indexes = new List<int>();

            var ind = 0;
            while (ind < line.Length)
            {
                var correctSubstringExists = false;
                for (int row = table.GetLength(0) - 1; row >= 0; row--)
                {
                    if (table[row, ind])
                    {
                        var substringLength = row * 2 + 2;
                        ind += substringLength;
                        correctSubstringExists = true;
                        break;
                    }
                }

                if (!correctSubstringExists)
                {
                    bracketsToClose.Add(line[ind]);
                    indexes.Add(ind);
                    ind++;
                }
            }

            bracketsToClose.Reverse();
            var closingBrackets = "";
            foreach (var bracket in bracketsToClose) closingBrackets += _closingBrackets[bracket];
            return (closingBrackets, indexes);
        }

        public ulong CountSecondPartBracketsScore(string brackets)
        {
            var bracketScoreDict = new Dictionary<char, int>
            {
                {')', 1},
                {']', 2},
                {'}', 3},
                {'>', 4}
            };

            ulong score = 0;
            foreach (var bracket in brackets)
            {
                score *= 5;
                score += (ulong)bracketScoreDict[bracket];
            }

            return score;
        }

        public void ReadData(out List<string> lines, string source)
        {
            lines = source.Split("\r\n").ToList();
        }

        public List<(string line, bool[,] table)> FindCorrectSubstringTables(List<string> lines)
        {
            var result = new List<(string, bool[,])>();
            foreach (var line in lines)
            {
                var correctSubstrings = FindCorrectSubstrings(line);
                result.Add((line, correctSubstrings));
            }

            return result;
        }

        private bool[,] FindCorrectSubstrings(string stringToCheck)
        {

            var correctSubstrings = new bool[stringToCheck.Length / 2, stringToCheck.Length];

            // find pairs all correct strings with length=2 () [] {} <> 
            for (int i = 0; i < stringToCheck.Length - 1; i++)
            {
                var currentBracket = stringToCheck[i];
                var nextBracket = stringToCheck[i + 1];

                if (_closingBrackets.ContainsKey(currentBracket))
                {
                    if (_closingBrackets[currentBracket] == nextBracket)
                    {
                        correctSubstrings[0, i] = true;
                    }
                }
            }

            for (int row = 1; row < correctSubstrings.GetLength(0); row++)
            {
                var prevCorrStringLen = row * 2;
                var previousRowIndexes = row - 1;

                for (int index = 0; index < stringToCheck.Length; index++)
                {
                    var currentBracket = stringToCheck[index];

                    // < correctString >
                    if (_closingBrackets.ContainsKey(currentBracket))
                    {
                        if (index + 1 < stringToCheck.Length && correctSubstrings[previousRowIndexes, index + 1] &&
                            index + prevCorrStringLen + 1 < stringToCheck.Length && _closingBrackets[currentBracket] ==
                            stringToCheck[index + prevCorrStringLen + 1])
                        {
                            correctSubstrings[row, index] = true;
                        }
                    }


                    for (int smallerRow = row-1; smallerRow >= 0; smallerRow--)
                    {
                        if (correctSubstrings[smallerRow, index])
                        {
                            var currentStringLen = row * 2 + 2;
                            var checkingRowLen = smallerRow * 2 + 2;

                            var restToFillString = currentStringLen - checkingRowLen;
                            var restToFillRow = (restToFillString - 2) / 2;

                            if (index + checkingRowLen < stringToCheck.Length &&
                                correctSubstrings[restToFillRow, index + checkingRowLen])
                            {
                                correctSubstrings[row, index] = true;
                            }
                        }
                    }

                }
            }

            return correctSubstrings;
        }

        public void DrawBoard(string currentString, bool[,] correctSubstrings)
        {
            Console.WriteLine(new string(' ', 4) + currentString);

            for (int len = 0; len < correctSubstrings.GetLength(0); len++)
            {
                Console.Write("{0,4}", len * 2 + 2);
                for (int i = 0; i < currentString.Length; i++)
                {
                    Console.Write(correctSubstrings[len, i] ? 'X' : '-');
                }
                Console.Write("\n");
            }
        }

    }
}