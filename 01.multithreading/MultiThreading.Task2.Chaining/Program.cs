/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();
            Random r = new Random();

            Task<int> task = Task.Run(() =>
            {
                Console.WriteLine("First Task – creates an array of 10 random integer.");
                var result= new List<int>(10);
                for (int i = 0; i < 10; i++)
                {
                    var rand = r.Next(100);
                    Console.WriteLine($"item {i} = {rand}");
                    result.Add(rand);
                }
                return result;
            }).ContinueWith((prevResult) =>
            {
                Console.WriteLine("Second Task – multiplies this array with another random integer.");
                var result= new List<int>(10);
                foreach (var item in prevResult.Result)
                {
                    var m=r.Next(100);
                    var mult = item * m;
                    Console.WriteLine($"{mult} = {item} * {m}");
                    result.Add(mult);
                }
                return result;
            }).ContinueWith((prevResult) =>
            {
                Console.WriteLine("Third Task – sorts this array by ascending.");
                var result = prevResult.Result.OrderBy(x => x).ToList();
                foreach (var item in result)
                {
                    Console.WriteLine($"{item}");
                }
                return result;
            }).ContinueWith((prevResult) =>
            {
                Console.WriteLine("Fourth Task – calculates the average value.");
                var result = prevResult.Result.Sum()/10;
                Console.WriteLine($"Average = {result}");
                return result;
            });

            task.Wait();
            Console.WriteLine("=================================");
            Console.WriteLine($"Final result = {task.Result}");

            Console.ReadLine();
        }
    }
}
