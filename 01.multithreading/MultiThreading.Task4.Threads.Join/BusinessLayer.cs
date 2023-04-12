using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    internal static class BusinessLayer
    {
        private static readonly SemaphoreSlim Semaphore = new (1);
        public static void RunWithJoin(int startThreadNumber)
        {
            var parent = new Thread(()=> Decrement(startThreadNumber));
            parent.Start();
            parent.Join();
        }

        public static void RunWithSemaphore(int startThreadNumber)
        {
            Semaphore.Wait();
            ThreadPool.QueueUserWorkItem(DecrementPool, startThreadNumber);
            Semaphore.Release();
        }

        private static void Decrement(int number)
        {
            if (number > 0)
            {
                var newNumber = number - 1;
                Console.WriteLine(newNumber);
                var decrementalTask = new Thread(()=> Decrement(newNumber));
                decrementalTask.Start();
                decrementalTask.Join();
            };
        }

        private static void DecrementPool(object value)
        {
            var number = (int)value;
            if (number > 0)
            {
                Semaphore.Wait();
                var newNumber = number-1;
                Console.WriteLine(newNumber);
                ThreadPool.QueueUserWorkItem(DecrementPool, newNumber);
                Semaphore.Release();
            };
        }
    }
}
