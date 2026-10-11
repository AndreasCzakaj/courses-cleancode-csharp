namespace CleanCodeCSharp.Examples.Di.Di2;

// With Dependency Inversion:
// SomeService depends on an interface ("Port"),
// the detail ("Adapter") depends on that interface, too.

public interface IDatabasePort
{
    void Load();
}

public class SomeService(IDatabasePort db)
{
    public void Process()
    {
        db.Load();
    }
}

public class App(SomeService someService)
{
    public void Begin()
    {
        someService.Process();
    }
}

public class DatabaseAdapter : IDatabasePort
{
    public void Load()
    {
    }
}

public class DependenciesContainer
{
    public App CreateApp()
    {
        IDatabasePort db = new DatabaseAdapter();
        var someService = new SomeService(db);
        return new App(someService);
    }
}

public class Program
{
    public static void Main()
    {
        var app = new DependenciesContainer().CreateApp();
        app.Begin();
    }
}
