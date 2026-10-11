using CleanCodeCSharp.Examples.Testing;

namespace CleanCodeCSharp.Tests.Examples.Testing;

public class FibonacciTest
{
    [Test]
    [Ignore("todo")]
    public void ItShouldYield0ForIndex0()
    {
        // Arrange  ("given")
        var fibonacci = new Fibonacci();
        int index = 0;
        int expected = 0;

        // Act      ("when")
        int actual = fibonacci.Calc(index);

        // Assert   ("then")
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    [Ignore("todo")]
    public void ItShouldYield1ForIndex1()
    {
    }

    [Test]
    [Ignore("todo")]
    public void ItShouldYield1ForIndex2()
    {
    }

    [Test]
    [Ignore("todo")]
    public void ItShouldYield2ForIndex3()
    {
    }

    [Test]
    [Ignore("todo")]
    public void ItShouldYield8ForIndex6()
    {
    }

    [Test]
    [Ignore("todo")]
    public void ItShouldYield102334155ForIndex40()
    {
    }

    [Test]
    [Ignore("todo")]
    public void ItShouldThrowForNegativeIndex()
    {
        // Arrange  ("given")
        var fibonacci = new Fibonacci();
        int index = -1;
        string expected = "???";

        // Act      ("when"): only DECLARE the action, NUnit will call it
        TestDelegate action = () => fibonacci.Calc(index);

        // Assert   ("then")
        var e = Assert.Throws<ArgumentOutOfRangeException>(action);
        Assert.That(e.Message, Does.StartWith(expected));

        // alternative syntax (1 step):
        Assert.That(
            () => fibonacci.Calc(index),
            Throws.TypeOf<ArgumentOutOfRangeException>().With.Message.StartsWith(expected)
        );
    }

    [Test]
    [Ignore("todo")]
    public void ItShouldThrowForIndexGreaterThan46()
    {
    }
}
