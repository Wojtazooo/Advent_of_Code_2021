using System.ComponentModel.Design;
using System.Data;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Advent_of_Code_2021
{
    public class Day9 : Day
    {
        public override void PrintOutput()
        {
            PrintSampleDataResult();
            PrintOfficialDataResult();
        }

        private void PrintSampleDataResult()
        {
            int[,] map;
            bool[,] entered;
            bool[,] isMin;

            ReadData(out map, Properties.Resources.SampleDataDay9);
            Console.WriteLine("SampleData");
            Console.WriteLine($"Day 9 - part one: {GetResultPartOne(map)}");
            Console.WriteLine($"Day 9 - part two: {GetResultPartTwo(map, out entered, out isMin)}");
            DrawBoard(map, entered, isMin);
        }

        private void PrintOfficialDataResult()
        {
            int[,] map;
            bool[,] entered;
            bool[,] isMin;

            ReadData(out map, Properties.Resources.DataDay9);
            Console.WriteLine("OfficialData");
            Console.WriteLine($"Day 9 - part one: {GetResultPartOne(map)}");
            Console.WriteLine($"Day 9 - part two: {GetResultPartTwo(map, out entered, out isMin)}");
            //DrawBoard(map, entered, isMin);
        }

        public void ReadData(out int[,] map, string source)
        {
            var lines = source.Split("\r\n");
            //lines = "2199943210\r\n3987894921\r\n9856789892\r\n8767896789\r\n9899965678".Split("\r\n");

            map = new int[lines.Length, lines[0].Length];

            for (int line = 0; line < lines.Length; line++)
            {
                for (int i = 0; i < lines[line].Length; i++)
                {
                    map[line, i] = (int)char.GetNumericValue(lines[line][i]);
                }
            }
        }

        public static int GetResultPartOne(int[,] map)
        {
            var minValues = new List<int>();


            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    var min = GetMinNeighbors(x, y, map);
                    if (min > map[y, x])
                    {
                        minValues.Add(map[y, x]);
                    }
                }
            }

            return minValues.Sum() + minValues.Count;
        }

        private static int GetMinNeighbors(int x, int y, int[,] map)
        {
            var min = int.MaxValue;

            int rowsCount = map.GetLength(0);
            int columnsCount = map.GetLength(1);

            // Left
            if (x > 0 && map[y, x - 1] < min) min = map[y, x - 1];
            // Right
            if (x + 1 < columnsCount && map[y, x + 1] < min) min = map[y, x + 1];
            // Up
            if (y > 0 && map[y - 1, x] < min) min = map[y - 1, x];
            // Down
            if (y + 1 < rowsCount && map[y + 1, x] < min) min = map[y + 1, x];
            // Left Down
            if (x > 0 && y + 1 < rowsCount && map[y + 1, x - 1] < min) min = map[y + 1, x - 1];
            // Left Up
            if (x > 0 && y > 0 && map[y - 1, x - 1] < min) min = map[y - 1, x - 1];
            // Right Down
            if (x + 1 < columnsCount && y + 1 < rowsCount && map[y + 1, x + 1] < min) min = map[y + 1, x + 1];
            // Right Up
            if (x + 1 < columnsCount && y > 0 && map[y - 1, x + 1] < min) min = map[y - 1, x + 1];

            return min;
        }

        public static int GetResultPartTwo(int[,] map, out bool[,] entered, out bool[,] isMin)
        {
            var basins = new List<int>();

            entered = new bool[map.GetLength(0), map.GetLength(1)];
            isMin = new bool[map.GetLength(0), map.GetLength(1)];

            for (var y = 0; y < map.GetLength(0); y++)
            {
                for (var x = 0; x < map.GetLength(1); x++)
                {
                    var min = GetMinNeighbors(x, y, map);
                    if (min > map[y, x])
                    {
                        isMin[y,x] = true;
                        basins.Add(FindBasinSize(x, y, map, entered));
                    }
                }
            }

            var sorted = basins.OrderByDescending(x => x).Take(3).ToList();
            return sorted[0] * sorted[1] * sorted[2];
        }

        public void DrawBoard(int[,] map, bool[,] entered, bool[,] isMin)
        {
            Console.WriteLine();
            Console.WriteLine(new string('=', map.GetLength(1) + 4));
            for (var y = 0; y < map.GetLength(0); y++)
            {
                Console.Write("||");
                for (var x = 0; x < map.GetLength(1); x++)
                {
                    if(isMin[y, x]) ConsoleExtensions.ColorConsoleWrite(map[y,x].ToString(), ConsoleColor.Red);
                    else if (entered[y, x])
                        ConsoleExtensions.ColorConsoleWrite(map[y, x].ToString(), ConsoleColor.DarkYellow);
                    else ConsoleExtensions.ColorConsoleWrite(map[y, x].ToString(), ConsoleColor.Green);
                }
                Console.Write("||\n");
            }
            Console.WriteLine(new string('=', map.GetLength(1) + 4));
            Console.WriteLine();
        }

        private static int FindBasinSize(int x, int y, int[,] map, bool[,] entered)
        {
            
            MoveRecursive(x, y, map, entered);

            var enteredCount = 0;
            for (var i = 0; i < entered.GetLength(0); i++)
            {
                for (var j = 0; j < entered.GetLength(1); j++)
                {
                    if (entered[i, j]) enteredCount++;
                }
            }
            return enteredCount;
        }

        private static void MoveRecursive(int x, int y, int[,] map, bool[,] entered)
        {
            entered[y, x] = true;

            var rowsCount = map.GetLength(0);
            var columnsCount = map.GetLength(1);

            // Left
            if (x > 0 && !entered[y, x - 1] && map[y, x - 1] < 9) MoveRecursive(x - 1, y, map, entered);
            // Right
            if (x + 1 < columnsCount && !entered[y, x + 1] && map[y, x + 1] < 9) MoveRecursive(x + 1, y, map, entered);
            // Up
            if (y > 0 && !entered[y - 1, x] && map[y - 1, x] < 9) MoveRecursive(x, y - 1, map, entered);
            // Down
            if (y + 1 < rowsCount && !entered[y + 1, x] && map[y + 1, x] < 9) MoveRecursive(x, y + 1, map, entered);
        }
    }
}