using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Advent_of_Code_2021
{
    public class Day5 : Day
    {
        public struct Point
        {
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        public class SurfaceMap
        {
            private List<(Point p1, Point p2)> _lines = new List<(Point start, Point end)> ();
            public int Width { get; }
            public int Height { get; }
            public int[,] Map { get; private set; }
            private bool _shouldCountDiagonalLines;

            public SurfaceMap(List<(Point start, Point end)> lines, bool shouldCountDiagonalLines)
            {
                _lines = new List<(Point start, Point end)>(lines);
                _shouldCountDiagonalLines = shouldCountDiagonalLines;
                Width = _lines.Max(line => Math.Max(line.p1.X, line.p2.X)) + 1;
                Height = _lines.Max(line => Math.Max(line.p1.Y, line.p2.Y)) + 1;
                CountMapToDraw();
            }

            public void CountMapToDraw()
            {
                Map = new int[Width, Height];
                foreach (var line in _lines)
                {
                    // first part consider only horizontal and vertical lines
                    if (line.p1.X == line.p2.X)
                    {
                        var x = line.p1.X;
                        var minY = Math.Min(line.p1.Y, line.p2.Y);
                        var maxY = Math.Max(line.p1.Y, line.p2.Y);
                        for (int y = minY; y <= maxY; y++)
                        {
                            Map[x,y]++;
                        }

                    }
                    else if(line.p1.Y == line.p2.Y)
                    {
                        var y = line.p1.Y;
                        var minX = Math.Min(line.p1.X, line.p2.X);
                        var maxX = Math.Max(line.p1.X, line.p2.X);
                        for (int x = minX; x <= maxX; x++)
                        {
                            Map[x,y]++;
                        }
                    }
                    else if (_shouldCountDiagonalLines)
                    {
                        var minX = Math.Min(line.p1.X, line.p2.X);
                        var maxX = Math.Max(line.p1.X, line.p2.X);

                        var minY = Math.Min(line.p1.Y, line.p2.Y);
                        var maxY = Math.Max(line.p1.Y, line.p2.Y);

                        bool increaseY;
                        if (line.p1.X == minX && line.p1.Y == minY)
                        {
                            increaseY = true;
                        }
                        else if (line.p1.X == minX && line.p1.Y != minY)
                        {
                            increaseY = false;
                        }
                        else if (line.p2.X == minX && line.p2.Y == minY)
                        {
                            increaseY = true;
                        }
                        else
                        {
                            increaseY = false;
                        }

                        var y = increaseY ? minY : maxY;
                        for (int x = minX; x <= maxX; x++)
                        {
                            Map[x, y]++;
                            y += increaseY ? 1 : -1;
                        }
                    }
                }
            }

            public void DrawMap()
            {
                Console.WriteLine(new string('=', Width + 2));
                for (int y = 0; y < Height; y++)
                {
                    Console.Write("|");
                    for(int x = 0; x < Width; x++)
                    {
                        var currentValue = Map[x, y];
                        if (currentValue == 0)
                        {
                            Console.Write('.');
                        }
                        else if(currentValue == 1)
                        {
                            ConsoleExtensions.ColorConsoleWrite(currentValue.ToString(), ConsoleColor.Blue);
                        }
                        else
                        {
                            ConsoleExtensions.ColorConsoleWrite(currentValue.ToString(), ConsoleColor.Red);
                        }
                    }
                    Console.Write("|\n");
                }
                Console.WriteLine(new string('=', Width + 2));
                Console.WriteLine();
            }

            public int GetNumberOfPointsWhereTwoLinesOverlap()
            {
                var pointsCountToReturn = 0;

                for (var y = 0; y < Height; y++)
                {
                    for (var x = 0; x < Width; x++)
                    {
                        if (Map[x, y] > 1)
                            pointsCountToReturn++;
                    }
                }
                return pointsCountToReturn;
            }
        }

        public override void PrintOutput()
        {
            PrintSampleDataOutput();
            PrintProperDataOutput();
        }

        public void PrintSampleDataOutput()
        {
            Console.WriteLine("Day 5 Sample data");

            ReadData(out var sampleLines, Properties.Resources.SampleDataDay5);
            var sampleSurface1 = new SurfaceMap(sampleLines, false);
            Console.WriteLine($"Day5 - part 1 = {sampleSurface1.GetNumberOfPointsWhereTwoLinesOverlap()}");
            sampleSurface1.DrawMap();

            var sampleSurface2 = new SurfaceMap(sampleLines, true);
            Console.WriteLine($"Day5 - part 2 = {sampleSurface2.GetNumberOfPointsWhereTwoLinesOverlap()}");
            sampleSurface2.DrawMap();
        }

        public void PrintProperDataOutput()
        {
            Console.WriteLine("Day 5");

            ReadData(out var lines, Properties.Resources.DataDay5);
            var  surface1 = new SurfaceMap(lines, false);
            Console.WriteLine($"Day5 - part 1 = {surface1.GetNumberOfPointsWhereTwoLinesOverlap()}");

            var surface2 = new SurfaceMap(lines, true);
            Console.WriteLine($"Day5 - part 2 = {surface2.GetNumberOfPointsWhereTwoLinesOverlap()}");
        }

        private static void ReadData(out List<(Point, Point)> lines, string data)
        {
            var textLines = data.Split("\r\n");
            lines = new();

            foreach (var textLine in textLines)
            {
                var coordinates = textLine.Split("->");
                var p1Coordinates = coordinates[0].Split(',');
                var p2Coordinates = coordinates[1].Split(',');

                var p1X = int.Parse(p1Coordinates[0].Trim());
                var p1Y = int.Parse(p1Coordinates[1].Trim());
                var p2X = int.Parse(p2Coordinates[0].Trim());
                var p2Y = int.Parse(p2Coordinates[1].Trim()); 
                lines.Add((new Point(p1X,p1Y), new Point(p2X,p2Y)));
            }
        }
    }
}