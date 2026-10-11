using CleanCodeCSharp.Clean.Uss;
using static CleanCodeCSharp.Tests.Clean.Uss.TestUtils;

namespace CleanCodeCSharp.Tests.Clean.Uss;

public class ValidationTest
{
    [TestCase(null, "null")]
    [TestCase("", "empty")]
    [TestCase("        ", "empty whitespace")]
    [TestCase("1234567", "too short")]
    [TestCase("1234567 ", "too short with padding")]
    [TestCase("123456789012345678901", "too long")]
    [TestCase("1234 6789", "invalid char: blank")]
    [TestCase("1234.6789", "invalid char: allowed in passwords only")]
    [TestCase("12345678\n", "trailing newline")]
    public void ValidateUsernameShouldFail(string? given, string reason)
    {
        Assert.That(
            () => Validation.ValidateUsername(given),
            Throws.TypeOf<ValidationException>().With.Message.EqualTo("invalid: username"),
            reason
        );
    }

    [TestCase("12345678", "min size")]
    [TestCase("12345678901234567890", "max size")]
    [TestCase("aZ09-_aZ", "all allowed chars")]
    [TestCase("alice_verified", "CreateValidCredentials")]
    [TestCase("idonotexist", "CreateValidCredentialsUnknownUser")]
    public void ValidateUsernameShouldPass(string given, string reason)
    {
        Assert.That(() => Validation.ValidateUsername(given), Throws.Nothing, reason);
    }

    [TestCase(null, "null")]
    [TestCase("", "empty")]
    [TestCase("            ", "empty whitespace")]
    [TestCase("12345678901", "too short")]
    [TestCase("12345678901 ", "too short with padding")]
    [TestCase("123456789012345678901234567890123", "too long")]
    [TestCase("123456 89012", "invalid char: blank")]
    [TestCase("123456!89012", "invalid char: !")]
    [TestCase("123456789012\n", "trailing newline")]
    public void ValidatePasswordShouldFail(string? given, string reason)
    {
        Assert.That(
            () => Validation.ValidatePassword(given),
            Throws.TypeOf<ValidationException>().With.Message.EqualTo("invalid: password"),
            reason
        );
    }

    [TestCase("123456789012", "min size")]
    [TestCase("12345678901234567890123456789012", "max size")]
    [TestCase("aZ09-_.,+aZ0", "all allowed chars")]
    [TestCase(ValidPassword, "ValidPassword")]
    [TestCase(ValidButWrongPassword, "ValidButWrongPassword")]
    public void ValidatePasswordShouldPass(string given, string reason)
    {
        Assert.That(() => Validation.ValidatePassword(given), Throws.Nothing, reason);
    }

    private static readonly TestCaseData[] InvalidCredentials =
    [
        new TestCaseData(null, "username").SetArgDisplayNames("null"),
        new TestCaseData(new Credentials(null, null), "username").SetArgDisplayNames("empty object"),
        new TestCaseData(CreateValidCredentials() with { Username = "al" }, "username")
            .SetArgDisplayNames("invalid username"),
        new TestCaseData(CreateValidCredentials() with { Password = "pwd" }, "password")
            .SetArgDisplayNames("invalid password"),
        new TestCaseData(new Credentials("al", "pwd"), "username")
            .SetArgDisplayNames("both invalid: reports the 1st one"),
    ];

    [TestCaseSource(nameof(InvalidCredentials))]
    public void ValidateCredentialsShouldFail(Credentials? given, string field)
    {
        Assert.That(
            () => Validation.ValidateCredentials(given),
            Throws.TypeOf<ValidationException>().With.Property("Field").EqualTo(field)
        );
    }

    private static readonly TestCaseData[] ValidCredentials =
    [
        new TestCaseData(CreateValidCredentials()).SetArgDisplayNames("CreateValidCredentials"),
        new TestCaseData(CreateValidCredentialsNotVerifiedAccount())
            .SetArgDisplayNames("CreateValidCredentialsNotVerifiedAccount"),
        new TestCaseData(CreateValidCredentialsUnknownUser())
            .SetArgDisplayNames("CreateValidCredentialsUnknownUser"),
    ];

    [TestCaseSource(nameof(ValidCredentials))]
    public void ValidateCredentialsShouldPass(Credentials given)
    {
        var actual = Validation.ValidateCredentials(given);

        Assert.That(actual, Is.EqualTo(new ValidCredentials(given.Username!, given.Password!)));
    }

    [Test]
    public void TheErrorShouldNameTheInvalidField()
    {
        var error = new ValidationException("username");

        Assert.Multiple(() =>
        {
            Assert.That(error.Field, Is.EqualTo("username"));
            Assert.That(error.Message, Is.EqualTo("invalid: username"));
        });
    }
}
