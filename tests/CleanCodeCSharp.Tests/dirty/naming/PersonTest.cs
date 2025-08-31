using NUnit.Framework;

namespace CleanCode.Naming.Tests;

[TestFixture]
public class PersonTest {

    [Test]
    public void Person() {
        var person = new Person();
        person.coreFirstName = "Kim";
        person.coreLastName = "Gordon";
        person.coreState = "active";
        person.addrStreetAndNo = "Penny Lane";
        person.addrZipAndCity = "Beverly Bills, 90210";
        person.addrState = "California";

        Assert.That(person.coreFirstName, Is.EqualTo("Kim"));
        Assert.That(person.coreLastName, Is.EqualTo("Gordon"));
        Assert.That(person.coreState, Is.EqualTo("active"));
        Assert.That(person.addrStreetAndNo, Is.EqualTo("Penny Lane"));
        Assert.That(person.addrZipAndCity, Is.EqualTo("Beverly Bills, 90210"));
        Assert.That(person.addrState, Is.EqualTo("California"));
    }
}