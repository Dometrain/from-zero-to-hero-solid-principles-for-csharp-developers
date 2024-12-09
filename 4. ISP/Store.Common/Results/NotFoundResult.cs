namespace Store.Common.Results;

public class NotFoundResult<T> : Result<T> where T : class
{
}

public class NotFoundResult : Result
{
}