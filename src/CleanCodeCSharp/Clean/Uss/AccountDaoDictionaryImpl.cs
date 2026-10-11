namespace CleanCodeCSharp.Clean.Uss;

public class AccountDaoDictionaryImpl(params Account[] accounts) : IAccountDao
{
    private readonly Dictionary<string, Account> repo = accounts.ToDictionary(account => account.Username);

    public Task<Account?> FindByUsername(string username)
    {
        return Task.FromResult(repo.GetValueOrDefault(username));
    }
}
