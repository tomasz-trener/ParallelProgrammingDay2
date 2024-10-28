
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

public class Example1
{


    [Benchmark]
    public void SquareEachValue()
    {
        const int count = 100000;

        var values = Enumerable.Range(0, count);
        var results = new int[count];

        Parallel.ForEach(values, (value, loopState, index) =>
        {
            results[index] = (int)Math.Pow(value, 2);
        });
    }

    [Benchmark]
    public void SquareEachValueChunked()
    {
        const int count = 100000;

        var values = Enumerable.Range(0, count);
        var results = new int[count];

        var part = Partitioner.Create(0, count, 10000);

        Parallel.ForEach(part, range =>
        {
            for (int i = range.Item1; i < range.Item2; i++)
            {
                results[i] = (int)Math.Pow(i, 2);
            }
        });
    }



    static void Main()
    {
        var summary = BenchmarkRunner.Run<Example1>();
        Console.WriteLine(summary);

    }
}


