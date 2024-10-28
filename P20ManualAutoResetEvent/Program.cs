

class Example1
{
  

    static ManualResetEventSlim evt = new ManualResetEventSlim(false);

    static void Worker(int id)
    {
        Console.WriteLine($"Worker {id} is waiting for the signal");

        evt.Wait();

        Console.WriteLine($"Worker {id}  starts working ");
    }


    static void Main1()
    {

        for (int i = 0; i < 5; i++)
        {
            int workerId = i;
            Task.Factory.StartNew(() => Worker(workerId));
        }

        Thread.Sleep(3000);

        evt.Set();

        Console.WriteLine("Signal sent");
        Console.ReadKey();

    }
}

class Example2
{


    static AutoResetEvent evt = new AutoResetEvent(false);

    static void Worker(int id)
    {
        Console.WriteLine($"Worker {id} is waiting for the signal");

        evt.WaitOne();

        Console.WriteLine($"Worker {id}  starts working ");
    }


    static void Main()
    {

        for (int i = 0; i < 5; i++)
        {
           // int workerId = i;
           Thread.Sleep(1000);
            Task.Factory.StartNew(() => Worker(i));
        }



        for (int i = 0; i < 5; i++)
        {
            Thread.Sleep(1000);
            Console.WriteLine("allowing worker to enter");
            evt.Set();
        }



        Console.WriteLine("Signal sent");
        Console.ReadKey();

    }
}