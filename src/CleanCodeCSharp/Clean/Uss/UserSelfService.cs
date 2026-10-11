namespace CleanCodeCSharp.Clean.Uss;

public class UserSelfServiceException(string message, Exception? cause = null) : Exception(message, cause);

// the SAME error for an unknown username and for a wrong password:
// the response must not reveal which usernames exist
public class AuthenticationFailedException() : UserSelfServiceException("unknown username or wrong password");

public class AccountNotVerifiedException() : UserSelfServiceException("account not verified");

public class ServerException(string message, Exception? cause = null) : UserSelfServiceException(message, cause);

public class UserSelfService(IAccountDao accountDao)
{
    public IAccountDao AccountDao { get; set; } = accountDao;

    public async Task<UserSession> Login(Credentials? credentials)
    {
        var (username, password) = Validation.ValidateCredentials(credentials);

        var account = await FindAccount(username);
        if (account == null || !Password.Verify(password, account.PasswordHash))
        {
            throw new AuthenticationFailedException();
        }
        if (account.Status != AccountStatus.Verified)
        {
            throw new AccountNotVerifiedException();
        }

        return new UserSession(account.Id, account.Username, account.Email);
    }

    private async Task<Account?> FindAccount(string username)
    {
        try
        {
            return await AccountDao.FindByUsername(username);
        }
        catch (DaoException e)
        {
            throw new ServerException("Database not available. Try later.", e);
        }
    }
}
