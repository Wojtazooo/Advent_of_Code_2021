namespace Advent_of_Code_2021
{
    public class Day3 : Day
    {
        public override void PrintOutput()
        {
            var (numbers, maxLength) = GetListOfBinaryNumbers();

            Console.WriteLine($@"Day3 - part 1: {GetResultOfFirstPart(numbers, maxLength)}");
            Console.WriteLine($@"Day3 - part 2: {GetResultOfPartTwo(numbers, maxLength)}");
        }

        private static (List<int> numbers, int maxLength) GetListOfBinaryNumbers()
        {
            var binaryNumbers = Properties.Resources.DataDay3.Split("\r\n");

            var maxLength = binaryNumbers.Max(x => x.Length);
            var numbers = binaryNumbers.Select(number => Convert.ToInt32(number, 2)).ToList();

            return (numbers, maxLength);
        }

        private static int GetResultOfFirstPart(List<int> binaryNumbers, int maxLength)
        {
            var gammaRate = 0;

            for (var i = 0; i < maxLength; i++)
            {
                var numberOfPositiveBits = 0;
                int mask = 1 << i;
                foreach (var number in binaryNumbers)
                {
                    var bit = (number & mask) != 0;
                    if (bit) numberOfPositiveBits++;
                }

                if (numberOfPositiveBits > binaryNumbers.Count / 2)
                {
                    gammaRate ^= mask;
                }
            }

            return gammaRate * GetEpsilonFromGamma(gammaRate, maxLength);
        }

        private static int GetEpsilonFromGamma(int gammaRate, int maxLength)
        {
            var mask = 0;
            for (var i = 0; i < maxLength; i++)
            {
                mask *= 2;
                mask += 1;
            }

            return (~gammaRate & mask);
        }

        private static int GetResultOfPartTwo(List<int> numbers, int maxLength)
        {
            var mostCommonBits = new List<int>(numbers);
            var leastCommonBits = new List<int>(numbers);

            var mask = 1 << (maxLength - 1);
            while ((mostCommonBits.Count > 1 || leastCommonBits.Count > 1) && mask > 0)
            {
                if (mostCommonBits.Count > 1)
                {
                    mostCommonBits = CheckIfMostCommonBitIsPositive(mostCommonBits, mask)
                        ? mostCommonBits.FindAll(x => (x & mask) != 0)
                        : mostCommonBits.FindAll(x => (x & mask) == 0);
                }

                if (leastCommonBits.Count > 1)
                {
                    leastCommonBits = CheckIfMostCommonBitIsPositive(leastCommonBits, mask)
                        ? leastCommonBits.FindAll(y => (y & mask) == 0)
                        : leastCommonBits.FindAll(y => (y & mask) != 0);
                }

                mask >>= 1;
            }

            return mostCommonBits[0] * leastCommonBits[0];
        }

        private static bool CheckIfMostCommonBitIsPositive(List<int> numbers, int mask)
        {
            var positiveBitsNumber = numbers.Count(number => (number & mask) != 0);
            return positiveBitsNumber >= numbers.Count - positiveBitsNumber;
        }
    }
}