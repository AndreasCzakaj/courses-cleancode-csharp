using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace CleanCodeCSharp.Clean.Uss;

/// <param name="Port">0: any free port</param>
public record ServerConfig(int Port, string ConnectionString);

public sealed class RunningServer(WebApplication app, int port) : IAsyncDisposable
{
    public int Port { get; } = port;

    public Task WaitForShutdown() => app.WaitForShutdownAsync();

    public async ValueTask DisposeAsync()
    {
        await app.StopAsync();
        await app.DisposeAsync();
    }
}

// Integration code only: wires the parts, defines the routes, starts the server.
// No logic here => nothing to unit test, the integration test covers it.
public static class Server
{
    public static async Task<RunningServer> Run(ServerConfig config)
    {
        var accountDao = new AccountDaoSqliteImpl(config.ConnectionString);
        var service = new UserSelfService(accountDao);
        var loginController = new LoginController(service, Console.Error.WriteLine);

        var builder = WebApplication.CreateBuilder();
        builder.Logging.SetMinimumLevel(LogLevel.Warning);
        var app = builder.Build();
        app.Urls.Add($"http://127.0.0.1:{config.Port}");

        // ASP.NET parses the JSON body; it answers 400 itself if the body is not valid JSON
        app.MapPost("/uss/login", async (Credentials? body) => ToResult(await loginController.Action(body)));

        await app.StartAsync();
        var port = GetPort(app);
        Console.WriteLine($"Server is running on port {port}");

        return new RunningServer(app, port);
    }

    private static IResult ToResult(HttpResult result)
    {
        return Results.Json(result.Content, statusCode: result.Status);
    }

    private static int GetPort(WebApplication app)
    {
        var addresses = app.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>()!;
        return new Uri(addresses.Addresses.First()).Port;
    }
}
