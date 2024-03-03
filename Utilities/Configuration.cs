using Microsoft.Extensions.Configuration;

namespace Utilities;

public class Configuration
{
    public static IConfigurationRoot LoadFromJsonFile(string path)
        => new ConfigurationBuilder().AddJsonFile(path, optional: false).Build();
}
