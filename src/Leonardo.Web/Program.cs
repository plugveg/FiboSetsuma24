using System.Text.Json.Serialization;
using Leonardo;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

await using FibonacciDataContext context = new();

app.MapGet("/", () => "Hello Setsumafu!");

app.MapGet("/fibonacci", async () =>
{
    var result = await new Fibonacci(context).RunAsync(["42"]);
    return Results.Json(result, FibonacciContext.Default.ListFibonacciResult);
});

app.Run();

[JsonSerializable(typeof(List<FibonacciResult>))]
internal partial class FibonacciContext : JsonSerializerContext
{
}