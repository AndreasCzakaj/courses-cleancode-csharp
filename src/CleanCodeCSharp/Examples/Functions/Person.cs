namespace CleanCodeCSharp.Examples.Functions;

public record Person(Company? Company = null);

public record Company(string? Name = null);
