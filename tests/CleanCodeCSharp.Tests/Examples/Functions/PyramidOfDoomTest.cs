using CleanCodeCSharp.Examples.Functions;

namespace CleanCodeCSharp.Tests.Examples.Functions;

public class PyramidOfDoomTest
{
    public interface IMyService
    {
        string GetCompanyName(Person? person);
    }

    class MyServiceDoomImpl : IMyService
    {
        public string GetCompanyName(Person? person)
        {
            if (person != null)
            {
                var company = person.Company;
                if (company != null)
                {
                    if (company.Name != null)
                    {
                        return company.Name;
                    }
                }
            }
            return "binary stars GmbH";
        }
    }

    // C#'s counterpart of Java's Optional chain: null-conditional (?.) and null-coalescing (??) operators
    class MyServiceNullConditionalImpl : IMyService
    {
        public string GetCompanyName(Person? person)
        {
            return person?.Company?.Name ?? "binary stars GmbH";
        }
    }

    [TestCaseSource(nameof(PyramidOfDoomParams))]
    public void PyramidOfDoom(IMyService impl, Person? person, string expected)
    {
        // when
        var actual = impl.GetCompanyName(person);

        // then
        Assert.That(actual, Is.EqualTo(expected));
    }

    static IEnumerable<TestCaseData> PyramidOfDoomParams()
    {
        var personWithoutCompany = new Person();
        var personWithoutCompanyName = new Person(new Company());
        var personWithCompanyNamedAcme = new Person(new Company("acme"));

        IMyService[] impls = [new MyServiceDoomImpl(), new MyServiceNullConditionalImpl()];
        foreach (var impl in impls)
        {
            yield return new TestCaseData(impl, null, "binary stars GmbH");
            yield return new TestCaseData(impl, personWithoutCompany, "binary stars GmbH");
            yield return new TestCaseData(impl, personWithoutCompanyName, "binary stars GmbH");
            yield return new TestCaseData(impl, personWithCompanyNamedAcme, "acme");
        }
    }
}
