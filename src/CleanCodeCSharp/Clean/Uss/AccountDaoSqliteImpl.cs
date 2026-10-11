using Microsoft.Data.Sqlite;

namespace CleanCodeCSharp.Clean.Uss;

public class AccountDaoSqliteImpl(string connectionString) : IAccountDao
{
    public async Task<Account?> FindByUsername(string username)
    {
        try
        {
            await using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText =
                "SELECT id, username, password_hash, email, tc_accepted, status FROM accounts WHERE username = $username";
            command.Parameters.AddWithValue("$username", username);

            await using var reader = await command.ExecuteReaderAsync();
            return await reader.ReadAsync() ? ToAccount(reader) : null;
        }
        catch (SqliteException e)
        {
            throw new DaoException("SQLite Error", e);
        }
    }

    /// <summary>Not part of the interface: only the setup (dev server, integration tests) saves accounts.</summary>
    public async Task Save(Account account)
    {
        await Execute(
            "INSERT INTO accounts VALUES ($id, $username, $passwordHash, $email, $tcAccepted, $status)",
            ("$id", account.Id),
            ("$username", account.Username),
            ("$passwordHash", account.PasswordHash),
            ("$email", account.Email),
            ("$tcAccepted", account.TcAccepted.ToString("O")),
            ("$status", account.Status.ToString())
        );
    }

    public async Task CreateTable()
    {
        await Execute("""
            CREATE TABLE accounts (
                id TEXT PRIMARY KEY,
                username TEXT NOT NULL UNIQUE,
                password_hash TEXT NOT NULL,
                email TEXT NOT NULL,
                tc_accepted TEXT NOT NULL,
                status TEXT NOT NULL
            )
            """);
    }

    private static Account ToAccount(SqliteDataReader reader)
    {
        return new Account(
            Id: reader.GetString(0),
            Username: reader.GetString(1),
            PasswordHash: reader.GetString(2),
            Email: reader.GetString(3),
            TcAccepted: DateTimeOffset.Parse(reader.GetString(4)),
            Status: Enum.Parse<AccountStatus>(reader.GetString(5), ignoreCase: true)
        );
    }

    private async Task Execute(string sql, params (string Name, string Value)[] parameters)
    {
        try
        {
            await using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = sql;
            foreach (var (name, value) in parameters)
            {
                command.Parameters.AddWithValue(name, value);
            }
            await command.ExecuteNonQueryAsync();
        }
        catch (SqliteException e)
        {
            throw new DaoException("SQLite Error", e);
        }
    }
}
