/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Programs
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();
            var source = new CancellationTokenSource();
            var token = source.Token;
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            var task = Task.Run(()=> {
                var date = DateTime.Now; 
                Console.WriteLine($"Parent task. Date is {date.ToLongDateString()}");
            }, token);

            task.ContinueWith( _ => { 
                Console.WriteLine("Continuation task should be executed regardless of the result of the parent task."); 
                }, TaskContinuationOptions.None);
            task.ContinueWith( _ =>{ 
                Console.WriteLine("Continuation task should be executed when the parent task finished without success.");
                }, TaskContinuationOptions.OnlyOnFaulted);
            task.ContinueWith((_, _) => { 
                Console.WriteLine("Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation."); 
                }, null, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            task.ContinueWith( (_, _) =>{ 
                Console.WriteLine("Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
                }, null,token, TaskContinuationOptions.OnlyOnCanceled, TaskScheduler.FromCurrentSynchronizationContext());
            
            task.Wait(token);
            Console.ReadLine();
        }
    }
}
