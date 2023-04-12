using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    internal static class BusinessLayer
    {
        private const int MaxLength = 10;
        private static int GetRandom => new Random().Next(1, 100);
        public static int GetTaskResult()
        {
            var task = Task.Run(() =>
            {
                Console.WriteLine("First Task – creates an array of 10 random integer.");
                var intArray= new int[MaxLength];
                for (var i = 0; i < MaxLength; i++)
                {
                    var rand = GetRandom;
                    Console.WriteLine($"item {i} = {rand}");
                    intArray[i]=rand;
                }
                return intArray;
            }).ContinueWith((prevResult) =>
            {
                Console.WriteLine("Second Task – multiplies this array with another random integer.");
                var multipliedArray = new int[MaxLength];
                var i = 0;
                foreach (var item in prevResult.Result)
                {
                    var multiplier= GetRandom;
                    var result = item * multiplier;
                    Console.WriteLine($"{result} = {item} * {multiplier}");
                    multipliedArray[i]=result;
                    i++;
                }
                return multipliedArray;
            }).ContinueWith((prevResult) =>
            {
                Console.WriteLine("Third Task – sorts this array by ascending.");
                var orderedArray = prevResult.Result.OrderBy(x => x).ToArray();
                foreach (var item in orderedArray)
                {
                    Console.WriteLine($"{item}");
                }
                return orderedArray;
            }).ContinueWith((prevResult) =>
            {
                Console.WriteLine("Fourth Task – calculates the average value.");
                var average = prevResult.Result.Sum()/MaxLength;
                Console.WriteLine($"Average = {average}");
                return average;
            });

            return task.Result;
        } 
    }
}
