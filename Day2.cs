using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2021
{
    public class Day2 : Day
    {
        enum Direction
        {
            up,
            down,
            forward,
        }

        public override void PrintOutput()
        {
            var moves = GetListOfMoves();

            Console.WriteLine($"Day2 - part 1: {GetResultOfFirstPart(moves)}");
            Console.WriteLine($"Day2 - part 2: {GetResultOfSecondPart(moves)}");
        }

        private static List<(Direction, int)> GetListOfMoves()
        {
            var moves = new List<(Direction, int)>();
            var data = Properties.Resources.DataDay2.Split('\n');
            foreach(var line in data)
            {
                var values = line.Split(" ");
                if (values.Length < 2) continue;
                var direction = Enum.Parse<Direction>(values[0]);
                var distance = int.Parse(values[1]);
                moves.Add((direction, distance));
            }
            return moves;
        }

        private static int GetResultOfFirstPart(List<(Direction direction, int distance)> moves)
        {
            var dx = 0;
            var dy = 0;

            foreach(var move in moves)
            {
                switch (move.direction)
                {
                    case Direction.up:
                        dy += move.distance;
                        break;
                    case Direction.down:
                        dy -= move.distance;
                        break;
                    case Direction.forward:
                        dx += move.distance;
                        break;
                    default:
                        break;
                }
            }
            return Math.Abs(dx * dy);
        }

        private static int GetResultOfSecondPart(List<(Direction direction, int value)> moves)
        {
            var horizontalPosition = 0;
            var depth = 0;
            var currentAim = 0;

            foreach (var move in moves)
            {
                switch (move.direction)
                {
                    case Direction.up:
                        currentAim -= move.value;
                        break;
                    case Direction.down:
                        currentAim += move.value;
                        break;
                    case Direction.forward:
                        horizontalPosition += move.value;
                        depth += currentAim * move.value; 
                        break;
                    default:
                        break;
                }
            }
            
            return horizontalPosition * depth;
        }
    }
}
