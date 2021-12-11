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
    public class Day11 : Day
    {
        public class DumboOctopusesBoard
        {
            public int[,] Board;
            public int flashes = 0;

            public DumboOctopusesBoard(int[,] board)
            {
                Board = board;
            }

            public void DrawBoard()
            {
                Console.Write(' ');
                Console.Write(new string('=',Board.GetLength(0)+2));
                Console.Write('\n');
                for (int i = 0; i < Board.GetLength(0); i++)
                {
                    Console.Write(" |");
                    for (int j = 0; j < Board.GetLength(1); j++)
                    {
                        if (Board[i, j] == 0)
                        {
                            ConsoleExtensions.ColorConsoleWrite(Board[i, j].ToString(), ConsoleColor.Red);
                        }
                        else
                        {
                            Console.Write(Board[i,j]);   
                        }
                    }
                    Console.Write("|\n");
                }
                Console.Write(' ');
                Console.WriteLine(new string('=', Board.GetLength(0) + 2));
            }

            public void PrintResultForPartOneAndTwo(int n)
            {
                var dayWhenAllFlashed = int.MaxValue;
                var day = 0;
                while (dayWhenAllFlashed == int.MaxValue)
                {
                    day++;
                    var allFlashed = NextDay();
                    if (day == n)
                    {
                        Console.WriteLine($"Result after {n} days");
                        DrawBoard();
                        Console.WriteLine($"Flashes = {flashes}");
                    }

                    if (allFlashed)
                    {
                        dayWhenAllFlashed = day;
                        Console.WriteLine($"Result after {day} days"); 
                        DrawBoard();
                    }
                }
            }

            public bool NextDay()
            {
                var considered = new bool[Board.GetLength(0), Board.GetLength(1)];
                var flashesFrom = new Stack<(int x, int y)>();

                // increase all energy levels by 1
                for (int i = 0; i < Board.GetLength(0); i++)
                {
                    for (int j = 0; j < Board.GetLength(1); j++)
                    {
                        Board[i, j]++;
                        if (Board[i, j] > 9)
                        {
                            Board[i, j] = 0;
                            flashes++;
                            flashesFrom.Push((i, j));
                            considered[i, j] = true;
                        }
                    }
                }

                while (flashesFrom.Count > 0)
                {
                    var currentPos = flashesFrom.Pop();

                    var x = currentPos.x;
                    var y = currentPos.y;

                    CheckField(x - 1, y - 1, flashesFrom, considered);
                    CheckField(x, y - 1, flashesFrom, considered);
                    CheckField(x + 1, y - 1, flashesFrom, considered);
                    CheckField(x - 1, y, flashesFrom, considered);
                    CheckField(x + 1, y, flashesFrom, considered);
                    CheckField(x - 1, y + 1, flashesFrom, considered);
                    CheckField(x, y + 1, flashesFrom, considered);
                    CheckField(x + 1, y + 1, flashesFrom, considered);
                }

                return CheckIfAllFlashed(considered);
            }

            public void CheckField(int x, int y, Stack<(int x, int y)> flashesFrom, bool[,] considered)
            {
                if (x < 0 || y < 0 || x >= considered.GetLength(0) || y >= considered.GetLength(1)) return;

                if(Board[x,y] != 0) Board[x, y]++;
                if (Board[x, y] > 9 && !considered[x, y])
                {
                    considered[x, y] = true;
                    Board[x, y] = 0;
                    flashes++;
                    flashesFrom.Push((x, y));
                }
            }

            public bool CheckIfAllFlashed(bool[,] considered)
            {
                for (int i = 0; i < considered.GetLength(0); i++)
                {
                    for (int j = 0; j < considered.GetLength(1); j++)
                    {
                        if (!considered[i, j]) return false;
                    }
                }
                return true;
            }

        }

        public override void PrintOutput()
        {
            ReadData(out var board, Properties.Resources.DataDay11);
            var dumboOctopusesBoard = new DumboOctopusesBoard(board);
            dumboOctopusesBoard.PrintResultForPartOneAndTwo(100);
        }

        public void ReadData(out int[,] board, string source)
        {
            var lines = source.Split("\r\n");
            var rows = lines.Length;
            var cols = lines[0].Length;

            board = new int[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    board[row, col] = (int)char.GetNumericValue(lines[row][col]);
                }
            }
        }

    }
}