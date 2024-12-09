namespace Store.Application.Models;

public class Paged<T> where T : class
{
    public IEnumerable<T> Data { get; set; } = new List<T>();
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
}