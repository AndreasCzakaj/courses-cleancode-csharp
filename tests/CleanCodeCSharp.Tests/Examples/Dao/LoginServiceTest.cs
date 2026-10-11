using CleanCodeCSharp.Examples.Dao;

namespace CleanCodeCSharp.Tests.Examples.Dao;

public class LoginServiceTest
{
    const string KnownUserName = "knownuser";
    const string KnownUserPassword = "password";
    const string UnknownUserName = KnownUserName + "x";

    LoginService loginService = null!;

    static Credentials CreateKnownUserCredentials() => new(KnownUserName, KnownUserPassword);

    [SetUp]
    public void SetUp()
    {
        var dao = new PersonDaoMapImpl();
        dao.Repo[KnownUserName] = new Person(UserName: KnownUserName, PasswordHash: KnownUserPassword);
        loginService = new LoginService(dao);
    }

    [Test]
    public void ShouldFailForMissingCredentials()
    {
        var e = Assert.Throws<ArgumentException>(() => loginService.Login(null));
        Assert.That(e.Message, Does.Contain("No credentials provided"));
    }

    [TestCaseSource(nameof(InvalidCredentialsParams))]
    public void ShouldFailForInvalidCredentials(Credentials? input, string expected)
    {
        var e = Assert.Throws<ArgumentException>(() => loginService.Validate(input));
        Assert.That(e.Message, Does.Contain(expected));
    }

    static object?[][] InvalidCredentialsParams() =>
    [
        [null, "No credentials provided"],
        [new Credentials(), "No username provided"],
        [new Credentials(UserName: "user1"), "No password provided"],
    ];

    [Test]
    public void ShouldFailForUnknownUsername()
    {
        var input = new Credentials(UnknownUserName, "egal");

        var e = Assert.Throws<ArgumentException>(() => loginService.Login(input));

        Assert.That(e.Message, Does.Contain("Unknown user or wrong password"));
    }

    [Test]
    public void ShouldFailForWrongPassword()
    {
        var input = CreateKnownUserCredentials() with { Password = "falsch!" };

        var e = Assert.Throws<ArgumentException>(() => loginService.Login(input));

        Assert.That(e.Message, Does.Contain("Unknown user or wrong password"));
    }

    [Test]
    public void ShouldFailIfDatabaseError()
    {
        loginService = new LoginService(new PersonDaoThrowingImpl());
        var input = CreateKnownUserCredentials();

        var e = Assert.Throws<InternalServerException>(() => loginService.Login(input));

        Assert.That(e.Message, Does.Contain("oops, database error"));
    }

    [Test]
    public void ShouldYieldUserSessionIfOk()
    {
        var input = CreateKnownUserCredentials();

        var actual = loginService.Login(input);

        Assert.That(actual, Is.Not.Null);
    }
}
