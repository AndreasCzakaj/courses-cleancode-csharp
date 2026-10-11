using CleanCodeCSharp.Dirty.Testing;

namespace CleanCodeCSharp.Tests.Dirty.Testing;

public class PersonServiceTest
{
    PersonService sut = null!;

    [SetUp]
    public void SetUp()
    {
        sut = new PersonService();
    }

    [Test]
    public void ShouldSignUpValidPerson()
    {
        // given
        var signUpData = new SignUpData();
        signUpData.FirstName = "CJ";
        signUpData.LastName = "Ramone";

        // when
        var userAccount = sut.SignUp(signUpData);

        // then
        Assert.That(userAccount.FirstName, Is.EqualTo(signUpData.FirstName));
        Assert.That(userAccount.LastName, Is.EqualTo(signUpData.LastName));
        Assert.That(userAccount.State, Is.EqualTo("new"));
        Assert.That(userAccount.Id, Does.Match("^[0-9]{1,1}$"));
    }

    [Test]
    public void ShouldThrowErrorWhenSigningUpInvalidPerson()
    {
        // given
        var signUpData = new SignUpData();
        signUpData.FirstName = "P";
        signUpData.LastName = "Ramone";

        // when
        TestDelegate action = () => sut.SignUp(signUpData);

        // then
        var ex = Assert.Throws<ArgumentException>(action);
        Assert.That(ex.Message, Is.EqualTo("First name must be at least 2 characters long"));
    }
}
