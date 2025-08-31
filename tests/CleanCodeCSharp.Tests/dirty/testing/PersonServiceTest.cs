using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace CleanCode.Testing.Tests;

[TestFixture]
public class PersonServiceTest {
    PersonService sut;

    [SetUp]
    public void SetUp() {
        sut = new PersonService();
    }

    [Test]
    public void ShouldSignUpValidPerson() {
        // given
        var signUpData = new SignUpData();
        signUpData.firstName = "CJ";
        signUpData.lastName = "Ramone";

        // when
        var userAccount = sut.SignUp(signUpData);

        // then
        Assert.That(userAccount.firstName, Is.EqualTo(signUpData.firstName));
        Assert.That(userAccount.lastName, Is.EqualTo(signUpData.lastName));
        Assert.That(userAccount.state, Is.EqualTo("new")); // or expected.coreState
        Assert.That(userAccount.id, Does.Match(new Regex("[0-9]{1,1}")));
    }

    [Test]
    public void ShouldThrowErrorWhenSigningUpInvalidPerson() {
        // given
        var signUpData = new SignUpData();
        signUpData.firstName = "P";
        signUpData.lastName = "Ramone";

        // when
        TestDelegate action = () => sut.SignUp(signUpData);

        // then
        var ex = Assert.Throws<ArgumentException>(action);
        Assert.That(ex.Message, Is.EqualTo("First name must be at least 2 characters long"));
    }
}