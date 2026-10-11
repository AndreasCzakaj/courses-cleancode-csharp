namespace CleanCodeCSharp.Examples.Errors;

public enum ErrorCode
{
    InternalError,
    ValidationError,
}

public class ProcessData;

public class ProcessResult
{
    public string? ErrorMessage { get; init; }
    public ProcessData? Data { get; init; }
    public bool IsSuccess => ErrorMessage == null;
}

public abstract class ServiceException : Exception;

public class InternalProblemException : ServiceException;

public class ValidationException : ServiceException;

public class Service
{
    public ErrorCode ProcessWithErrorCode()
    {
        return ErrorCode.ValidationError;
    }

    public ProcessResult ProcessWithResultObject()
    {
        return new ProcessResult { ErrorMessage = "drama!" };
    }

    public void ProcessWithCallbacks(Action<ProcessData> resolve, Action<ErrorCode> reject)
    {
        reject(ErrorCode.ValidationError);
    }

    public ProcessData ProcessWithExceptions()
    {
        throw new ValidationException();
    }
}
