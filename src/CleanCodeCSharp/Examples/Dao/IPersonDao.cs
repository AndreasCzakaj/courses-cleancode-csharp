namespace CleanCodeCSharp.Examples.Dao;

public interface IPersonDao
{
    /// <exception cref="DaoException">if the storage is not available</exception>
    Person? Get(string userName);
}
