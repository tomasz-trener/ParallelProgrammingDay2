

class Example1
{
    private static int taskCount = 5;
    static CountdownEvent cte = new CountdownEvent(taskCount);
    static Random random = new Random();
    static void Main()
    {
       
        var tasks = new Task[taskCount];

        for (int i = 0; i < taskCount; i++)
        {
            tasks[i] = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Task {0} running", Task.CurrentId);
                Thread.Sleep(random.Next(3000));
                cte.Signal();
            });
        }

        var finalTask = Task.Factory.StartNew(() =>
        {
            Console.WriteLine("Final task running");
            cte.Wait();
            Console.WriteLine("All tasks completed");
        });

        finalTask.Wait();
    }
}