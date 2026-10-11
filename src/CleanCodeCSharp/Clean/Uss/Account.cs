namespace CleanCodeCSharp.Clean.Uss;

public enum AccountStatus
{
    New,
    Verified,
}

/// <param name="PasswordHash">format: "&lt;salt&gt;:&lt;hash&gt;", see <see cref="Password"/></param>
public record Account(
    string Id,
    string Username,
    string PasswordHash,
    string Email,
    DateTimeOffset TcAccepted,
    AccountStatus Status
);

// untrusted input, e.g. the body of an HTTP request: anything may be missing
public record Credentials(string? Username, string? Password);

// what the validation returns: nothing is missing any more
public record ValidCredentials(string Username, string Password);

public record UserSession(string AccountId, string Username, string Email);
