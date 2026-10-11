using System.Text;
using CleanCodeCSharp.Examples.Classes;

namespace CleanCodeCSharp.Examples.Functions;

public class Functions
{
    // output arg
    public void AppendFullName(string firstName, string lastName, StringBuilder stringBuilder)
    {
        stringBuilder.Append(firstName).Append(' ').Append(lastName);
    }

    // unclean with 4 args
    public static void PrintAddressLine1(
        Employee employee,
        Func<string, string, string?> translationProvider,
        string language,
        Action<string> printConsumer
    )
    {
        var translation = translationProvider(language, employee.Position ?? "");
        printConsumer(translation == null ? $"{employee.Name}" : $"{employee.Name} - {translation}");
    }

    // option a: extracted to a class, some args moved to the constructor
    public class EmployeePrinter2(Func<string, string, string?> translationProvider, string language)
    {
        public void PrintAddressLine(Employee employee, Action<string> printConsumer)
        {
            var translation = translationProvider(language, employee.Position ?? "");
            printConsumer(translation == null ? $"{employee.Name}" : $"{employee.Name} - {translation}");
        }
    }

    // option b: currying, i.e. functions returning functions, each of which has only 1 arg
    public Func<Employee, Action<Action<string>>> PrintAddressLine3(Func<string, string?> translationProvider)
    {
        return employee => printConsumer =>
        {
            var translation = translationProvider(employee.Position ?? "");
            printConsumer(translation == null ? $"{employee.Name}" : $"{employee.Name} - {translation}");
        };
    }

    // option a again: this time all "static" args moved to the constructor
    public class EmployeePrinter4(Action<string> printConsumer, Func<string, string?> translationProvider)
    {
        public void PrintAddressLine(Employee employee)
        {
            var translation = translationProvider(employee.Position ?? "");
            printConsumer(translation == null ? $"{employee.Name}" : $"{employee.Name} - {translation}");
        }
    }
}
