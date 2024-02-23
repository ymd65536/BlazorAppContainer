namespace BlazorAppContainer.Data;

using Momento.Sdk;
using Momento.Sdk.Auth;
using Momento.Sdk.Config;

public class MomentoCacheSample
{
    private ICredentialProvider? authProvider;
    private TimeSpan DEFAULT_TTL;
    private ICacheClient? client;

    public MomentoCacheSample()
    {
        DEFAULT_TTL = TimeSpan.FromSeconds(60);
        authProvider = new EnvMomentoTokenProvider("MOMENTO_AUTH_TOKEN");
        client = new CacheClient(Configurations.Laptop.V1(), authProvider, DEFAULT_TTL);
    }

    async public void SetValue(string CacheName, string Key, string Value)
    {
        try
        {
            var setResponse = await client.SetAsync(CacheName, Key, Value);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error setting value: {e.Message}. Exiting.");
        }
    }
}
