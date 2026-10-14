namespace CleanCodeCSharp.Examples.Dao;

public class SignUpService(IGenericDao<string, Person> dao)
{
    public SignUpResult Process(SignUpData signUpData)
    {
        //ValidateSignUpData(signUpData);
        throw new NotImplementedException();
    }
}
