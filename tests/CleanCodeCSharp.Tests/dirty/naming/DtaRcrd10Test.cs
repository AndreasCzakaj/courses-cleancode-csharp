using NUnit.Framework;

namespace CleanCode.Naming.Tests;

[TestFixture]
public class DtaRcrd10Test {

    [Test]
    public void DtaRcrd10() {
        var dataRecord = new DtaRcrd10();
        dataRecord.serverProt = "12345";

        Assert.That(dataRecord.serverProt, Is.EqualTo("12345"));
    }
}