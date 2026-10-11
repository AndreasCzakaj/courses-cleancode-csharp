namespace CleanCodeCSharp.Dirty.Functions;

public class PkiService
{
    public string CreateSignedCertificateForCsr(string csrAsBase64, DateOnly notBefore, DateOnly notAfter, string type, string uid)
    {
        // openssl pki magic....
        return "LS0tL...tLS0K";
    }
}
