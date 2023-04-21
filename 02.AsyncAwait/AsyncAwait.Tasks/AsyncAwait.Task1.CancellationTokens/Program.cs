/*
* Study the code of this application to calculate the sum of integers from 0 to N, and then
* change the application code so that the following requirements are met:
* 1. The calculation must be performed asynchronously.
* 2. N is set by the user from the console. The user has the right to make a new boundary in the calculation process,
* which should lead to the restart of the calculation.
* 3. When restarting the calculation, the application should continue working without any failures.
*/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal class Program
{
    private static Task _calculator;
    /// <summary>
    /// The Main method should not be changed at all.
    /// </summary>
    /// <param name="args"></param>
    private static void Main(string[] args)
    {
        Console.WriteLine("Mentoring program L2. Async/await.V1. Task 1");
        Console.WriteLine("Calculating the sum of integers from 0 to N.");
        Console.WriteLine("Use 'q' key to exit...");
        Console.WriteLine();

        Console.WriteLine("Enter N: ");
        var cancelTokenSource = new CancellationTokenSource();

        var input = Console.ReadLine();
        while (input != null && input.Trim().ToUpper() != "Q")
        {
            if (int.TryParse(input, out var n))
            {
                if (_calculator == null || _calculator.IsCompleted)
                {
                    var cts = cancelTokenSource;
                    _calculator = new Task(() => CalculateSumAsync(n, cts.Token));
                }
                else
                {
                    cancelTokenSource.Cancel();
                    cancelTokenSource.Dispose();
                    cancelTokenSource = new CancellationTokenSource();
                    var cts = cancelTokenSource;
                    _calculator = new Task(() => CalculateSumAsync(n, cts.Token));
                }
                _calculator.Start();
            }
            else
            {
                Console.WriteLine($"Invalid integer: '{input}'. Please try again.");
                Console.WriteLine("Enter N: ");
            }

            input = Console.ReadLine();
        }

        Console.WriteLine("Press any key to continue");
        Console.ReadLine();
        cancelTokenSource.Dispose();
    }

    private static async void CalculateSumAsync(int n, CancellationToken token)
    {
        token.Register(() => Console.WriteLine($"Sum for {n} cancelled..."));
        Console.WriteLine($"The task for {n} started... Enter N to cancel the request:");
        var sum = await Calculator.CalculateAsync(n, token);

        if (token.IsCancellationRequested) return;

        Console.WriteLine($"Sum for {n} = {sum}.");
        Console.WriteLine();
        Console.WriteLine("Enter N: ");
    }
}
