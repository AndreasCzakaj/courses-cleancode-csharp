using CleanCodeCSharp.Clean.FunWithFlags;

namespace CleanCodeCSharp.Tests.Clean.FunWithFlags;

public class UuidGeneratorTest
{
    [TestCaseSource(nameof(ShouldCreateAUuidInTheMatchingFormatParams))]
    public void ShouldCreateAUuidInTheMatchingFormat(IUuidGenerator uuidGenerator, string expectedRegex)
    {
        // when
        var actual = uuidGenerator.Create();

        // then
        Assert.That(actual, Does.Match($"^{expectedRegex}$"));
    }

    static IEnumerable<TestCaseData> ShouldCreateAUuidInTheMatchingFormatParams()
    {
        var baseImpl = new UuidGeneratorNaiveRandomImpl();

        yield return new TestCaseData(baseImpl, "[a-f0-9]{32}")
            .SetName("it should match the pattern for case: lower case, no dashes");
        //yield return new TestCaseData(new ???, "[A-F0-9]{32}")
        //    .SetName("it should match the pattern for case: upper case, no dashes");
        //yield return new TestCaseData(new ???, "[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}")
        //    .SetName("it should match the pattern for case: lower case, with dashes");
        //yield return new TestCaseData(new ???, "[A-F0-9]{8}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{12}")
        //    .SetName("it should match the pattern for case: upper case, with dashes");
    }

    [Test]
    public void ShouldUseAllChars()
    {
        var hexChars = "0123456789abcdef".ToCharArray();
        IUuidGenerator uuidGenerator = new UuidGeneratorNaiveRandomImpl();

        // yes, I'm generating many UUIDs. I need this because the process is random.
        var foundChars = Enumerable.Range(0, 10)
            .SelectMany(_ => uuidGenerator.Create())
            .Distinct();

        Assert.That(foundChars, Is.EquivalentTo(hexChars));
    }
}
