namespace CleanCodeCSharp.Examples.Dao;

public class GenericDaoMapImpl<TId, TEntity>(Func<TEntity, TId> uidProvider) : IGenericDao<TId, TEntity>
    where TId : notnull
{
    public Dictionary<TId, TEntity> Repo { get; } = new();

    public TEntity? Get(TId uid)
    {
        return Repo.GetValueOrDefault(uid);
    }

    public TEntity Save(TEntity entity)
    {
        Repo[uidProvider(entity)] = entity;
        return entity;
    }
}
