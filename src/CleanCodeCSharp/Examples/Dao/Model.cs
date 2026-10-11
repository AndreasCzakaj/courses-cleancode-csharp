namespace CleanCodeCSharp.Examples.Dao;

public record Credentials(string? UserName = null, string? Password = null);

public record Person(string UserName, string PasswordHash, string? Email = null, string? Id = null);

public record SignUpData(string UserName, string? Password = null, string? Email = null);

public record SignUpResult(Person Person);

public record UserSession;
