
public class Example1
{


    static void Main()
    {
       
        var numbers = Enumerable.Range(1, 100).ToArray();

        var results = numbers.AsParallel()
            .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
            .Select(x =>
            {
                var result = Math.Pow(x, 2);
                Thread.Sleep(1000);
                Console.WriteLine("P");
                return result;
            });

        foreach (var item in results)
        {
            Console.WriteLine($"{item} C");
        }




    }
}

