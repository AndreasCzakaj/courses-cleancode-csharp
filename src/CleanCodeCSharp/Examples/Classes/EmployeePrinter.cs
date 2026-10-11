namespace CleanCodeCSharp.Examples.Classes;

public class EmployeePrinter
{
    public void Print1(IEnumerable<Employee?> employees, Action<string> lineConsumer)
    {
        foreach (var employee in employees.OfType<Employee>())
        {
            lineConsumer(ToLine1(employee));
        }
    }

    // private: can only be tested indirectly via Print1
    private string ToLine1(Employee employee)
    {
        return $"{employee.Name ?? "anonymous"} ({employee.Position ?? "n.a."})";
    }

    public void Print2(IEnumerable<Employee?> employees, Action<string> lineConsumer)
    {
        foreach (var employee in employees.OfType<Employee>())
        {
            lineConsumer(ToLine2(employee));
        }
    }

    // internal: can be tested directly (see InternalsVisibleTo in the .csproj file)
    internal string ToLine2(Employee employee)
    {
        return $"{employee.Name ?? "anonymous"} ({employee.Position ?? "n.a."})";
    }
}
