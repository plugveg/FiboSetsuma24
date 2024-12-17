using System.Text.Json.Serialization;
using Leonardo;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FibonacciDataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<Fibonacci>();

var app = builder.Build();

await using FibonacciDataContext context = new();

app.MapGet("/", () => "Hello Setsumafu!");

app.MapGet("/fibonacci/{number:int}", async (int number, Fibonacci fibonacci, ILogger<Program> logger) =>
{
    logger.LogInformation("Requesting Fibonacci for number {Number}", number);
    var result = await fibonacci.RunAsync([number.ToString()]);
    return Results.Json(result, FibonacciContext.Default.ListFibonacciResult);
});

app.Run();

[JsonSerializable(typeof(List<FibonacciResult>))]
internal partial class FibonacciContext : JsonSerializerContext
{
}