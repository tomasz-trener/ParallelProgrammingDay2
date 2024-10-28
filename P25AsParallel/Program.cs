
using System.Collections.Concurrent;

public class Example1
{


    static void Main1()
    {
        const int count = 50;
        var items = Enumerable.Range(1, count).ToArray();

        //var results = new int[count];
        var results = new ConcurrentDictionary<int,int>();


        items.AsParallel().ForAll(i =>
        {
            int newValue = i * i;
            Console.WriteLine($"{newValue} ({Task.CurrentId})\t");
            results[i-1] = newValue;
        });


        foreach (var i in results)
        {
            Console.WriteLine($"{i}\t");
        }



    }
}



public class Example2
{


    static void Main()
    {
        const int count = 50;
        var items = Enumerable.Range(1, count).ToArray();

        
        var cubes = items.AsParallel().AsOrdered().Select(i => i * i *i);


        foreach (var i in cubes)
        {
            Console.WriteLine($"{i}\t");
        }


    }
}


