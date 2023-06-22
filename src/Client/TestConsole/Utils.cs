using Newtonsoft.Json;

namespace TestConsole;

public static class Utils
{
    public static async Task<T> ConvertResponseContentAsync<T>(HttpResponseMessage response)
    {
        string responseContent = await response.Content.ReadAsStringAsync();
        T? convertedObject = JsonConvert.DeserializeObject<T>(responseContent);

        return convertedObject;
    }
}

