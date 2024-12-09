using Microsoft.Extensions.Configuration;

namespace Store.Infrastructure.Infrastructure;

// Note: This is not the recommended way to load config but is shown as an example of how to do it without dependency injection.
public class ConfigHelper
{
    private static ConfigHelper _appSettings;

    public string Value { get; set; }

    public static string AppSetting(string Key)
    {
        _appSettings = GetCurrentSettings(Key);
        return _appSettings.Value;
    }

    public ConfigHelper(IConfiguration config, string Key)
    {
        Value = config.GetValue<string>(Key);
    }

    public static ConfigHelper GetCurrentSettings(string key)
    {
        var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        var configuration = builder.Build();
        var keySplit = key.Split(":");
        var sectionKey = keySplit.Length > 1 ? string.Join(":", keySplit.Skip(1)) : key;
        return new ConfigHelper(configuration.GetSection(keySplit[0] ?? "AppSettings"), sectionKey);
    }
}
