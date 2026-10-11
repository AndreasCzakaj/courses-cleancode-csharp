using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;

namespace CleanCodeCSharp.Dirty.Uss;

public class Server
{
    public static void Run()
    {
        var app = WebApplication.CreateBuilder().Build();

        app.MapPost("/uss/login", async (HttpContext ctx) =>
        {
            SqliteConnection? c = null;
            try
            {
                var b = await new StreamReader(ctx.Request.Body).ReadToEndAsync();
                JsonElement d = default;
                if (b != "")
                {
                    d = JsonDocument.Parse(b).RootElement;
                }
                if (d.ValueKind == JsonValueKind.Object)
                {
                    if (d.TryGetProperty("username", out var u) && u.ValueKind == JsonValueKind.String)
                    {
                        if (u.GetString()!.Length >= 8)
                        {
                            if (u.GetString()!.Length <= 20)
                            {
                                if (Regex.IsMatch(u.GetString()!, @"^[a-zA-Z0-9\-_]+\z"))
                                {
                                    if (d.TryGetProperty("password", out var p) && p.ValueKind == JsonValueKind.String)
                                    {
                                        if (p.GetString()!.Length >= 12 && p.GetString()!.Length <= 32)
                                        {
                                            if (Regex.IsMatch(p.GetString()!, @"^[a-zA-Z0-9\-_.,+]+\z"))
                                            {
                                                // check db
                                                c = new SqliteConnection(
                                                    Environment.GetEnvironmentVariable("USS_DB") ?? "Data Source=uss.db"
                                                );
                                                await c.OpenAsync();
                                                var cmd = c.CreateCommand();
                                                cmd.CommandText =
                                                    "SELECT id, username, password_hash, email, status FROM accounts WHERE username = $u";
                                                cmd.Parameters.AddWithValue("$u", u.GetString());
                                                var r = await cmd.ExecuteReaderAsync();
                                                if (await r.ReadAsync())
                                                {
                                                    var tmp = r.GetString(2).Split(":");
                                                    var h = Convert.ToHexString(Rfc2898DeriveBytes.Pbkdf2(
                                                        p.GetString()!, Encoding.UTF8.GetBytes(tmp[0]), 210_000,
                                                        HashAlgorithmName.SHA512, 64)).ToLower();
                                                    if (h == tmp[1])
                                                    {
                                                        if (r.GetString(4) == "verified")
                                                        {
                                                            ctx.Response.StatusCode = 200;
                                                            await ctx.Response.WriteAsJsonAsync(new
                                                            {
                                                                accountId = r.GetString(0),
                                                                username = r.GetString(1),
                                                                email = r.GetString(3),
                                                            });
                                                        }
                                                        else
                                                        {
                                                            ctx.Response.StatusCode = 400;
                                                            await ctx.Response.WriteAsJsonAsync(new { error = "account not verified" });
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ctx.Response.StatusCode = 401;
                                                        await ctx.Response.WriteAsJsonAsync(new { error = "unknown username or wrong password" });
                                                    }
                                                }
                                                else
                                                {
                                                    ctx.Response.StatusCode = 401;
                                                    await ctx.Response.WriteAsJsonAsync(new { error = "unknown username or wrong password" });
                                                }
                                            }
                                            else
                                            {
                                                ctx.Response.StatusCode = 400;
                                                await ctx.Response.WriteAsJsonAsync(new { error = "invalid: password" });
                                            }
                                        }
                                        else
                                        {
                                            ctx.Response.StatusCode = 400;
                                            await ctx.Response.WriteAsJsonAsync(new { error = "invalid: password" });
                                        }
                                    }
                                    else
                                    {
                                        ctx.Response.StatusCode = 400;
                                        await ctx.Response.WriteAsJsonAsync(new { error = "invalid: password" });
                                    }
                                }
                                else
                                {
                                    ctx.Response.StatusCode = 400;
                                    await ctx.Response.WriteAsJsonAsync(new { error = "invalid: username" });
                                }
                            }
                            else
                            {
                                ctx.Response.StatusCode = 400;
                                await ctx.Response.WriteAsJsonAsync(new { error = "invalid: username" });
                            }
                        }
                        else
                        {
                            ctx.Response.StatusCode = 400;
                            await ctx.Response.WriteAsJsonAsync(new { error = "invalid: username" });
                        }
                    }
                    else
                    {
                        ctx.Response.StatusCode = 400;
                        await ctx.Response.WriteAsJsonAsync(new { error = "invalid: username" });
                    }
                }
                else
                {
                    ctx.Response.StatusCode = 400;
                    await ctx.Response.WriteAsJsonAsync(new { error = "invalid: username" });
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
