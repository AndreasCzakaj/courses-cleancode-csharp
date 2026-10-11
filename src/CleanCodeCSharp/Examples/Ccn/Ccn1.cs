namespace CleanCodeCSharp.Examples.Ccn;

public class Ccn1
{
    public int GetPort()
    {
        string? portAsString = Environment.GetEnvironmentVariable("port");
        if (portAsString != null)
        {
            try
            {
                return int.Parse(portAsString);
            }
            catch (FormatException)
            {
                return 8080;
            }
        }
        return 8080;
    }
}
