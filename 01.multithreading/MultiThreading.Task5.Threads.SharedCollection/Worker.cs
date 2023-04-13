using System;
using System.Collections.Generic;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    internal class Worker
    {
        private static readonly List<int> Collection = new ();
        private static readonly Mutex Locker = new ();
        private static int _printed;
        private const int MaxLength = 10;
        private static int GetRandom => new Random().Next(1, 100);

        public static void Print()
        {
            do
            {
                if(_printed < Collection.Count)
                { 
                    Locker.WaitOne();
                    foreach (var item in Collection)
                    {
                        Console.Write(item + " ");
                    }
                    Console.WriteLine();
                    _printed = Collection.Count;
                    Locker.ReleaseMutex();
                }
            }
            while (_printed < MaxLength);
        }

        public static void Add()
        {
            for(var i = 0; i < MaxLength; i++)
            {
                Locker.WaitOne();
                Collection.Add(GetRandom);
                Locker.ReleaseMutex();
            };
        }
    }
}
