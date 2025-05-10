using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Template.Bot.Services;

namespace Template.Bot.Jobs;

public class StatusJob(IServiceProvider serviceProvider)
{
    private int lastStatus;
    private readonly string[] statuses = ["/help", "eris.gg"];

    public async Task SetStatus()
    {
        try
        {
            var client = serviceProvider.GetRequiredService<DiscordSocketClient>();
            var prom = serviceProvider.GetRequiredService<PrometheusService>();
            await client.SetCustomStatusAsync(statuses[lastStatus]);
            lastStatus++;
            if (lastStatus == statuses.Length) lastStatus = 0;
            prom.Latency.Set(client.Latency);
        }
        catch (Exception e)
        {
            Log.Error(e, "Failed to set status");
        }
    }
}