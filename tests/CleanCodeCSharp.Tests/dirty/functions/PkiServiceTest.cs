using System;
using NUnit.Framework;

namespace CleanCode.Functions.Tests;

[TestFixture]
public class PkiServiceTest {
    PkiService sut;

    [SetUp]
    public void SetUp() {
        sut = new PkiService();
    }

    [Test]
    public void CreateSignedCertificateForCsrOK() {
        // given
        string csrAsBase64 = "LS0...0tCg==";
        DateTime notBefore = DateTime.Parse("2024-12-01");
        DateTime notAfter = DateTime.Parse("2025-11-30");
        string type = "apps";
        string uid = "p123";

        // when
        var actual = sut.CreateSignedCertificateForCsr(csrAsBase64, notBefore, notAfter, type, uid);

        // then
        Assert.That(actual, Is.EqualTo("LS0tL...tLS0K"));
    }
}