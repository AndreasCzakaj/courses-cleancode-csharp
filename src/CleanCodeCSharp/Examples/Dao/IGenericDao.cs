namespace CleanCodeCSharp.Examples.Dao;

public interface IGenericDao<TId, TEntity> where TId : notnull
{
    /// <exception cref="DaoException">if the storage is not available</exception>
    TEntity? Get(TId uid);

    /// <exception cref="DaoException">if the storage is not available</exception>
    TEntity Save(TEntity entity);
}
