namespace Leonardo.Tests;

public class UnitTest1
{
    [Fact]
    public async void Test1()
    {
        var strings = new string[] { "42" };
        var results = await Fibonacci.RunAsync(strings);
        Assert.Equal(42, results[0].Input);
        Assert.Equal(267914296, results[0].Result);
    }
}