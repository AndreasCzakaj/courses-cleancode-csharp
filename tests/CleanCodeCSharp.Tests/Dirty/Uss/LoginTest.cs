using CleanCodeCSharp.Dirty.Uss;

namespace CleanCodeCSharp.Tests.Dirty.Uss;

public class LoginTest
{
    [TestCase("zukurz1", "passwordok12", "invalid: username")]
    [TestCase("zulang789012345678901", "passwordok12", "invalid: username")]
    [TestCase("zulang#8", "passwordok12", "invalid: username")]
    [TestCase(null, "passwordok12", "invalid: username")]
    public void ItShouldFailForInvalidData(string? username, string? password, string expectedError)
    {
        var service = new LoginService();

        // when
        var actual = service.ValidateUserCredentials(username, password);

        // then
        Assert.That(actual.ok, Is.EqualTo(false));
        Assert.That(actual.error, Is.EqualTo(expectedError));
    }

    /*[Test]
    public async Task<int> ItShouldReturnSessionData()
    {
        var service = new LoginServiceFake();
        LoginResult result = await service.Login("horst123", "wlgwlg:wlgkwg");
        Assert.That(result.errorType, Is.EqualTo(ErrorTypes.OK));
        Assert.That(result.UserSession.AccountId, Is.EqualTo("1"));
    }*/
}

class LoginServiceFake : LoginService
{
    public override async Task<UserAccount?> getUserAccount(string u)
    {
        return new UserAccount("1", "horst123", "wlgwlg:wlgkwg", "ich@internet.com", "verified");
    }

}
