namespace CleanCodeCSharp.Examples.Dao;

public class LoginService(IPersonDao dao)
{
    public UserSession Login(Credentials? credentials)
    {
        Validate(credentials);

        var person = GetPersonFromDb(credentials!.UserName!);
        if (person?.PasswordHash != HashPassword(credentials.Password!))
        {
            throw new ArgumentException("Unknown user or wrong password");
        }
        return new UserSession();
    }

    internal Person? GetPersonFromDb(string userName)
    {
        try
        {
            return dao.Get(userName);
        }
        catch (DaoException e)
        {
            throw new InternalServerException("oops, database error", e);
        }
    }

    internal string HashPassword(string password)
    {
        return password; // ToDo: hash it
    }

    internal void Validate(Credentials? credentials)
    {
        if (credentials == null)
        {
            throw new ArgumentException("No credentials provided");
        }
        if (credentials.UserName == null)
        {
            throw new ArgumentException("No username provided");
        }
        if (credentials.Password == null)
        {
            throw new ArgumentException("No password provided");
        }
    }
}
