using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

class Program
{
    static void RunPerformanceTest(List<int> data, int size)
    {
        Console.WriteLine($"Size: {size}");

        var sw = Stopwatch.StartNew();
        var linqResult = data.Where(IsPrime).Select(x => Math.Sqrt(x)).ToList();
        sw.Stop();
        Console.WriteLine($"LINQ: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        var plinqResult = data.AsParallel().Where(IsPrime).Select(x => Math.Sqrt(x)).ToList();
        sw.Stop();
        Console.WriteLine($"PLINQ: {sw.ElapsedMilliseconds} ms\n");
    }

    static void RunThreadSafetyTest(List<int> data)
    {
        Console.WriteLine("Thread Safety Test:");
        
        int expectedCount = data.Count(x => x % 2 == 0);
        Console.WriteLine($"Expected: {expectedCount}");

        int unsafeCount = 0;
        data.AsParallel().ForAll(x =>
        {
            if (x % 2 == 0)
            {
                unsafeCount++;
            }
        });
        Console.WriteLine($"Unsafe count: {unsafeCount}");

        int safeCountLock = 0;
        object syncObj = new object();
        data.AsParallel().ForAll(x =>
        {
            if (x % 2 == 0)
            {
                lock (syncObj)
                {
                    safeCountLock++;
                }
            }
        });
        Console.WriteLine($"Safe count (lock): {safeCountLock}");

        int safeCountInterlocked = 0;
        data.AsParallel().ForAll(x =>
        {
            if (x % 2 == 0)
            {
                Interlocked.Increment(ref safeCountInterlocked);
            }
        });
        Console.WriteLine($"Safe count (Interlocked): {safeCountInterlocked}");
    }

    static bool IsPrime(int number)
    {
        if (number < 2) return false;
        for (int i = 2; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0) return false;
        }
        return true;
    }

    static void Main()
    {
        var sizes = new[] { 1_000_000, 5_000_000, 10_000_000 };
        var rand = new Random(42);

        foreach (var size in sizes)
        {
            var data = Enumerable.Range(0, size).Select(_ => rand.Next(1, 100000)).ToList();
            RunPerformanceTest(data, size);
        }

        var testData = Enumerable.Range(0, 5_000_000).Select(_ => rand.Next(1, 100)).ToList();
        RunThreadSafetyTest(testData);
    }
}
