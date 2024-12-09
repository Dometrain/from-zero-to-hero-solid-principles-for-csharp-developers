namespace Store.Infrastructure.Models;

public class PagedRecord<T>
{
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public JsonList<T> Data { get; set; } = new JsonList<T>();
}