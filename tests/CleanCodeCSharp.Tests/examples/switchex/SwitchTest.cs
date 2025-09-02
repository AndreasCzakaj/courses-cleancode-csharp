using System.Collections;
using NUnit.Framework;

namespace CleanCode.SwitchEx.Tests;

[TestFixture]
public class SwitchTest {
    Switch sut;

    [SetUp]
    public void SetUp() {
        sut = new Switch();
    }

    [TestCaseSource(nameof(ShouldGetHeroParams))]
    public void ShouldGetHero2(string power, string expected) {
        Assert.That(sut.GetHero(power), Is.EqualTo(expected));
    }

    static IEnumerable ShouldGetHeroParams() {
        var sut = new Switch();
        return new[] {
            new TestCaseData("", "Aquaman")
                .SetName("it should yield Aquaman for input "),
            new TestCaseData("unknown", "Aquaman")
                .SetName("it should yield Aquaman for input unknown"),
            new TestCaseData("wind", "Storm")
                .SetName("it should yield Storm for input wind"),
            new TestCaseData("laser", "Cyclops")
                .SetName("it should yield Cyclops for input laser")
        };
    }
}