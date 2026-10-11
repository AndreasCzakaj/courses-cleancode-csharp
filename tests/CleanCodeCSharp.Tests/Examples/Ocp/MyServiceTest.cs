using CleanCodeCSharp.Examples.Ocp;

namespace CleanCodeCSharp.Tests.Examples.Ocp;

public class MyServiceTest
{
    MyService sut = null!;

    [SetUp]
    public void SetUp()
    {
        sut = new MyService();
    }

    [Test]
    public void ShouldDoMagic()
    {
        Assert.That(sut.Magic(null), Is.EqualTo("Expelliarmus!"));
    }
}
