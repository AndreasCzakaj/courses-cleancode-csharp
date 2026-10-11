namespace CleanCodeCSharp.Clean.Uss;

public class LoginController(UserSelfService service, Action<Exception> logError)
{
    public Task<HttpResult> Action(Credentials? body)
    {
        // the body is untrusted input: the service validates it
        return ControllerUtils.Respond(200, () => service.Login(body), logError);
    }
}
