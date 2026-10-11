using Microsoft.Data.Sqlite;

namespace CleanCodeCSharp.Clean.Uss;

// Starts the user self service for local use: `dotnet run --project src/CleanCodeCSharp`
// The database is a SQLite database in memory, filled with the accounts below.
public static class Dev
{
    private static readonly (string Username, string Password, AccountStatus Status)[] Accounts =
    [
        ("alice_verified", "Correct-Horse_42", AccountStatus.Verified),
        ("bob_not_verified", "Battery.Staple+7", AccountStatus.New),
    ];

    public static async Task Main()
    {
        var connectionString = $"Data Source=uss-{Guid.NewGuid()};Mode=Memory;Cache=Shared";

        // a database in memory lives as long as at least 1 connection is open
        await using var keepAlive = new SqliteConnection(connectionString);
        await keepAlive.OpenAsync();

        var accountDao = new AccountDaoSqliteImpl(connectionString);
        await accountDao.CreateTable();
        foreach (var (username, password, status) in Accounts)
        {
            await accountDao.Save(new Account(
                Id: Guid.NewGuid().ToString(),
                Username: username,
                PasswordHash: Password.Hash(password),
                Email: $"{username}@example.com",
                TcAccepted: DateTimeOffset.UtcNow,
                Status: status
            ));
        }

        Console.WriteLine("SQLite in memory: " + connectionString);
        foreach (var account in Accounts)
        {
            Console.WriteLine(account);
        }

        var port = int.Parse(Environment.GetEnvironmentVariable("PORT") ?? "3000");
        await using var server = await Server.Run(new ServerConfig(port, connectionString));
        await server.WaitForShutdown();
    }
}
