using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ServerMonitor.Bot.EventHandlers;

namespace ServerMonitor.Bot;

public class Events(IServiceProvider serviceProvider)
{
    private readonly DiscordSocketClient client = serviceProvider.GetRequiredService<DiscordSocketClient>();
    private readonly ClientEvents clientEvents = new ClientEvents(serviceProvider);

    public void Register()
    {
        client.JoinedGuild += clientEvents.OnGuildJoined;
        client.LeftGuild += clientEvents.OnGuildLeft;
        client.Ready += clientEvents.OnClientReady;
        client.Disconnected += clientEvents.OnClientDisconnected;
    }

    public void Deregister()
    {
        client.JoinedGuild -= clientEvents.OnGuildJoined;
        client.LeftGuild -= clientEvents.OnGuildLeft;
        client.Ready -= clientEvents.OnClientReady;
        client.Disconnected -= clientEvents.OnClientDisconnected;
    }
}