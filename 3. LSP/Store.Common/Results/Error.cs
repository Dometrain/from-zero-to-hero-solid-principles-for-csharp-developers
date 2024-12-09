namespace Store.Common.Results;

public class Error
{
    public Error(string code, string details)
    {
        Code = code;
        Details = details;
    }

    public string Code { get; }
    public string Details { get; }
}