using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    internal static class Worker
    {
        private const int MaxLength = 10;
        private static int GetRandom => new Random().Next(1, 100);
        public static int GetTaskResult()
        {
            var task = Task.Run(Creator)
                .ContinueWith(Multiplier)
                .ContinueWith(Orderer)
                .ContinueWith(Average);

            return task.Result;
        }

        private static int Average(Task<int[]> prevResult)
        {
            Console.WriteLine("Fourth Task – calculates the average value.");
            var average = prevResult.Result.Sum() / MaxLength;
            Console.WriteLine($"Average = {average}");
            return average;
        }

        private static int[] Orderer(Task<int[]> prevResult)
        {
            Console.WriteLine("Third Task – sorts this array by ascending.");
            var orderedArray = prevResult.Result.OrderBy(x => x).ToArray();
            foreach (var item in orderedArray)
            {
                Console.WriteLine($"{item}");
            }

            return orderedArray;
        }

        private static int[] Multiplier(Task<int[]> prevResult)
        {
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            var multipliedArray = new int[MaxLength];
            for(var i=0; i<MaxLength; i++)
            {
                var multiplier = GetRandom;
                var result = prevResult.Result[i] * multiplier;
                Console.WriteLine($"{result} = {prevResult.Result[i]} * {multiplier}");
                multipliedArray[i] = result;
            }

            return multipliedArray;
        }

        private static int[] Creator()
        {
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            var integers = new int[MaxLength];
            for (var i = 0; i < MaxLength; i++)
            {
                var rand = GetRandom;
                Console.WriteLine($"item {i} = {rand}");
                integers[i] = rand;
            }

            return integers;
        }
    }
}
