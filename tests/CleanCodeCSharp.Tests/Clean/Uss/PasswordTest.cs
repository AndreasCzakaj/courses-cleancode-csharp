using CleanCodeCSharp.Clean.Uss;
using static CleanCodeCSharp.Tests.Clean.Uss.TestUtils;

namespace CleanCodeCSharp.Tests.Clean.Uss;

public class PasswordTest
{
    private const string Salt = "00112233445566778899aabbccddeeff";
    private static readonly string PasswordHash = Password.Hash(ValidPassword, Salt);

    [Test]
    public void HashShouldReturnSaltAndHashHexEncoded()
    {
        Assert.That(Password.Hash(ValidPassword), Does.Match(@"^[a-f0-9]{32}:[a-f0-9]{128}\z"));
    }

    [Test]
    public void HashShouldNotContainThePassword()
    {
        Assert.That(Password.Hash(ValidPassword), Does.Not.Contain(ValidPassword));
    }

    [Test]
    public void HashShouldBeReproducibleForTheSameSalt()
    {
        Assert.That(Password.Hash(ValidPassword, Salt), Is.EqualTo(PasswordHash));
    }

    [Test]
    public void HashShouldUseANewSaltEachTime()
    {
        var first = Password.Hash(ValidPassword);
        var second = Password.Hash(ValidPassword);

        // same password, different hashes
        Assert.That(first, Is.Not.EqualTo(second));
    }

    [Test]
    public void VerifyShouldAcceptTheRightPassword()
    {
        Assert.That(Password.Verify(ValidPassword, PasswordHash), Is.True);
    }

    private static readonly TestCaseData[] Rejected =
    [
        new TestCaseData(ValidButWrongPassword, PasswordHash).SetArgDisplayNames("wrong password"),
        new TestCaseData("", PasswordHash).SetArgDisplayNames("empty password"),
        new TestCaseData(ValidPassword, Salt).SetArgDisplayNames("stored value w/out hash"),
        new TestCaseData(ValidPassword, "").SetArgDisplayNames("stored value empty"),
        new TestCaseData(ValidPassword, PasswordHash[..^1]).SetArgDisplayNames("stored hash truncated"),
    ];

    [TestCaseSource(nameof(Rejected))]
    public void VerifyShouldReject(string password, string given)
    {
        Assert.That(Password.Verify(password, given), Is.False);
    }
}
