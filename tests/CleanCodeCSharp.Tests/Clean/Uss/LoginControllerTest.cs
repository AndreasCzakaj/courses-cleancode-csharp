using CleanCodeCSharp.Clean.Uss;
using static CleanCodeCSharp.Tests.Clean.Uss.TestUtils;

namespace CleanCodeCSharp.Tests.Clean.Uss;

public class LoginControllerTest
{
    private const string MessageLoginError = "unknown username or wrong password";

    private UserSelfService service;
    private List<Exception> loggedErrors;
    private LoginController ctrl;

    [SetUp]
    public void SetUp()
    {
        service = CreateUserSelfServiceWithWorkingDeps();
        loggedErrors = [];
        ctrl = new LoginController(service, loggedErrors.Add);
    }

    private static readonly TestCaseData[] ClientErrors =
    [
        new TestCaseData(null, 400, "invalid: username").SetArgDisplayNames("400: no body"),
        new TestCaseData(CreateValidCredentials() with { Username = "al" }, 400, "invalid: username")
            .SetArgDisplayNames("400: invalid username"),
        new TestCaseData(CreateValidCredentials() with { Password = "pwd" }, 400, "invalid: password")
            .SetArgDisplayNames("400: invalid password"),
        new TestCaseData(CreateValidCredentialsUnknownUser(), 401, MessageLoginError)
            .SetArgDisplayNames("401: no such username"),
        new TestCaseData(CreateValidCredentials() with { Password = ValidButWrongPassword }, 401, MessageLoginError)
            .SetArgDisplayNames("401: wrong password"),
        new TestCaseData(CreateValidCredentialsNotVerifiedAccount(), 400, "account not verified")
            .SetArgDisplayNames("400: account not verified"),
    ];

    [TestCaseSource(nameof(ClientErrors))]
    public async Task ShouldAnswerWithAnError(Credentials? body, int status, string error)
    {
        // when
        var actual = await ctrl.Action(body);

        // then
        Assert.That(actual, Is.EqualTo(new HttpResult(status, new ErrorContent(error))));
        Assert.That(loggedErrors, Is.Empty);
    }

    [Test]
    public async Task ShouldAnswerWith500IfTheDatabaseIsNotAvailable()
    {
        // given
        service.AccountDao = new AccountDaoThrowingImpl();

        // when
        var actual = await ctrl.Action(CreateValidCredentials());

        // then: the client gets no details, the log does
        Assert.That(
            actual,
            Is.EqualTo(new HttpResult(500, new ErrorContent(ControllerUtils.MessageServerError)))
        );
        Assert.That(loggedErrors, Has.Exactly(1).TypeOf<ServerException>());
    }

    [Test]
    public async Task ShouldAnswerWith200AndASession()
    {
        // given
        var account = CreateVerifiedAccount();

        // when
        var actual = await ctrl.Action(CreateValidCredentials());

        // then
        Assert.That(
            actual,
            Is.EqualTo(new HttpResult(200, new UserSession(account.Id, account.Username, account.Email)))
        );
    }
}
