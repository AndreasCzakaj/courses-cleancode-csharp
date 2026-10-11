using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;

namespace CleanCodeCSharp.Dirty.Uss;

// Starts the user self service for local use: `dotnet run --project src/CleanCodeCSharp`
// The database is a SQLite database in memory, filled with the accounts below.
public static class Dev
{
    private static readonly (string Username, string Password, string Status)[] Accounts =
    [
        ("alice_verified", "Correct-Horse_42", "verified"),
        ("bob_not_verified", "Battery.Staple+7", "new"),
    ];

    public static void Main()
    {
        var connectionString = $"Data Source=uss-{Guid.NewGuid()};Mode=Memory;Cache=Shared";

        // a database in memory lives as long as at least 1 connection is open
        using var keepAlive = new SqliteConnection(connectionString);
        keepAlive.Open();

        Execute(keepAlive, """
            CREATE TABLE accounts (
                id TEXT PRIMARY KEY,
                username TEXT NOT NULL UNIQUE,
                password_hash TEXT NOT NULL,
                email TEXT NOT NULL,
                tc_accepted TEXT NOT NULL,
                status TEXT NOT NULL
            )
            """);
        foreach (var (username, password, status) in Accounts)
        {
            Execute(keepAlive,
                "INSERT INTO accounts VALUES ($id, $username, $passwordHash, $email, $tcAccepted, $status)",
                ("$id", Guid.NewGuid().ToString()),
                ("$username", username),
                ("$passwordHash", Hash(password)),
                ("$email", $"{username}@example.com"),
                ("$tcAccepted", DateTimeOffset.UtcNow.ToString("O")),
                ("$status", status));
        }

        Console.WriteLine("SQLite in memory: " + connectionString);
        foreach (var account in Accounts)
        {
            Console.WriteLine(account);
        }

        Environment.SetEnvironmentVariable("USS_DB", connectionString);
        Server.Run();
    }

    // format: "<salt>:<hash>", both hex encoded
    private static string Hash(string password)
    {
        var salt = Convert.ToHexString(RandomNumberGenerator.GetBytes(16)).ToLower();
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password, Encoding.UTF8.GetBytes(salt), 210_000, HashAlgorithmName.SHA512, 64);
        return $"{salt}:{Convert.ToHexString(hash).ToLower()}";
    }

    private static void Execute(SqliteConnection connection, string sql, params (string Name, string Value)[] parameters)
    {
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        foreach (var (name, value) in parameters)
        {
            command.Parameters.AddWithValue(name, value);
        }
        command.ExecuteNonQuery();
    }
}
