
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code_2021
{
    public class Day13 : Day
    {
        public const string XFoldCommand = "fold along x=";
        public const string YFoldCommand = "fold along y=";

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

            public void DrawBoard(bool drawFold, bool xFold, int whereFold)
            {
                Console.WriteLine(new string('=', width + 2));
                for(int y = 0; y < height; y++)
                {
                    Console.Write("|");
                    for(int x = 0; x < width; x++)
                    {
                        if (!drawFold)
                        {
                            Console.Write(board[x, y] ? "#" : " ");
                        }
                        else
                        {
                            if (xFold && whereFold == x)
                            {
                                ConsoleExtensions.ColorConsoleWrite("|", ConsoleColor.Red);
                            }
                            else if (!xFold && whereFold == y)
                            {
                                ConsoleExtensions.ColorConsoleWrite("-", ConsoleColor.Red);
                            }
                            else
                            {
                                Console.Write(board[x, y] ? "#" : " ");
                            }
                        }
                       
                    }
                    Console.Write("|\n");
                }
                Console.WriteLine(new string('=', width + 2));
            }

            public void Fold(bool foldByX, int whereFold)
            {
                bool[,] foldedBoard;
                if (foldByX)
                {
                    foldedBoard = new bool[whereFold, height];
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            if (x < whereFold) 
                            {
                                foldedBoard[x, y] = board[x, y];
                            }
                            else if(x > whereFold && board[x, y]) 
                            {
                                int d = x - whereFold;
                                foldedBoard[x - 2 * d, y] = true;
                            }
                        }
                    }
                }
                else 
                {
                    foldedBoard = new bool[width, whereFold];
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            if (y < whereFold)
                            {
                                foldedBoard[x, y] = board[x, y];
                            }
                            else if (y > whereFold && board[x, y])
                            {
                                int d = y - whereFold;
                                foldedBoard[x, y - 2 * d] = true;
                            }
                        }
                    }
                }

                board = foldedBoard;
            }

            public int HowManyDots()
            {
                var count = 0;
                foreach (var p in board)
                {
                    if(p) count++;
                }
                return count;
            }

        }

        public override void PrintOutput()
        {

            ReadData(Properties.Resources.DataDay13, out var points, out var folds);
            var boardToFold = new BoardToFold(points);

            boardToFold.Fold(folds[0].xFold, folds[0].whereFold);

            Console.WriteLine($"Day 13 - part 1: {boardToFold.HowManyDots()}");

            for (int i = 1; i < folds.Count; i++)
            {
                var xFold = folds[i].xFold;
                var whereFold = folds[i].whereFold;

                if (boardToFold.width < 100 && boardToFold.height < 100)
                {
                    var text = xFold ? "Fold X = " : "Fold Y = ";
                    Console.WriteLine(text + whereFold);
                    boardToFold.DrawBoard(true, xFold, whereFold);
                }
                boardToFold.Fold(xFold, whereFold);
            }

            boardToFold.DrawBoard(false, true, 0);
        }

        public void ReadData(string source, out List<(int x, int y)> points, out List<(bool xFold, int whereFold)> folds)
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