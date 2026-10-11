namespace CleanCodeCSharp.Clean.Uss;

public class AccountDaoThrowingImpl : IAccountDao
{
    public Task<Account?> FindByUsername(string username)
    {
        throw new DaoException("FindByUsername: oops");
    }
}
