using CleanCodeCSharp.Dirty.Naming;

namespace CleanCodeCSharp.Tests.Dirty.Naming;

public class DtaRcrd10Test
{
    [Test]
    public void ShouldStoreTheServerPort()
    {
        var dataRecord = new DtaRcrd10();
        dataRecord.ServerProt = "12345";

        Assert.That(dataRecord.ServerProt, Is.EqualTo("12345"));
    }
}
