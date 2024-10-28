class Example1
{


    static SemaphoreSlim pool = new SemaphoreSlim(3,5);

    static void Person(int id)
    {
        Console.WriteLine($"Person {id} is waiting for the signal");

        pool.Wait();

        Console.WriteLine($"Worker {id}  starts working ");

        Thread.Sleep(3000);

        Console.WriteLine($"Worker {id}  finishes working ");

        pool.Release();
    }


    static void Main1()
    {

        for (int i = 0; i < 10; i++)
        {
            int personId = i;
            Task.Factory.StartNew(() => Person(personId));
        }



        Console.ReadKey();

    }
}


class Example2
{


    static Semaphore parkSemaphore = new Semaphore(3, 5, "GlobalSemaphoreName");
 
    static void Vistor(int id)
    {
        Console.WriteLine($"Vistor {id} is waiting for the signal");

        parkSemaphore.WaitOne();

        Console.WriteLine($"Vistor {id}  starts working ");
 
    }


    static void Main()
    {
        int visitorcounter = 1;

        while (true)
        {
            var key = Console.ReadKey().Key;

            if (key == ConsoleKey.S)
            {
                int visitorId = visitorcounter++;
                Task.Run(() => Vistor(visitorId));
            }
            else if (key == ConsoleKey.E)
            {
                parkSemaphore.Release();
                Console.WriteLine("Visitor left the park");
            }

        }


        Console.ReadKey();

    }
}