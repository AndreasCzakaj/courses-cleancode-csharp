using CleanCodeCSharp.Clean.Uss;
using Microsoft.Data.Sqlite;

namespace CleanCodeCSharp.Tests.Clean.Uss.Integration;

/// <summary>A SQLite database in memory with the table "accounts". Dispose it to delete it.</summary>
public sealed class SqliteTestDatabase : IDisposable
{
    // a database in memory lives as long as at least 1 connection is open
    private readonly SqliteConnection keepAlive;

    public string ConnectionString { get; } = $"Data Source=uss-test-{Guid.NewGuid()};Mode=Memory;Cache=Shared";

    private SqliteTestDatabase()
    {
        keepAlive = new SqliteConnection(ConnectionString);
        keepAlive.Open();
    }

    public static async Task<SqliteTestDatabase> Create(params Account[] accounts)
    {
        var database = new SqliteTestDatabase();
        var accountDao = new AccountDaoSqliteImpl(database.ConnectionString);
        await accountDao.CreateTable();
        foreach (var account in accounts)
        {
            await accountDao.Save(account);
        }
        return database;
    }

    public void Dispose()
    {
        keepAlive.Dispose();
    }
}
