namespace Store.Common.Results;

public abstract class Result<T> : Result where T : class
{
    protected Result()
    {
    }

    protected Result(T data)
    {
        Data = data;
    }

    public T Data { get; } = null;
}

public abstract class Result
{
    public bool Success { get; protected set; }
}