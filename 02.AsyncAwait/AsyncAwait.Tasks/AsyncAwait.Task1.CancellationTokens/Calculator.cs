using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal static class Calculator
{
    public static async Task<long> CalculateAsync(int iterationCount, CancellationToken token)
    {
        long sum = 0;

        for (var i = 1; i <= iterationCount; i++)
        {
            token.ThrowIfCancellationRequested();
            sum += i;
            
            await Task.Delay(100, token);
        }

        return await Task.FromResult(sum);
    }
}
