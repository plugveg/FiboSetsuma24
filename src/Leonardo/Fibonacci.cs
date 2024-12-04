using Microsoft.EntityFrameworkCore;

namespace Leonardo;

public record FibonacciResult(int Input, long Result);

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
        await using var context = new FibonacciDataContext();

        foreach (var input in strings)
        {

            var int32 = Convert.ToInt32(input);
            var t_fibo = await context.TFibonaccis.Where(t => t.FibInput == int32).FirstOrDefaultAsync();
            if (t_fibo != null)
            {
                var r = Task.Run(() => new FibonacciResult(t_fibo.FibInput, t_fibo.FibOutput));
                tasks.Add(r);
            }
            else
            {
                var r = Task.Run(() => 
                {
                    var result = Fibonacci.Run(int32);
                    return new FibonacciResult(int32, result);
                });
                tasks.Add(r);   
            }
        }

        //Task.WaitAll(tasks.ToArray());
        var results = new List<FibonacciResult>();
        foreach (var task in tasks)
        {
            var r = await task;
            var existingEntry = await context.TFibonaccis.Where(t => t.FibInput == r.Input).FirstOrDefaultAsync();
            if (existingEntry == null)
            {
                context.TFibonaccis.Add(new TFibonacci { FibId = Guid.NewGuid(), FibInput = r.Input, FibOutput = r.Result });
            }
            results.Add(r);
        }
        await context.SaveChangesAsync();
        return results;
    }

}