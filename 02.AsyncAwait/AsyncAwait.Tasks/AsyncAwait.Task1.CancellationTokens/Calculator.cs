using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal static class Calculator
{
    public static Task<long> CalculateAsync(int n, CancellationToken token)
    {
        long sum = 0;

        for (var i = 1; i <= n; i++)
        {
            if (token.IsCancellationRequested) break;
            sum += i;
            Thread.Sleep(100);
        }

        return Task.FromResult(sum);
    }
}
