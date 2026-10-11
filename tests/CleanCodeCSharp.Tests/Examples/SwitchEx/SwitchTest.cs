using CleanCodeCSharp.Examples.SwitchEx;

namespace CleanCodeCSharp.Tests.Examples.SwitchEx;

public class SwitchTest
{
    Switch sut = null!;

    [SetUp]
    public void SetUp()
    {
        sut = new Switch();
    }

    [TestCaseSource(nameof(ShouldGetHeroParams))]
    public void ShouldGetHero1(string power, string expected)
    {
        Assert.That(sut.GetHero(power), Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(ShouldGetHeroParams))]
    public void ShouldGetHero2(string power, string expected)
    {
        Assert.That(sut.GetHero2(power), Is.EqualTo(expected));
    }

    static object[][] ShouldGetHeroParams() =>
    [
        ["", "Aquaman"],
        ["unknown", "Aquaman"],
        ["wind", "Storm"],
        ["laser", "Cyclops"],
    ];
}
