// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using Leonardo;
using Microsoft.Extensions.Configuration;

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");   
IConfiguration configuration = new ConfigurationBuilder() 
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddEnvironmentVariables()        
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)   
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true) 
    .Build();
var applicationName = configuration.GetValue<string>("Application:Name");  
var applicationMessage = configuration.GetValue<string>("Application:Message");   
Console.WriteLine($"Application Name : {applicationName}");   
Console.WriteLine($"Application Message : {applicationMessage}"); 

var stopwatch = new Stopwatch();
stopwatch.Start();
await using FibonacciDataContext context = new();
var results = await new Fibonacci(context).RunAsync(args);
stopwatch.Stop();
Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds}ms");
foreach (var result in results)
{
    Console.WriteLine($"Fibonacci for input {result.Input} is {result.Result}");
}


