namespace CleanCodeCSharp.Examples.Oop.Fcoi.Composition;

// Strategy for bonuses
public interface IBonusCalculator
{
    double CalculateBonus(double basePay);
}

public class StandardBonus : IBonusCalculator
{
    public double CalculateBonus(double basePay) => basePay * 0.10;
}

public class ManagerBonus : IBonusCalculator
{
    public double CalculateBonus(double basePay) => basePay * 0.20;
}

public class NoBonus : IBonusCalculator
{
    public double CalculateBonus(double basePay) => 0;
}

// Strategy for compensation
public interface ICompensationStrategy
{
    double BasePay { get; }
    double CalculatePay(int hoursWorked);
}

public class SalariedCompensation(double annualSalary) : ICompensationStrategy
{
    public double BasePay => annualSalary;

    public double CalculatePay(int hoursWorked) => annualSalary / 12; // Monthly pay
}

public class HourlyCompensation(double hourlyRate) : ICompensationStrategy
{
    public double BasePay => hourlyRate * 2080; // Approximate annual

    public double CalculatePay(int hoursWorked) => hourlyRate * hoursWorked;
}

// Employee uses composition
public class Employee(string name, ICompensationStrategy compensation, IBonusCalculator bonusCalculator)
{
    public double CalculateBonus()
    {
        return bonusCalculator.CalculateBonus(compensation.BasePay);
    }

    public double CalculatePay(int hoursWorked)
    {
        return compensation.CalculatePay(hoursWorked);
    }

    public void Work()
    {
        Console.WriteLine($"{name} is working");
    }

    // Can change strategies at runtime!
    public void PromoteToManager()
    {
        bonusCalculator = new ManagerBonus();
    }

    public void ConvertToContractor(double hourlyRate)
    {
        compensation = new HourlyCompensation(hourlyRate);
        bonusCalculator = new NoBonus();
    }
}

public static class Usage
{
    public static readonly Employee John = new("John", new SalariedCompensation(60_000), new StandardBonus());

    public static readonly Employee Jane = new("Jane", new HourlyCompensation(50), new NoBonus());

    public static readonly Employee Bob = new("Bob", new SalariedCompensation(90_000), new ManagerBonus());
}
