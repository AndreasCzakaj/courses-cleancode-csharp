namespace CleanCodeCSharp.Dirty.Testing;

public class PersonService
{
    public UserAccount SignUp(SignUpData signUpData)
    {
        if (signUpData == null)
        {
            throw new ArgumentException("Data must not be null");
        }
        if ((signUpData.FirstName?.Length ?? 0) < 2)
        {
            throw new ArgumentException("First name must be at least 2 characters long");
        }

        UserAccount userAccount = new UserAccount();
        userAccount.FirstName = signUpData.FirstName;
        userAccount.LastName = signUpData.LastName;
        userAccount.State = "new";
        userAccount.Id = "1";
        return userAccount;
    }
}

public class SignUpData
{
    public string? FirstName { get; set; } = "";
    public string? LastName { get; set; } = "";
}

public class UserAccount : SignUpData
{
    public string Id { get; set; } = "";
    public string State { get; set; } = "";
}
