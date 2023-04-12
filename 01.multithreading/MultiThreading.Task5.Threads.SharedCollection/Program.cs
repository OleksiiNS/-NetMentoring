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
        private static readonly List<int> Collection = new ();
        private static readonly Mutex Locker = new ();
        private static int _printed;
        private static int GetRandom => new Random().Next(1, 100);

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var taskPrint = Task.Factory.StartNew(Print);
            var taskAdd = Task.Factory.StartNew(Add);
            
            Task.WaitAll(taskAdd, taskPrint);
            
        }

        private static void Print()
        {
            do
            {
                if(_printed < Collection.Count)
                { 
                    Locker.WaitOne();
                    foreach (var item in Collection)
                    {
                       Console.Write(item+" ");
                    }
                    Console.WriteLine();
                    _printed = Collection.Count;
                    Locker.ReleaseMutex();
                }
            }
            while (_printed < 10);
        }

        private static void Add()
        {
            for (var i=0; i<10; i++)
            {
               Locker.WaitOne();
                Collection.Add(GetRandom);
                Locker.ReleaseMutex();
            };
        }
    }
}
