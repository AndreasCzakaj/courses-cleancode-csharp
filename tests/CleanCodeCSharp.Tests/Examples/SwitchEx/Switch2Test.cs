using CleanCodeCSharp.Examples.SwitchEx;

namespace CleanCodeCSharp.Tests.Examples.SwitchEx;

public class Switch2Test
{
    List<string> collector = null!;
    Switch2 sut = null!;

    [SetUp]
    public void SetUp()
    {
        sut = new Switch2();
        collector = new List<string>();
    }

    [TestCase("", "plitsch, platsch")]
    [TestCase("unknown", "plitsch, platsch")]
    [TestCase("wind", "blow'em away!")]
    [TestCase("laser", "burn'em!")]
    [Ignore("todo")]
    public void UseForce(string power, string expected)
    {
        //sut.UseForce(power, collector.Add);
        Assert.That(collector, Is.EqualTo(new[] { expected }));
    }
}
