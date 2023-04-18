/*
 * 3. Write a program, which multiplies two matrices and uses class Parallel.
 * a. Implement logic of MatricesMultiplierParallel.cs
 *    Make sure that all the tests within MultiThreading.Task3.MatrixMultiplier.Tests.csproj run successfully.
 * b. Create a test inside MultiThreading.Task3.MatrixMultiplier.Tests.csproj to check which multiplier runs faster.
 *    Find out the size which makes parallel multiplication more effective than the regular one.
 */

using System;
using System.Diagnostics;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("3.	Write a program, which multiplies two matrices and uses class Parallel. ");
            Console.WriteLine();
            byte sizeOfMatrix = 0;
            do
            {
                sizeOfMatrix++;
                if (CreateAndProcessMatrices(sizeOfMatrix))
                {
                    Console.WriteLine($"ParallelMultiplier is faster then Multiplier when size of Matrix = {sizeOfMatrix}");
                    break;
                }
            } while (sizeOfMatrix<byte.MaxValue);

            Console.ReadLine();
        }

        private static bool CreateAndProcessMatrices(byte sizeOfMatrix)
        {
            Console.WriteLine($"Multiplying Matrix with size = {sizeOfMatrix}");

            var firstMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix, true);
            var secondMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix, true);
            var resultMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix);
            var resultParallelMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix);

            Console.WriteLine("firstMatrix:");
            firstMatrix.Print();
            Console.WriteLine("secondMatrix:");
            secondMatrix.Print();

            Stopwatch stopWatch = new();
            stopWatch.Start();
            new MatricesMultiplierParallel().Multiply(firstMatrix, secondMatrix, resultParallelMatrix);            
            stopWatch.Stop();
            var parallelMultiplierTime = stopWatch.ElapsedMilliseconds;
            Console.WriteLine($"Time Taken to ParallelMultiplier in milliseconds {parallelMultiplierTime}");
            Console.WriteLine("resultParallelMatrix:");
            resultParallelMatrix.Print();

            stopWatch.Restart();
            new MatricesMultiplier().Multiply(firstMatrix, secondMatrix, resultMatrix);
            stopWatch.Stop();
            var seqMultiplierTime = stopWatch.ElapsedMilliseconds;
            Console.WriteLine($"Time Taken to Multiplier in milliseconds {seqMultiplierTime}");
            Console.WriteLine("resultMatrix:");
            resultMatrix.Print();

            return parallelMultiplierTime < seqMultiplierTime;
        }
    }
}
