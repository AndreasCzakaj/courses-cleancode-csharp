namespace CleanCodeTraining.Tests;

using System.Collections;
using System.Text.RegularExpressions;
using NUnit.Framework;

[TestFixture]
public class UuidGeneratorTest {

    [TestCaseSource(nameof(ShouldCreateAUuidInTheMatchingFormatParams))]
    public void ShouldCreateAUuidInTheMatchingFormat(UuidGenerator uuidGenerator, string expectedRegex, string info) {
        // when
        var actual = uuidGenerator.Create();

        // then
        Assert.That(actual, Does.Match(expectedRegex), $"Failed for case: {info}");
    }

    static IEnumerable ShouldCreateAUuidInTheMatchingFormatParams() {
        var baseImpl = new UuidGeneratorNaiveRandomImpl();
        return new[] {
            new TestCaseData(baseImpl, "[a-f0-9]{32}", "lower case, no dashes")
                .SetName("it should match pattern [a-f0-9]{32} for case: lower case, no dashes")
            //new TestCaseData(new ???, "[A-F0-9]{32}", "upper case, no dashes")
            //    .SetName("it should match pattern [A-F0-9]{32} for case: upper case, no dashes"),
            //new TestCaseData(new ???, "[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}", "lower case, with dashes")
            //    .SetName("it should match pattern [a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12} for case: lower case, with dashes"),
            //new TestCaseData(new ???, "[A-F0-9]{8}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{12}", "upper case, with dashes")
            //    .SetName("it should match pattern [A-F0-9]{8}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{12} for case: upper case, with dashes")
        };
    }
}