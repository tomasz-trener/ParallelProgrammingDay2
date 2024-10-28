public class Example1
{


    static void Main()
    {

       var sum=   Enumerable.Range(1, 1000).Sum();
        Console.WriteLine(sum);


        var sum1= Enumerable.Range(1, 1000)
            .Aggregate(
                0,
                (acc, n) => acc + n,
                acc => acc
            );
        Console.WriteLine(sum1);


       var sum2= ParallelEnumerable.Range(1, 1000)
            .Aggregate(
                0,
                (partialSum, n) => partialSum + n,
                (total, subTotal) => total + subTotal,
                acc => acc
            );


        Console.WriteLine(sum2);

    }
}

