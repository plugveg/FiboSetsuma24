using Leonardo;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello Thomas!");

app.MapGet("/Fibonacci", 
    async () => await Fibonacci.RunAsync(new []{"42", "22"}));

app.Run();
