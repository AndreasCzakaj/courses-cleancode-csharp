using CleanCodeCSharp.Clean.Uss;

namespace CleanCodeCSharp.Tests.Clean.Uss;

public static class TestUtils
{
    public const string ValidPassword = "Correct-Horse_42";
    public const string ValidButWrongPassword = "Battery.Staple+7";

    // hashing is slow on purpose => once for all tests
    private static readonly string ValidPasswordHash = Password.Hash(ValidPassword);

    // The "creators" return valid objects.
    // A test then breaks them in 1 place (`with { ... }`), so it tests for 1 error.

    public static Account CreateVerifiedAccount()
    {
        return new Account(
            Id: "0b0e7a4c-8d2f-4c3a-9a51-6f1f3c1d2e01",
            Username: "alice_verified",
            PasswordHash: ValidPasswordHash,
            Email: "alice@example.com",
            TcAccepted: DateTimeOffset.Parse("2026-10-01T08:00:00Z"),
            Status: AccountStatus.Verified
        );
    }

    public static Account CreateNotVerifiedAccount()
    {
        return CreateVerifiedAccount() with
        {
            Id = "0b0e7a4c-8d2f-4c3a-9a51-6f1f3c1d2e02",
            Username = "bob_not_verified",
            Email = "bob@example.com",
            Status = AccountStatus.New,
        };
    }

    public static Credentials CreateValidCredentials()
    {
        return new Credentials(CreateVerifiedAccount().Username, ValidPassword);
    }

    public static Credentials CreateValidCredentialsNotVerifiedAccount()
    {
        return new Credentials(CreateNotVerifiedAccount().Username, ValidPassword);
    }

    public static Credentials CreateValidCredentialsUnknownUser()
    {
        return new Credentials("idonotexist", ValidPassword);
    }

    public static UserSelfService CreateUserSelfServiceWithWorkingDeps()
    {
        var accountDao = new AccountDaoDictionaryImpl(CreateVerifiedAccount(), CreateNotVerifiedAccount());
        return new UserSelfService(accountDao);
    }
}
