
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code_2021
{
    public class Day13 : Day
    {
        public const string XFoldCommand = "fold along x=";
        public const string YFoldCommand = "fold along y=";

        public const string data = @"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5";

        public class BoardToFold
        {
            public bool[,] board;
            public int height => board.GetLength(1);
            public int width => board.GetLength(0);

            public BoardToFold(List<(int x, int y)> points)
            {
                var maxX = points.Max(p => p.x);
                var maxY = points.Max(p => p.y);
                board = new bool[maxX + 1, maxY + 1];
                foreach(var (x, y) in points)
                {
                    board[x, y] = true;
                }
            }

            public void DrawBoard()
            {
                Console.WriteLine(new string('=', width + 2));
                for(int y = 0; y < height; y++)
                {
                    Console.Write("|");
                    for(int x = 0; x < width; x++)
                    {
                        Console.Write(board[x, y] ? "#" : "-");
                    }
                    Console.Write("|\n");
                }
            }

            public void Fold(bool FoldByX, int value)
            {
                bool[,] foldedBoard;
                if (FoldByX)
                {
                    foldedBoard = new bool[width, value];

                }
                else // foldByY
                {
                    foldedBoard = new bool[width, value];

                }
            }

        }

        public override void PrintOutput()
        {
            ReadData(data, out var points, out var folds);
            var board = new BoardToFold(points);
            board.DrawBoard();
            //Console.WriteLine($"Day 12 - part 1: {}");
            //Console.WriteLine($"Day 12 - part 2: {}");
        }

        public void ReadData(string source, out List<(int x, int y)> points, out List<(bool xFold, int value)> folds)
        {
            points = new List<(int x, int y)>();
            folds = new List<(bool xFold, int value)>();
            var addingPoints = true;
            var lines = source.Split("\r\n");

            foreach (var t in lines)
            {
                if (t == "")
                {
                    addingPoints = false;
                }
                    else if(addingPoints)
                {
                    var divided = t.Split(',');
                    points.Add((int.Parse(divided[0]), int.Parse(divided[1])));
                }
                else
                {
                    var xFold = t.Contains(XFoldCommand);
                    var value = int.Parse(t.Substring(t.IndexOf('=') + 1));
                    folds.Add((xFold, value));
                }
            }
        }

    }
}