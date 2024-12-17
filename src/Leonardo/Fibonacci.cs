using Microsoft.EntityFrameworkCore;

namespace Leonardo;

public record FibonacciResult(int Input, long Result);

public class Fibonacci
{
    private readonly FibonacciDataContext _context;

    public Fibonacci(FibonacciDataContext context)
    {
        _context = context;
    }
    
    private int Run(int n)
    {
        if (n <= 1) return n;
        return Run(n - 1) + Run(n - 2);
    }
    
    public async Task<List<FibonacciResult>> RunAsync(string[] strings)
    {
        var tasks = new List<Task<FibonacciResult>>();

        foreach (var input in strings)
        {

            var int32 = Convert.ToInt32(input);
            var t_fibo = await _context.TFibonaccis.Where(t => t.FibInput == int32).FirstOrDefaultAsync();
            if (t_fibo != null)
            {
                var r = Task.Run(() => new FibonacciResult(t_fibo.FibInput, t_fibo.FibOutput));
                tasks.Add(r);
            }
            else
            {
                var r = Task.Run(() => 
                {
                    var result = Run(int32);
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
            var existingEntry = await _context.TFibonaccis.Where(t => t.FibInput == r.Input).FirstOrDefaultAsync();
            if (existingEntry == null)
            {
                _context.TFibonaccis.Add(new TFibonacci { FibId = Guid.NewGuid(), FibInput = r.Input, FibOutput = r.Result });
            }
            results.Add(r);
        }
        await _context.SaveChangesAsync();
        return results;
    }

}