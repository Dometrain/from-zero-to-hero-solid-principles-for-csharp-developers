namespace Store.Common.Results;

public class SuccessResult<T> : Result<T> where T : class
{
    public SuccessResult()
    {
        Success = true;
    }

    public SuccessResult(T data) : base(data)
    {
        Success = true;
    }
}

public class SuccessResult : Result
{
    public SuccessResult()
    {
        Success = true;
    }
}