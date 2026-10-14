using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

namespace CleanCodeCSharp.Dirty.Uss;

public class Server
{
    public static void Run()
    {
        var app = WebApplication.CreateBuilder().Build(); // int

        app.MapPost("/uss/login", async (HttpContext ctx) => // int
        {
            SqliteConnection? c = null; // int
            try
            {
                var b = await new StreamReader(ctx.Request.Body).ReadToEndAsync(); // int

                if (b == "")
                {
                    throw new JsonException("invalid json");
                }

                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(b);
                var username = values?.GetValueOrDefault("username");
                var password = values?.GetValueOrDefault("password");
                var service = new LoginService();

                var result = await service.Login(username, password);
                if (result.errorType == ErrorTypes.OK)
                {
                    ctx.Response.StatusCode = 200;
                    await ctx.Response.WriteAsJsonAsync(new { error = result.error });
                }
                else if (result.errorType == ErrorTypes.Validation)
                {
                    ctx.Response.StatusCode = 400;
                    await ctx.Response.WriteAsJsonAsync(result.UserSession);
                }
                else if (result.errorType == ErrorTypes.Auth)
                {
                    ctx.Response.StatusCode = 401;
                    await ctx.Response.WriteAsJsonAsync(result.UserSession);
                }
                else
                {
                    ctx.Response.StatusCode = 500;
                }
            }
            catch (JsonException)
            {
                ctx.Response.StatusCode = 400;
                await ctx.Response.WriteAsJsonAsync(new { error = "invalid: username" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ctx.Response.StatusCode = 500;
                await ctx.Response.WriteAsJsonAsync(new { error = "try again later" });
            }
            finally
            {
                if (c != null)
                {
                    await c.CloseAsync();
                }
            }
        });

        var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";
        Console.WriteLine("Server is running on port " + port);
        app.Run("http://localhost:" + port);
    }
}

