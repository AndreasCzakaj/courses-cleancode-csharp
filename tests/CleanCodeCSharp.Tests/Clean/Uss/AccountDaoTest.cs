using CleanCodeCSharp.Clean.Uss;
using static CleanCodeCSharp.Tests.Clean.Uss.TestUtils;

namespace CleanCodeCSharp.Tests.Clean.Uss;

public class AccountDaoTest
{
    private readonly Account account = CreateVerifiedAccount();

    [Test]
    public async Task DictionaryImplShouldFindAnAccountByItsUsername()
    {
        var dao = new AccountDaoDictionaryImpl(account);

        Assert.That(await dao.FindByUsername(account.Username), Is.EqualTo(account));
    }

    [Test]
    public async Task DictionaryImplShouldReturnNullForAnUnknownUsername()
    {
        var dao = new AccountDaoDictionaryImpl();

        Assert.That(await dao.FindByUsername(account.Username), Is.Null);
    }

    [Test]
    public void ThrowingImplShouldThrowADaoException()
    {
        var dao = new AccountDaoThrowingImpl();

        Assert.That(
            () => dao.FindByUsername("anyone"),
            Throws.TypeOf<DaoException>().With.Message.EqualTo("FindByUsername: oops")
        );
    }
}
