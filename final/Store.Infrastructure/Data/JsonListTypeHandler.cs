using System.Data;
using Dapper;
using Store.Infrastructure.Models;

public class JsonListTypeHandler<T> : SqlMapper.TypeHandler<JsonList<T>>
{
    public override void SetValue(IDbDataParameter parameter, JsonList<T> value)
    {
        parameter.Value = value.ToString();
    }

    public override JsonList<T> Parse(object value)
    {
        return JsonList<T>.FromString(value.ToString());
    }
}
