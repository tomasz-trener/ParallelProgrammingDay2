
public class Example1
{


    static void Main1()
    {

        var numbers = Enumerable.Range(1, 100).ToArray();

        var cts = new CancellationTokenSource();

        var results = numbers.AsParallel()
            .WithMergeOptions(ParallelMergeOptions.NotBuffered)
            .WithCancellation(cts.Token)
            .Select(x =>
            {
                if(cts.Token.IsCancellationRequested)
                {
                    //  Console.WriteLine("Cancellation requested");
                    //  cts.Token.ThrowIfCancellationRequested();
                    return 0;
                }



                var result = Math.Pow(x, 2);
                Thread.Sleep(1000);
                return result;
            });


        try
        {
            foreach (var item in results)
            {
                if (item > 50)
                    cts.Cancel();

                Console.WriteLine($"{item} C");
            }

        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("cancelled");
        }
        



    }
}



public class Example2
{


    static void Main()
    {

        var numbers = Enumerable.Range(1, 100).ToArray();

        var cts = new CancellationTokenSource();

        var results = numbers.AsParallel()
            .WithMergeOptions(ParallelMergeOptions.NotBuffered)
            .WithCancellation(cts.Token)
            .Select(x =>
            {
                if (cts.Token.IsCancellationRequested)
                {
                    //  Console.WriteLine("Cancellation requested");
                    //  cts.Token.ThrowIfCancellationRequested();
                    return 0;
                }

                if (x == 50)
                    throw new Exception("50 is not allowed");

                var result = Math.Pow(x, 2);
                Thread.Sleep(1000);
                return result;
            });


        try
        {
            foreach (var item in results)
            {
               // if (item > 50)
                //    cts.Cancel();

                Console.WriteLine($"{item} C");
            }

        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("cancelled");
        }
        catch(AggregateException ex)
        {
            ex.Handle(e =>
            {
                Console.WriteLine("error");
                return true;
            });
        }



    }
}

