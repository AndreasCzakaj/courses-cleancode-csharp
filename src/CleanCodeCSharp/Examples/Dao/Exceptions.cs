namespace CleanCodeCSharp.Examples.Dao;

public class DaoException(string message) : Exception(message);

public class InternalServerException(string message, Exception cause) : Exception(message, cause);
