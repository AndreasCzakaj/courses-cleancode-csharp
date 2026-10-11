using CleanCodeCSharp.Examples.Dao;

namespace CleanCodeCSharp.Tests.Examples.Dao;

// a fake: simple and real, no mocking library needed
public class PersonDaoMapImpl : IPersonDao
{
    public Dictionary<string, Person> Repo { get; } = new();

    public virtual Person? Get(string userName)
    {
        return Repo.GetValueOrDefault(userName);
    }
}

public class PersonDaoThrowingImpl : IPersonDao
{
    public Person? Get(string userName)
    {
        throw new DaoException("whatever");
    }
}
