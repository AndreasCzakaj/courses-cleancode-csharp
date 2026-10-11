using CleanCodeCSharp.Examples.Classes;

namespace CleanCodeCSharp.Tests.Examples.Classes;

public class EmployeePrinterTest
{
    List<string> collector = null!;
    EmployeePrinter sut = null!;

    [SetUp]
    public void SetUp()
    {
        collector = new List<string>();
        sut = new EmployeePrinter();
    }

    // "private" ToLine1 can only be tested indirectly via Print1
    [TestCaseSource(nameof(ToLineParams))]
    public void Print1One(Employee employee, string expected)
    {
        sut.Print1([employee], collector.Add);
        Assert.That(collector, Is.EqualTo(new[] { expected }));
    }

    // "internal" ToLine2 can be tested directly
    [TestCaseSource(nameof(ToLineParams))]
    public void ToLine2(Employee employee, string expected)
    {
        Assert.That(sut.ToLine2(employee), Is.EqualTo(expected));
    }

    static object[][] ToLineParams() =>
    [
        [new Employee(Name: "n1", Position: "p1"), "n1 (p1)"],
        [new Employee(Name: null, Position: "p1"), "anonymous (p1)"],
        [new Employee(Name: "n1", Position: null), "n1 (n.a.)"],
        [new Employee(Name: null, Position: null), "anonymous (n.a.)"],
    ];
}
