using CleanCodeCSharp.Examples.Dao;

namespace CleanCodeCSharp.Tests.Examples.Dao;

public class SignUpServiceTest
{
    static readonly Person ExistingPerson = new(
        UserName: "i_already_exist",
        PasswordHash: "password1",
        Email: "i_already_exist@acme.com"
    );

    static readonly Person NewPerson = new(
        UserName: "i_am_new",
        PasswordHash: "password2",
        Email: "i_am_new@acme.com"
    );

    GenericDaoMapImpl<string, Person> dao = null!;
    SignUpService sut = null!;

    [SetUp]
    public void SetUp()
    {
        dao = new GenericDaoMapImpl<string, Person>(person => person.UserName);
        dao.Save(ExistingPerson);
        sut = new SignUpService(dao);
    }

    [Test]
    public void ShouldFailForDuplicateUserNames()
    {
        // given
        var signUpData = new SignUpData(ExistingPerson.UserName);

        // when
        TestDelegate action = () => sut.Process(signUpData);

        // then
        var e = Assert.Throws<ArgumentException>(action);
        Assert.That(e.Message, Does.Contain("userName: already exists"));
    }

    [Test]
    public void ShouldSignUpNewPerson()
    {
        // given
        var signUpData = new SignUpData(NewPerson.UserName, NewPerson.PasswordHash, NewPerson.Email);

        // when
        var result = sut.Process(signUpData);

        // then
        Assert.That(result.Person, Is.EqualTo(NewPerson));
        Assert.That(dao.Get(NewPerson.UserName), Is.EqualTo(NewPerson));
    }
}
