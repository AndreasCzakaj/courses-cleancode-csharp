namespace CleanCodeCSharp.Examples.Oop.Fcoi.Inheritance;

public class Employee(string name, double salary)
{
    // we need to expose data for subclasses
    // -> parent is coupled to children
    protected double Salary { get; } = salary;

    public virtual double CalculateBonus()
    {
        return Salary * 0.10; // 10% bonus
    }

    public void Work()
    {
        Console.WriteLine($"{name} is working");
    }
}

public class Manager(string name, double salary) : Employee(name, salary)
{
    public override double CalculateBonus()
    {
        return Salary * 0.20; // 20% bonus for managers
    }

    public void ConductMeeting()
    {
        Console.WriteLine("Conducting meeting");
    }
}

public class Contractor(string name, double hourlyRate) : Employee(name, hourlyRate)
{
    public override double CalculateBonus()
    {
        return 0; // Contractors don't get bonuses
    }

    // Problem: Contractors shouldn't have a salary!
    // They have hourly rates, but we're forced into Employee's model
}
