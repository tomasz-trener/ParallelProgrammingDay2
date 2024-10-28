class Example1
{   
    static void Main1()
    {

        //for (int i = 0; i < 3; i++)
        //{
        //    Action action = () => Console.WriteLine(i);
        //    action();
        //}

        List<Action> actions = new List<Action>();

        for (int i = 0; i < 3; i++)
        {
           // int a= i;
            actions.Add(() => Console.WriteLine(i));
           
        }

        foreach (var action in actions)
        {
            action();
        }

    }
}


class Example2
{
    static void Main2()
    {
        var a = new Action(() => Console.WriteLine($"First {Task.CurrentId}"));
        var b = new Action(() => Console.WriteLine($"Second {Task.CurrentId}"));
        var c = new Action(() => Console.WriteLine($"Third {Task.CurrentId} "));

        Parallel.Invoke(a, b, c);

    }
}


class Example3
{
    static void Main3()
    {

        Parallel.For(1, 11, i =>
        {
            Console.WriteLine($"{i*i}\t");
        });

        Console.WriteLine("ok");
    }
}


class Example4
{
    static void Main4()
    {
        string[] words = { "one", "two", "three", "four", "five" };
        Parallel.ForEach(words, word =>
        {
            Console.WriteLine($"{word} has length {word.Length} (task {Task.CurrentId})");
        });

        Console.WriteLine("ok");
    }
}

class Example5
{
    public static IEnumerable<int> Range(int start, int end, int step)
    {
        for (int i = start; i < end; i+=step)
        {
            yield return i;
        }  
    }

    static void Main5()
    {
        Parallel.ForEach(Range(1, 11, 2), i =>
        {
            Console.WriteLine($"{i}\t");
        });

        Console.WriteLine("ok");
    }
}


class Example6
{
 

    static void Main()
    {
        string[] words = { "one", "two", "three", "four", "five" };

        var po = new ParallelOptions();
        po.MaxDegreeOfParallelism = 3;
        

        Parallel.ForEach(words, po, word =>
        {
            Console.WriteLine($"{word} has length {word.Length} (task {Task.CurrentId})");
        });

        Console.WriteLine("ok");
    }
}