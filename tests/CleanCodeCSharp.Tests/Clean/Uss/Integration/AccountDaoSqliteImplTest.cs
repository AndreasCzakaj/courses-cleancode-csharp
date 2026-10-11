using CleanCodeCSharp.Clean.Uss;
using static CleanCodeCSharp.Tests.Clean.Uss.TestUtils;

namespace CleanCodeCSharp.Tests.Clean.Uss.Integration;

[Category("Integration")]
public class AccountDaoSqliteImplTest
{
    private SqliteTestDatabase database;
    private AccountDaoSqliteImpl dao;

    [SetUp]
    public async Task SetUp()
    {
        database = await SqliteTestDatabase.Create();
        dao = new AccountDaoSqliteImpl(database.ConnectionString);
    }

    [TearDown]
    public void TearDown()
    {
        database.Dispose();
    }

    [Test]
    public async Task ShouldFindAnAccountByItsUsername()
    {
        // given
        var account = CreateVerifiedAccount();
        await dao.Save(account);
        await dao.Save(CreateNotVerifiedAccount());

        // when
        var actual = await dao.FindByUsername(account.Username);

        // then: the account as it was saved
        Assert.That(actual, Is.EqualTo(account));
    }

    [Test]
    public async Task ShouldReturnNullForAnUnknownUsername()
    {
        Assert.That(await dao.FindByUsername("idonotexist"), Is.Null);
    }

    [Test]
    public void ShouldThrowADaoExceptionIfTheDatabaseIsMissing()
    {
        // given: no such file, and SQLite must not create it
        var missing = Path.Combine(Path.GetTempPath(), $"uss-missing-{Guid.NewGuid()}", "uss.db");
        dao = new AccountDaoSqliteImpl($"Data Source={missing};Mode=ReadWrite");

        // then
        Assert.That(
            () => dao.FindByUsername("alice_verified"),
            Throws.TypeOf<DaoException>().With.Message.EqualTo("SQLite Error")
        );
    }

    [Test]
    public async Task ShouldThrowADaoExceptionIfTheUsernameIsTaken()
    {
        await dao.Save(CreateVerifiedAccount());

        Assert.That(() => dao.Save(CreateVerifiedAccount()), Throws.TypeOf<DaoException>());
    }
}
