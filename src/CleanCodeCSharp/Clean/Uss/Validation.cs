using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace CleanCodeCSharp.Clean.Uss;

public class ValidationException(string field) : Exception($"invalid: {field}")
{
    public string Field { get; } = field;
}

// [NotNull]: after the call of a validation function, the compiler knows that the value is not null.
public static partial class Validation
{
    // \z, not $: in .NET, $ also matches before a trailing newline
    [GeneratedRegex(@"^[a-zA-Z0-9\-_]{8,20}\z")]
    private static partial Regex RegexUsername();

    [GeneratedRegex(@"^[a-zA-Z0-9\-_.,+]{12,32}\z")]
    private static partial Regex RegexPassword();

    public static void ValidateUsername([NotNull] string? username)
    {
        if (!Matches(username, RegexUsername()))
        {
            throw new ValidationException("username");
        }
    }

    public static void ValidatePassword([NotNull] string? password)
    {
        if (!Matches(password, RegexPassword()))
        {
            throw new ValidationException("password");
        }
    }

    public static ValidCredentials ValidateCredentials(Credentials? credentials)
    {
        // the input is untrusted: it may be missing altogether
        var (username, password) = credentials ?? new Credentials(null, null);
        ValidateUsername(username);
        ValidatePassword(password);
        return new ValidCredentials(username, password);
    }

    private static bool Matches([NotNullWhen(true)] string? value, Regex regex)
    {
        return value != null && regex.IsMatch(value);
    }
}
