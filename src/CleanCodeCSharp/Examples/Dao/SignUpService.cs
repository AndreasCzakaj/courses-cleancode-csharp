using System.Text.RegularExpressions;

namespace CleanCodeCSharp.Examples.Dao;

public class SignUpService(IGenericDao<string, Person> dao)
{
    public SignUpResult Process(SignUpData signUpData)
    {
        // 1. validate signUpData -> reuse tests from LOGIN feature!
        var validationResult = ValidateSignUpData(signUpData);

        // 2. check if user already exists
        // 2.1 try to get user from DB by username
        // a: has data: DUPLICATE username -> error
        // else: username noch frei

        // 3. in DB speichern

        // (email verification per token & email)

        throw new NotImplementedException();
    }

    private Result ValidateSignUpData(SignUpData signUpData)
    {
        throw new NotImplementedException();
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
