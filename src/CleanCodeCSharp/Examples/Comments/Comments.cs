namespace CleanCodeCSharp.Examples.Comments;

public class Comments
{
    public void Init()
    {
        // get the port
        const string defaultPortAsString = "8080";
        // Admin spelled the env param wrong, and we can't change it
        string? portAsString = Environment.GetEnvironmentVariable("prot");
        int port = int.Parse(portAsString ?? defaultPortAsString);

        // start server on given port
        Console.WriteLine(port);
    }
}
