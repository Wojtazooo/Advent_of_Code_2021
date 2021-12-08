using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Advent_of_Code_2021
{
    public class Day7 : Day
    {


        public override void PrintOutput()
        {
            var positions = ReadData();

            Console.WriteLine($"Day 7 - part one: {GetResultPartOne(positions)}");
            Console.WriteLine($"Day 7 - part one: {GetResultPartTwo(positions)}");
        }

        public List<int> ReadData()
        {
            return Properties.Resources.DataDay7.Split(',').Select(int.Parse).ToList();
        }

        public double GetMedian(List<int> values)
        {
            var localValues = new List<int>(values);
            localValues.Sort();
            int n = localValues.Count;
            return n % 2 == 1 ? localValues[n / 2] : (localValues[n/2 - 1] + localValues[n/2])/2.0;
        }

        public double GetAverage(List<int> values)
        {
            return values.Sum() / (double)values.Count;
        }

        public int GetResultPartOne(List<int> positions)
        {
            var median = GetMedian(positions);

            if ((int)median == median)
            {
                return Math.Min(
                    CountFuelPartOne(positions, (int) median), 
                    CountFuelPartOne(positions, (int) median + 1)
                    );
            }
            else
            {
                return CountFuelPartOne(positions, (int)median);
            }
        }

        public int GetResultPartTwo(List<int> positions)
        {
            var average = GetAverage(positions);

            if ((int)average == average)
            {
                return CountFuelPartTwo(positions, (int)average);
            }
            else
            {
                return Math.Min(
                    CountFuelPartTwo(positions, (int) average),
                    CountFuelPartTwo(positions, (int) average + 1)
                    );
            }
        }

        public int CountFuelPartOne(List<int> positions, int destination)
        {
            var totalFuel = 0;
            foreach (var position in positions)
            {
                totalFuel += Math.Abs(position - destination);
            }
            return totalFuel;
        }

        public int CountFuelPartTwo(List<int> positions, int destination)
        {
            var totalFuel = 0;
            foreach (var position in positions)
            {
                var distance = Math.Abs(position - destination);
                var fuelCost = GetSumOfArithmeticSequence(1, distance, distance);
                totalFuel += (int)fuelCost;
            }
            return totalFuel;
        }

        public double GetSumOfArithmeticSequence(double firstElement, double lastElement, int count)
        {
            return (firstElement + lastElement) * count / 2.0;
        }

    }
}