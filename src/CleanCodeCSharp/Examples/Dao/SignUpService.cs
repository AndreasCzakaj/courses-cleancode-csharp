namespace CleanCodeCSharp.Examples.Dao;

public class SignUpService(IGenericDao<string, Person> dao)
{
    public SignUpResult Process(SignUpData signUpData)
    {
        ValidateSignUpData(signUpData);

        Person? maybeExistingPerson = GetPersonFromStorage(signUpData.UserName);
        if (maybeExistingPerson != null)
        {
            throw new ArgumentException("userName: already exists");
        }
        var person = new Person(
            UserName: signUpData.UserName,
            PasswordHash: signUpData.Password ?? "", // ToDo: hash it
            Email: signUpData.Email
        );
        Person savedPerson = SavePersonToStorage(person);
        return new SignUpResult(savedPerson);
    }

    internal Person? GetPersonFromStorage(string userName)
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

    internal Person SavePersonToStorage(Person person)
    {
        try
        {
            return dao.Save(person);
        }
        catch (DaoException e)
        {
            throw new InternalServerException("oops, database error", e);
        }
    }

    internal void ValidateSignUpData(SignUpData signUpData)
    {
        // check data... throw ArgumentException if needed
    }
}
