namespace CleanCodeTraining;

public class Vertical {
    public string? stuff;

    void Two(User user, Company company) {
        if (user == null) {
            if (company == null) {
                stuff = "default";
                return;
            } 
            stuff = company.name + " employee";
            return;
        }

        if (user.salary < 100_000) {
            stuff = "staff";
            return;
        }
        stuff = "manager";
    }
}

class User {
    public int salary = 666;
}

class Company {
    public string name = "";
}