namespace CleanCodeCSharp.Clean.Uss;

// an interface on purpose: the service is isolated from the database,
// and the implementations are interchangeable
public interface IAccountDao
{
    Task<Account?> FindByUsername(string username);
}

public class DaoException(string message, Exception? cause = null) : Exception(message, cause);
