using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;

namespace CleanCodeCSharp.Dirty.Uss;

public class LoginService
{

    public async Task<LoginResult> Login(string u, string p)
    {
        var validationResult = ValidateUserCredentials(u, p);
        if (!validationResult.ok)
        {
            return new LoginResult(ErrorTypes.Validation, validationResult.error);
        }

        Dictionary<string, UserAccount> dic = new Dictionary<string, UserAccount>();
        dic.Add("test", new UserAccount("test", "test", "test", "verified", "test"));
        var account2 = dic.GetValueOrDefault("test", null);


        var account = await getUserAccount(u);

        if (account == null)
        {
            return new LoginResult(ErrorTypes.Validation, "unknown username or wrong password");
        }

        if (IsMatchingPassword(p, account.passwordHash))
        {
            return new LoginResult(ErrorTypes.Auth, "unknown username or wrong password");
        }
        if (account.status != "verified")
        {
            return new LoginResult(ErrorTypes.Auth, "account not verified");
        }

        var session = new UserSession(account.AccountId, account.username, account.email);
        return new LoginResult(ErrorTypes.OK, null, session);
    }

    protected virtual bool IsMatchingPassword(string plainPassword, string passwordHash)
    {
        var tmp = passwordHash.Split(":");
        var h = Convert.ToHexString(Rfc2898DeriveBytes.Pbkdf2(
            plainPassword, Encoding.UTF8.GetBytes(tmp[0]), 210_000,
            HashAlgorithmName.SHA512, 64)).ToLower();

        return h == tmp[1];
    }

    public virtual async Task<UserAccount?> getUserAccount(string u)
    {
        SqliteConnection c;
        // check db
        c = new SqliteConnection(
            Environment.GetEnvironmentVariable("USS_DB") ?? "Data Source=uss.db"
        );
        await c.OpenAsync();
        var cmd = c.CreateCommand();
        cmd.CommandText =
            "SELECT id, username, password_hash, email, status FROM accounts WHERE username = $u";
        cmd.Parameters.AddWithValue("$u", u);
        var r = await cmd.ExecuteReaderAsync();

        if (await r.ReadAsync())
        {
            return new UserAccount(
                r.GetString(0),
                r.GetString(1),
                r.GetString(2),
                r.GetString(3),
                r.GetString(4)
            );
        }

        return null;
    }

    public Result ValidateUserCredentials(string? username, string? password)
    {
        if (username == null || username.Length < 8 || username!.Length > 20 || !Regex.IsMatch(username!, @"^[a-zA-Z0-9\-_]+\z"))
        {
            return new Result(false, "invalid: username");
        }

        if (password == null || password!.Length < 12 || password!.Length > 32 || !Regex.IsMatch(password!, @"^[a-zA-Z0-9\-_.,+]+\z"))
        {
            return new Result(false, "invalid: password");
        }

        return new Result(true);
    }
}

public record Result(bool ok, string? error = null);
public record UserAccount(string AccountId, string username, string passwordHash, string email, string status);

public record UserSession(string AccountId, string username, string email);
public record LoginResult(ErrorTypes errorType, string? error = null, UserSession? UserSession = null);

public enum ErrorTypes
{
    OK, Validation, Auth
}
