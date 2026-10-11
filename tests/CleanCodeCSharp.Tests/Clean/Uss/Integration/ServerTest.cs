using System.Net.Http.Json;
using System.Text;
using CleanCodeCSharp.Clean.Uss;
using static CleanCodeCSharp.Tests.Clean.Uss.TestUtils;

namespace CleanCodeCSharp.Tests.Clean.Uss.Integration;

// All parts together: HTTP => ASP.NET => controller => service => SQLite.
// The details of each scenario are covered by the (much faster) unit tests.
[Category("Integration")]
public class ServerTest
{
    private SqliteTestDatabase database;
    private RunningServer server;
    private HttpClient client;

    [OneTimeSetUp]
    public async Task StartServer()
    {
        database = await SqliteTestDatabase.Create(CreateVerifiedAccount(), CreateNotVerifiedAccount());
        server = await Server.Run(new ServerConfig(Port: 0, database.ConnectionString));
        client = new HttpClient { BaseAddress = new Uri($"http://127.0.0.1:{server.Port}") };
    }

    [OneTimeTearDown]
    public async Task StopServer()
    {
        client.Dispose();
        await server.DisposeAsync();
        database.Dispose();
    }

    private static readonly TestCaseData[] ClientErrors =
    [
        new TestCaseData(new Credentials(null, null), 400).SetArgDisplayNames("400: credentials have invalid syntax"),
        new TestCaseData(CreateValidCredentialsUnknownUser(), 401).SetArgDisplayNames("401: no such username"),
        new TestCaseData(CreateValidCredentials() with { Password = ValidButWrongPassword }, 401)
            .SetArgDisplayNames("401: wrong password"),
        new TestCaseData(CreateValidCredentialsNotVerifiedAccount(), 400)
            .SetArgDisplayNames("400: account not verified"),
    ];

    [TestCaseSource(nameof(ClientErrors))]
    public async Task PostLoginShouldAnswerWithAnError(Credentials body, int expected)
    {
        var resp = await client.PostAsJsonAsync("/uss/login", body);

        Assert.That((int)resp.StatusCode, Is.EqualTo(expected));
        var content = await resp.Content.ReadFromJsonAsync<ErrorContent>();
        Assert.That(content?.Error, Is.Not.Empty);
    }

    [TestCase("", TestName = "400: no body at all")]
    [TestCase("{oops", TestName = "400: not JSON")]
    [TestCase("""{"username": 12345678, "password": 123456789012}""", TestName = "400: not strings")]
    [TestCase("""{"username": {"$ne": ""}, "password": "Correct-Horse_42"}""", TestName = "400: an object as username")]
    public async Task PostLoginShouldAnswerWith400(string body)
    {
        var resp = await PostLogin(body);

        Assert.That((int)resp.StatusCode, Is.EqualTo(400));
    }

    [Test]
    public async Task PostLoginShouldAnswerWith200AndASession()
    {
        var account = CreateVerifiedAccount();

        var resp = await client.PostAsJsonAsync("/uss/login", CreateValidCredentials());

        Assert.That((int)resp.StatusCode, Is.EqualTo(200));
        Assert.That(
            await resp.Content.ReadAsStringAsync(),
            Is.EqualTo(
                $$"""{"accountId":"{{account.Id}}","username":"{{account.Username}}","email":"{{account.Email}}"}"""
            )
        );
    }

    private Task<HttpResponseMessage> PostLogin(string body)
    {
        return client.PostAsync("/uss/login", new StringContent(body, Encoding.UTF8, "application/json"));
    }
}
