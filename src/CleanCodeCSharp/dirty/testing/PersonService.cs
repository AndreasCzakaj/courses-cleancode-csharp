namespace CleanCode.Testing;

public class PersonService {
    public UserAccount SignUp(SignUpData signUpData) {
        if ((signUpData?.firstName?.Length ?? 0) < 2) {
            throw new ArgumentException("First name must be at least 2 characters long");
        }

        UserAccount userAccount = new UserAccount();
        userAccount.firstName = signUpData.firstName;
        userAccount.lastName = signUpData.lastName;
        userAccount.state = "new";
        userAccount.id = "1";
        return userAccount;
    }
}

public class SignUpData {
    public string firstName = "";
    public string lastName = "";
}

public class UserAccount : SignUpData {
    public string id = "";
    public string state = "";
}