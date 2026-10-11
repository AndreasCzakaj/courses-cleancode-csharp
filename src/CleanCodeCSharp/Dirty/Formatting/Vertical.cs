namespace CleanCodeCSharp.Dirty.Formatting;

public class Vertical
{
    public string? Stuff { get; private set; }

    public void Two(User? user, Company? company)
    {
        if (user == null)
        {
            if (company == null)
            {
                Stuff = "default";
                return;
            }
            Stuff = company.Name + " employee";
            return;
        }

        if (user.Salary < 100_000)
        {
            Stuff = "staff";
            return;
        }
        Stuff = "manager";
    }
}

public class User
{
    public int Salary { get; set; } = 666;
}

public class Company
{
    public string Name { get; set; } = "";
}
