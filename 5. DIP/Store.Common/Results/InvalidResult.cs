namespace Store.Common.Results;

public class InvalidResult<T> : Result<T> where T : class
{
    public InvalidResult(string message) : this(message, Array.Empty<Error>())
    {
    }

    public InvalidResult(string message, IReadOnlyCollection<Error> errors)
    {
        Message = message;
        Success = false;
        Errors = errors ?? Array.Empty<Error>();
    }

    public string Message { get; }
    public IReadOnlyCollection<Error> Errors { get; }
}

public class InvalidResult : Result
{
    public InvalidResult(string message) : this(message, Array.Empty<Error>())
    {
    }

    public InvalidResult(string message, IReadOnlyCollection<Error> errors)
    {
        Message = message;
        Success = false;
        Errors = errors ?? Array.Empty<Error>();
    }

    public string Message { get; }
    public IReadOnlyCollection<Error> Errors { get; }
}