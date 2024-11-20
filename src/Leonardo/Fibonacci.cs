﻿namespace Leonardo;

public record FibonacciResult(int Input, int Result);

public static class Fibonacci
{
    private static int Run(int n)
    {
        if (n <= 1) return n;
        return Run(n - 1) + Run(n - 2);
    }
    
    public static async Task<List<FibonacciResult>> RunAsync(string[] strings)
    {
        var tasks = new List<Task<FibonacciResult>>();

        foreach (var input in strings)
        {

            var int32 = Convert.ToInt32(input);
            var r = Task.Run(() => 
            {
                var result = Fibonacci.Run(int32);
                return new FibonacciResult(int32, result);
            });
            tasks.Add(r);
        }

        //Task.WaitAll(tasks.ToArray());
        var results = new List<FibonacciResult>();
        foreach (var task in tasks)
        {
            var r = await task;
            results.Add(r);
        }
        return results;
    }

}