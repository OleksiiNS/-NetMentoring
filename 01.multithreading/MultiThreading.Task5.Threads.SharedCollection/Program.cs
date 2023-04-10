/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static List<int> _collection = new List<int>();
        private static Mutex _locker = new Mutex();
        private static int _printed = 0;
        private static Random _random = new Random();
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var taskPrint = Task.Factory.StartNew(() => Print());
            var taskAdd = Task.Factory.StartNew(() => Add());
            
            Task.WaitAll(taskAdd, taskPrint);
            
        }

        private static void Print()
        {
            do
            {
                if(_printed<_collection.Count)
                { 
                    _locker.WaitOne();
                    foreach (var item in _collection)
                    {
                       Console.Write(item+" ");
                    }
                    Console.WriteLine();
                    _printed = _collection.Count;
                    _locker.ReleaseMutex();
                }
            }
            while (_printed < 10);
        }

        static void Add()
        {
            for (int i=0; i<10; i++)
            {
                var r = _random.Next(100);
                
                _locker.WaitOne();
                _collection.Add(r);
                _locker.ReleaseMutex();
            };
        }
    }
}
