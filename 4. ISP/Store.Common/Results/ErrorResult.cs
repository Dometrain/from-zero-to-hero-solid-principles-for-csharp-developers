namespace Store.Common.Results;

public class ErrorResult<T> : Result<T> where T : class
{
    public ErrorResult(string message)
    {
        Message = message;
    }

    public string Message { get; }
}

public class ErrorResult : Result
{
    public ErrorResult(string message)
    {
        Message = message;
    }

    public string Message { get; }
}