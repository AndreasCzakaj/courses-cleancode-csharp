using CleanCodeCSharp.Clean.Uss;

namespace CleanCodeCSharp.Tests.Clean.Uss;

public class ControllerUtilsTest
{
    private static readonly TestCaseData[] StatusCases =
    [
        new TestCaseData(new ValidationException("username"), 400)
            .SetArgDisplayNames("invalid input is the client's fault"),
        new TestCaseData(new AuthenticationFailedException(), 401)
            .SetArgDisplayNames("unknown username or wrong password"),
        new TestCaseData(new AccountNotVerifiedException(), 400).SetArgDisplayNames("account not verified"),
        new TestCaseData(new ServerException("oops"), 500).SetArgDisplayNames("a server error is our fault"),
        new TestCaseData(new DaoException("oops"), 500).SetArgDisplayNames("any other error is our fault"),
        new TestCaseData(new NullReferenceException(), 500).SetArgDisplayNames("a bug is our fault"),
    ];

    [TestCaseSource(nameof(StatusCases))]
    public void ShouldMapToHttpStatus(Exception error, int expected)
    {
        Assert.That(ControllerUtils.CalcHttpErrorCode(error), Is.EqualTo(expected));
    }

    private static readonly TestCaseData[] MessageCases =
    [
        new TestCaseData(new ValidationException("username"), "invalid: username")
            .SetArgDisplayNames("names the invalid field"),
        new TestCaseData(new AuthenticationFailedException(), "unknown username or wrong password")
            .SetArgDisplayNames("does not reveal which one"),
        new TestCaseData(new AccountNotVerifiedException(), "account not verified")
            .SetArgDisplayNames("account not verified"),
        new TestCaseData(new ServerException("db is down"), ControllerUtils.MessageServerError)
            .SetArgDisplayNames("hides the server's internals: ServerException"),
        new TestCaseData(new DaoException("SQLite Error"), ControllerUtils.MessageServerError)
            .SetArgDisplayNames("hides the server's internals: DaoException"),
        new TestCaseData(new NullReferenceException(), ControllerUtils.MessageServerError)
            .SetArgDisplayNames("hides the server's internals: a bug"),
    ];

    [TestCaseSource(nameof(MessageCases))]
    public void ShouldMapToMessage(Exception error, string expected)
    {
        Assert.That(ControllerUtils.CalcErrorMessage(error), Is.EqualTo(expected));
    }
}
