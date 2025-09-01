namespace CleanCodeTraining;

public class Vertical {
    void Two(User user, Company company) {
        string stuff;
        if (user == null) {
            if (company == null) {
                stuff = "default";
            }
            stuff = company.name + " employee";
        }
        if (user.salary < 100_000) {
            stuff = "staff";
        }
        stuff = "manager";
    }
}

class User {
    public int salary = 666;
}

class Company {
    public string name;
}