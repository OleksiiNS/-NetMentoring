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

            for(byte b=2; b<byte.MaxValue; b++)
            { 
                CreateAndProcessMatrices(b);
            }
            Console.ReadLine();
        }

        private static void CreateAndProcessMatrices(byte sizeOfMatrix)
        {
            //Console.WriteLine("Multiplying...");
            var firstMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix, true);
            var secondMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix, true);
            var resultMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix);
            var resultParallelMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix);

            //Console.WriteLine("firstMatrix:");
            //firstMatrix.Print();
            //Console.WriteLine("secondMatrix:");
            //secondMatrix.Print();

            Stopwatch stopWatch = new();
            stopWatch.Start();
            new MatricesMultiplier().Multiply(firstMatrix, secondMatrix, resultMatrix);
            stopWatch.Stop();
            var seqElapsedMilliseconds = stopWatch.ElapsedMilliseconds;
            //Console.WriteLine($"Time Taken to Multipy in miliseconds {stopWatch.ElapsedMilliseconds}");
            //Console.WriteLine("resultMatrix:");
            //resultMatrix.Print();

            stopWatch.Restart();
            new MatricesMultiplierParallel().Multiply(firstMatrix, secondMatrix, resultParallelMatrix);
            stopWatch.Stop();
            var parallelElapsedMilliseconds = stopWatch.ElapsedMilliseconds;
            Console.WriteLine($"Sequential loop time in milliseconds {seqElapsedMilliseconds} vs Parallel loop time {parallelElapsedMilliseconds} for Matrix size {sizeOfMatrix}");

            //Console.WriteLine($"Time Taken to ParallelMultipy in miliseconds {stopWatch.ElapsedMilliseconds}");
            //Console.WriteLine("resultParallelMatrix:");
            //resultParallelMatrix.Print();
        }
    }
}
