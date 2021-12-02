using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2021
{
    public class Day1 : Day
    {
        public static int CountNumberOfIncreases(List<int> values)
        {
            var numberOfIncreases = 0;
            for (int i = 1; i < values.Count; i++)
            {
                if (values[i - 1] < values[i]) numberOfIncreases++;
            }
            return numberOfIncreases;
        }

        public static int CountNumberOfIncreasesPartTwo(List<int> values)
        {
            LinkedList<int> currentWindow = new LinkedList<int>();
            currentWindow.AddLast(values[0]);
            currentWindow.AddLast(values[1]);
            currentWindow.AddLast(values[2]);

            var numberOfIncreases = 0;
            for (int i = 3; i < values.Count; i++)
            {
                currentWindow.AddLast(values[i]);
                if (currentWindow.Last() > currentWindow.First()) numberOfIncreases++;
                currentWindow.RemoveFirst();
            }
            return numberOfIncreases;
        }

        public override void PrintOutput()
        {
            var data = Properties.Resources.DataDay1.Split('\n');

            var values = new List<int>(data.Length);
            foreach (var line in data)
            {
                values.Add(int.Parse(line));
            }

            Console.WriteLine("Day1 - Part 1: " + CountNumberOfIncreases(values));
            Console.WriteLine("Day1 - Part 2: " + CountNumberOfIncreasesPartTwo(values));
        }
    }
}
