namespace CleanCodeCSharp.Dirty.Functions;

// C# does not need a builder to get "named args":
// "required" + "init" properties are set via an object initializer, e.g.
//   var person = new Person { Name = "Kim", Age = 42 };
public class Person
{
    public required string Name { get; init; }
    public int? Age { get; init; }
    public string? Email { get; init; }
}
