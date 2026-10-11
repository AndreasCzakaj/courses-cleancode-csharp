using CleanCodeCSharp.Examples.Classes;
using CleanCodeCSharp.Examples.Functions;
using FunctionsExample = CleanCodeCSharp.Examples.Functions.Functions;

namespace CleanCodeCSharp.Tests.Examples.Functions;

public class FunctionsTest
{
    static readonly Dictionary<string, Dictionary<string, string>> Translations = new()
    {
        ["en"] = new() { ["gf"] = "Managing Director" },
        ["de"] = new() { ["gf"] = "Geschäftsführer" },
    };

    static readonly string[] ExpectedLines = ["n1 - Managing Director", "n2 - Geschäftsführer"];

    static string? Translate(string language, string key) => Translations[language].GetValueOrDefault(key);

    FunctionsExample sut = null!;
    List<string> collector = null!;

    [SetUp]
    public void SetUp()
    {
        sut = new FunctionsExample();
        collector = new List<string>();
    }

    [Test]
    public void PrintAddressLine1()
    {
        FunctionsExample.PrintAddressLine1(new Employee("n1", "gf"), Translate, "en", collector.Add);
        FunctionsExample.PrintAddressLine1(new Employee("n2", "gf"), Translate, "de", collector.Add);

        Assert.That(collector, Is.EqualTo(ExpectedLines));
    }

    [Test]
    public void PrintAddressLine2()
    {
        var printerEn = new FunctionsExample.EmployeePrinter2(Translate, "en");
        var printerDe = new FunctionsExample.EmployeePrinter2(Translate, "de");

        printerEn.PrintAddressLine(new Employee("n1", "gf"), collector.Add);
        printerDe.PrintAddressLine(new Employee("n2", "gf"), collector.Add);

        Assert.That(collector, Is.EqualTo(ExpectedLines));
    }

    [Test]
    public void PrintAddressLine3()
    {
        var printEn = sut.PrintAddressLine3(key => Translate("en", key));
        var printDe = sut.PrintAddressLine3(key => Translate("de", key));

        printEn(new Employee("n1", "gf"))(collector.Add);
        printDe(new Employee("n2", "gf"))(collector.Add);

        Assert.That(collector, Is.EqualTo(ExpectedLines));
    }

    [Test]
    public void PrintAddressLine4()
    {
        var printerEn = new FunctionsExample.EmployeePrinter4(collector.Add, key => Translate("en", key));
        var printerDe = new FunctionsExample.EmployeePrinter4(collector.Add, key => Translate("de", key));

        printerEn.PrintAddressLine(new Employee("n1", "gf"));
        printerDe.PrintAddressLine(new Employee("n2", "gf"));

        Assert.That(collector, Is.EqualTo(ExpectedLines));
    }
}
