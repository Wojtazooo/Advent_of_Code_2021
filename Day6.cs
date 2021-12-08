using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Advent_of_Code_2021
{
    public class Day6 : Day
    {
        public const int MaxDayValueForLanternfish = 8;
        public const int TimerForNewFish = 8;
        public const int OldFishTimerAfterBirth = 6;

        public class GroupOfLanternFishes
        {
            private long[] _groupCounts = new long[MaxDayValueForLanternfish + 1];
            public GroupOfLanternFishes(List<int> initial)
            {
                foreach (var fishTimer in initial)
                {
                    _groupCounts[fishTimer]++;
                }
            }

            public void NextDay()
            {
                long fishesToAdd = _groupCounts[0];

                for (int i = 1; i < MaxDayValueForLanternfish + 1; i++)
                {
                    _groupCounts[i - 1] = _groupCounts[i];
                }

                _groupCounts[TimerForNewFish] = fishesToAdd;
                _groupCounts[OldFishTimerAfterBirth] += fishesToAdd;
            }

            public long GetTotalCount()
            {
                return _groupCounts.ToList().Sum();
            }
        }

        public override void PrintOutput()
        {
            var initialValues = ReadData();

            var days = 80;
            Console.WriteLine($"Day 6 - Part 1: {Simulate(days, initialValues)}");
            days = 256;
            Console.WriteLine($"Day 6 - Part 1: {Simulate(days, initialValues)}");
        }

        public List<int> ReadData()
        {
            return Properties.Resources.DataDay6.Split(',').ToList().Select(int.Parse).ToList();
        }

        public long Simulate(int days, List<int> initialFishes)
        {
            var groupOfFishes = new GroupOfLanternFishes(initialFishes);
            for (int d = 0; d < days; d++)
            {
                groupOfFishes.NextDay();
            }

            return groupOfFishes.GetTotalCount();
        }
    }
}