namespace CleanCodeCSharp.Examples.Errors;

public class Client(Service service)
{
    public void ConsumeWithErrorCode()
    {
        ErrorCode errorCode = service.ProcessWithErrorCode();
        if (errorCode == ErrorCode.ValidationError)
        {
            // handle error
        }
        else
        {
            // where's my data?
        }
        // what about unknown/new errors?
    }

    public void ConsumeWithResultObject()
    {
        ProcessResult result = service.ProcessWithResultObject();
        if (result.IsSuccess)
        {
            Console.WriteLine(result.Data);
        }
        else
        {
            // handle error
        }
    }

    public void ConsumeWithCallbacks()
    {
        service.ProcessWithCallbacks(
            data => Console.WriteLine(data),
            errorCode => Console.Error.WriteLine(errorCode) // handleError
        );
    }

    public void ConsumeWithExceptions()
    {
        try
        {
            var data = service.ProcessWithExceptions();
            Console.WriteLine(data);
        }
        catch (ValidationException)
        {
            // handle here, e.g. return HTTP 400
            // ... or re-throw it so a generic error handler can deal with it
        }
        catch (InternalProblemException)
        {
            // handle here, e.g. return HTTP 500
            // ... or re-throw it so a generic error handler can deal with it
        }
        // everything else will be thrown up the call stack
    }
}
