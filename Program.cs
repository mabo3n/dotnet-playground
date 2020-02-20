using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DotNetPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            new Program().Run();
        }

        public void Run()
        {
            var frequenciesIndex = new Dictionary<string, long>();
            var uniqueWordCount = 3_893_941; // ~350MB

            var random = new Random();
            var timer = Stopwatch.StartNew();
            for (var i = 0; i < uniqueWordCount; i++)
            {
                var @string = i.ToString();
                var randomOffset = LongRandom(10, long.MaxValue, random);

                frequenciesIndex[@string] = randomOffset;
            }

            Console.WriteLine($"Index loaded in: {timer.Elapsed}");

            string input;
            while ((input = Console.ReadLine()) != "tchau")
            {
                Console.WriteLine($"Offset for word '{input}' is {frequenciesIndex[input]}");
            }
        }

        long LongRandom(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }
    }
}
