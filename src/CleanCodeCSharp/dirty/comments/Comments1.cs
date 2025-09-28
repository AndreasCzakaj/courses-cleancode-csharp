namespace CleanCode.Comments;

public class Comments1 {
    void Init() {
        // get the port from env or use default
        string? portAsString = Environment.GetEnvironmentVariable("port");
        int defaultPort = 8080;
        int port = portAsString == null ? defaultPort : int.Parse(portAsString);

        // start server on given port
        Console.WriteLine(port);
    }
}