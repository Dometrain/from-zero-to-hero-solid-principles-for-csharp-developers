using Newtonsoft.Json;

namespace Store.Infrastructure.Models;

public class JsonList<T> : List<T>
{
    public JsonList()
    {
    }

    public JsonList(IEnumerable<T> list) : base(list)
    {
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static JsonList<T> FromString(string value)
    {
        var result = new JsonList<T>();
        var items = JsonConvert.DeserializeObject<T[]>(value);
        result.AddRange(items);
        return result;
    }
}