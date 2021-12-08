using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code_2021
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var days = new List<Day>
            {
                new Day1(),
                new Day2(), 
                new Day3(), 
                new Day4(),
                new Day5(),
                new Day6(),
                new Day7(),
            };
            foreach (var day in days)
            {
                Console.WriteLine("=====================================");
                day.PrintOutput();
            }
        }
    }
}