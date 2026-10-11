namespace CleanCodeCSharp.Examples.Di.Di1;

// Without Dependency Inversion:
// the dependencies point in the same direction as the control flow,
// from the entry point down to the details.

public class Program
{
    public static void Main()
    {
        var app = new App();
        app.Begin();
    }
}

public class App
{
    private readonly SomeService someService = new();

    public void Begin()
    {
        someService.Process();
    }
}

public class SomeService
{
    private readonly TheRealDatabase db = new();

    public void Process()
    {
        db.Load();
    }
}

public class TheRealDatabase
{
    public void Load()
    {
    }
}
