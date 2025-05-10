using System.Text.Json;
using Serilog;
using ServerMonitor.Models;

namespace ServerMonitor.Bot.Services.Games;

public class BaseGameService
{
    protected async Task<RestResponse<T>> Request<T>(string url)
    {
        try
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(new Uri(url));
            return new RestResponse<T>(response.IsSuccessStatusCode,
                JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!);
        }
        catch (Exception e)
        {
            Log.Warning(e, "Failed to get response from server");
            return new RestResponse<T>(false, default!, e);
        }
    }
}