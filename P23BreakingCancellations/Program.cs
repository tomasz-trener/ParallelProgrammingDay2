class Example1
{

 
    static void Main1()
    {

        var cts = new CancellationTokenSource();

        var po = new ParallelOptions();
        po.CancellationToken = cts.Token;

        var random = new Random();

        try
        {
            Parallel.For(0, 200, po, (i, state) =>
            {
                Thread.Sleep(random.Next(1000));


                if (po.CancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Cancellation requested");
                    return;
                }
                if (i == 100)
                {
                    cts.Cancel();
                    Console.WriteLine("Cancelled");
                }


                Console.WriteLine($"{i} [{Task.CurrentId}] ");

            });

        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Cancelled by token");
        }

        Console.ReadKey();

    }
}


class Example2



{


    static void Main2()
    {

        var cts = new CancellationTokenSource();

        var po = new ParallelOptions();
        po.CancellationToken = cts.Token;

        var random = new Random();

        try
        {
            Parallel.For(0, 200, po, (i, state) =>
            {
                Thread.Sleep(random.Next(1000));


                if (po.CancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Cancellation requested");
                    return;
                }

                if(state.IsExceptional)
                {
                    Console.WriteLine("Exceptional");
                    return;
                }

                if (i == 100)
                {
                 //   cts.Cancel();
                //    Console.WriteLine("Cancelled");
                    throw new Exception("Exception ...");
                }


                Console.WriteLine($"{i} [{Task.CurrentId}] ");

            });

        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Cancelled by token");
        }
        catch(AggregateException ae)
        {
            ae.Handle(e =>
            {
                Console.WriteLine(e.Message);
                return true;
            });
        }

        Console.ReadKey();

    }
}


class Example3
{


    static void Main3()
    {

        var cts = new CancellationTokenSource();

        var po = new ParallelOptions();
        po.CancellationToken = cts.Token;

        var random = new Random();

        try
        {
            Parallel.For(0, 200, po, (i, state) =>
            {
                Thread.Sleep(random.Next(1000));


                if (po.CancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Cancellation requested");
                    return;
                }

                if (state.IsExceptional)
                {
                    Console.WriteLine("Exceptional");
                    return;
                }

                if(state.IsStopped)
                {
                    Console.WriteLine("Stopped");
                    return;
                }

                if (i == 100)
                {
                    //   cts.Cancel();
                    //    Console.WriteLine("Cancelled");
                    //  throw new Exception("Exception ...");
                    state.Stop();
                }


                Console.WriteLine($"{i} [{Task.CurrentId}] ");

            });

        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Cancelled by token");
        }

        Console.ReadKey();

    }
}


class Example4
{


    static void Main()
    {

        var cts = new CancellationTokenSource();

        var po = new ParallelOptions();
        po.CancellationToken = cts.Token;

        var random = new Random();

        try
        {
            ParallelLoopResult result = Parallel.For(0, 200, po, (i, state) =>
            {
                Thread.Sleep(random.Next(1000));


                if (po.CancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Cancellation requested");
                    return;
                }

                if (state.IsExceptional)
                {
                    Console.WriteLine("Exceptional");
                    return;
                }

                if (state.IsStopped)
                {
                    Console.WriteLine("Stopped");
                    return;
                }

                if (i == 100)
                {
                    //   cts.Cancel();
                    //    Console.WriteLine("Cancelled");
                    //  throw new Exception("Exception ...");
                    // state.Stop();
                    Console.WriteLine("break");
                   state.Break();
                }


                Console.WriteLine($"{i} [{Task.CurrentId}] ");


            });

            Console.WriteLine();
            Console.WriteLine($"Loop was finished? {result.IsCompleted}");
            Console.WriteLine($"Lowest brake iteration {result.LowestBreakIteration}");

        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Cancelled by token");
        }

        Console.ReadKey();

    }
}