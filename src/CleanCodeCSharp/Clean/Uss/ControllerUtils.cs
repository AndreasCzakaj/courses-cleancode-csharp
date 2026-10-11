namespace CleanCodeCSharp.Clean.Uss;

/// <summary>
/// What a controller answers. A plain value on purpose: it isolates the controllers from ASP.NET,
/// and a test can simply compare it.
/// </summary>
public record HttpResult(int Status, object Content);

public record ErrorContent(string Error);

public static class ControllerUtils
{
    public const string MessageServerError = "try again later";

    /// <summary>The errors that are the client's fault, with their HTTP status</summary>
    private static readonly (Type Type, int Status)[] ClientErrors =
    [
        (typeof(ValidationException), 400),
        (typeof(AuthenticationFailedException), 401),
        (typeof(AccountNotVerifiedException), 400),
    ];

    public static int CalcHttpErrorCode(Exception e)
    {
        var clientError = ClientErrors.Where(clientError => clientError.Type.IsInstanceOfType(e)).ToArray();
        return clientError.Length > 0 ? clientError[0].Status : 500;
    }

    /// <summary>Clients get the details of their own errors only, never the server's internals.</summary>
    public static string CalcErrorMessage(Exception e)
    {
        return CalcHttpErrorCode(e) < 500 ? e.Message : MessageServerError;
    }

    /// <summary>
    /// Runs a service call and maps its outcome to the HTTP result:
    /// the result with the given status, or an error status with a message.
    /// </summary>
    public static async Task<HttpResult> Respond<T>(
        int successStatus,
        Func<Task<T>> serviceCall,
        Action<Exception> logError
    ) where T : notnull
    {
        try
        {
            return new HttpResult(successStatus, await serviceCall());
        }
        catch (Exception serviceError)
        {
            var status = CalcHttpErrorCode(serviceError);
            if (status >= 500)
            {
                logError(serviceError);
            }
            return new HttpResult(status, new ErrorContent(CalcErrorMessage(serviceError)));
        }
    }
}
