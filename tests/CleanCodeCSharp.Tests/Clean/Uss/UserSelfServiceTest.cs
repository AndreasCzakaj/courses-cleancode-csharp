using CleanCodeCSharp.Clean.Uss;
using static CleanCodeCSharp.Tests.Clean.Uss.TestUtils;

namespace CleanCodeCSharp.Tests.Clean.Uss;

public class UserSelfServiceTest
{
    private UserSelfService service;

    [SetUp]
    public void SetUp()
    {
        service = CreateUserSelfServiceWithWorkingDeps();
    }

    [Test]
    public void IfIPassNoCredentialsThereShouldBeAValidationError()
    {
        // given
        Credentials? credentials = null;

        // when
        Task Actual() => service.Login(credentials);

        // then
        Assert.That(Actual, Throws.TypeOf<ValidationException>().With.Message.EqualTo("invalid: username"));
    }

    [Test]
    public void IfIPassSyntacticallyInvalidCredentialsThereShouldBeAValidationError()
    {
        // given
        var credentials = CreateValidCredentials() with { Password = "tooshort" };

        // when
        Task Actual() => service.Login(credentials);

        // then
        Assert.That(Actual, Throws.TypeOf<ValidationException>().With.Message.EqualTo("invalid: password"));
    }

    [Test]
    public void IfIDontHaveAnAccountThereShouldBeALoginError()
    {
        // given
        var credentials = CreateValidCredentialsUnknownUser();

        // when
        Task Actual() => service.Login(credentials);

        // then
        Assert.That(Actual, Throws.TypeOf<AuthenticationFailedException>());
    }

    [Test]
    public void IfIDontPassTheRightPasswordThereShouldBeTheSameLoginError()
    {
        // given
        var credentials = CreateValidCredentials() with { Password = ValidButWrongPassword };

        // when
        Task Actual() => service.Login(credentials);

        // then
        Assert.That(Actual, Throws.TypeOf<AuthenticationFailedException>());
    }

    [Test]
    public void IfMyAccountIsNotVerifiedYetThereShouldBeAnError()
    {
        // given
        var credentials = CreateValidCredentialsNotVerifiedAccount();

        // when
        Task Actual() => service.Login(credentials);

        // then
        Assert.That(Actual, Throws.TypeOf<AccountNotVerifiedException>());
    }

    [Test]
    public void IfMyAccountIsNotVerifiedAndThePasswordIsWrongThereShouldBeTheLoginError()
    {
        // given
        var credentials = CreateValidCredentialsNotVerifiedAccount() with { Password = ValidButWrongPassword };

        // when
        Task Actual() => service.Login(credentials);

        // then: the status of an account is none of a stranger's business
        Assert.That(Actual, Throws.TypeOf<AuthenticationFailedException>());
    }

    [Test]
    public void IfTheDatabaseDoesNotWorkThenIShouldGetAnAppropriateError()
    {
        // given
        service.AccountDao = new AccountDaoThrowingImpl();
        var credentials = CreateValidCredentials();

        // when
        Task Actual() => service.Login(credentials);

        // then
        Assert.That(
            Actual,
            Throws.TypeOf<ServerException>().With.Message.EqualTo("Database not available. Try later.")
        );
    }

    [Test]
    public async Task IfIPassValidCredentialsOfAVerifiedAccountThenIGetASession()
    {
        // given
        var credentials = CreateValidCredentials();
        var account = CreateVerifiedAccount();

        // when
        var actual = await service.Login(credentials);

        // then: exactly these fields, e.g. no password hash
        Assert.That(actual, Is.EqualTo(new UserSession(account.Id, account.Username, account.Email)));
    }
}
