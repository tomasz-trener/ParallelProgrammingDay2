public class Example1
{
    static int bread = 0;
    static int coffe = 0;

    static int inconsistentCount = 0;

    static void Thread_WithoutMemoryBarrier()
    {
        for (int i = 0; i < 1000000; i++)
        {
            bread = 0;
            coffe = 0;

            bread = 1;
            coffe = 1;
        }
     
    }

    static void Thread_WithMemoryBarrier()
    {
        for (int i = 0; i < 1000000; i++)
        {
            bread = 0;
            coffe = 0;

            bread = 1;
            Thread.MemoryBarrier();
            coffe = 1;
        }

    }

    static void ThreadB()
    {
        for (int i = 0; i < 1000000; i++)
        {
            if (bread == 1 && coffe == 0)
            {
                Interlocked.Increment(ref inconsistentCount);
            }
        }
     }

    static void Test(bool useMemoryBarier)
    {
        inconsistentCount = 0;

        Task t1 = Task.Run(() =>
        {
            if(useMemoryBarier)
               Thread_WithMemoryBarrier();
            else
               Thread_WithoutMemoryBarrier();
        });

        Task t2 = Task.Run(() => ThreadB());
        Task.WaitAll(t1, t2);
    }


    static void Main()
    {
        Console.WriteLine("Test without memorybarrier");
        Test(false);
        Console.WriteLine($"number of inconsistent {inconsistentCount}");

        Console.WriteLine("Test with memorybarrier");
        Test(true);
        Console.WriteLine($"number of inconsistent {inconsistentCount}");

    }
}

