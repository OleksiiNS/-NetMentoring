/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        public static SemaphoreSlim semaphore = new SemaphoreSlim(1);
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            int number = 10;
            var parent = new Thread(()=> Decrement(number));
            parent.Start();
            parent.Join();

            Console.WriteLine();
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");
            semaphore.Wait();
            ThreadPool.QueueUserWorkItem(new WaitCallback(DecrementPool), number);
            semaphore.Release();
            Console.ReadLine();
        }

        static void Decrement(int number)
        {
            if (number > 0)
            {
                var newNumber = number-1;
                Console.WriteLine(newNumber);
                var task = new Thread(()=> Decrement(number-1));
                task.Start();
                task.Join();
            };
        }

        static void DecrementPool(object value)
        {
            int number = (int)value;
            if (number > 0)
            {
                semaphore.Wait();
                var newNumber = number-1;
                Console.WriteLine(newNumber);
                ThreadPool.QueueUserWorkItem(new WaitCallback(DecrementPool), number-1);
                semaphore.Release();
            };
        }
    }
}
