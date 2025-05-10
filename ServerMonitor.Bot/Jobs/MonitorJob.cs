using Discord;
using Discord.WebSocket;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServerMonitor.Bot.Services.Games;
using ServerMonitor.Database;
using ServerMonitor.Database.Entities;
using Game = ServerMonitor.Models.Game;

namespace ServerMonitor.Bot.Jobs;

public class MonitorJob(IServiceProvider serviceProvider)
{
    [DisableConcurrentExecution("monitor", 300)]
    public async Task Check()
    {
        var client = serviceProvider.GetRequiredService<DiscordSocketClient>();
        var db = serviceProvider.GetRequiredService<DatabaseContext>();
        var channels = await db.Channels.Include(c => c.Server).ToListAsync();

        foreach (var channel in channels)
        {
            var socketChannel = await client.GetChannelAsync(channel.Id);
            if (socketChannel is not SocketTextChannel textChannel) continue;

            Embed embed;

            switch (channel.Server.Game)
            {
                case Game.MotorTown:
                {
                    var api = serviceProvider.GetRequiredService<MotorTownService>();
                    var players = api.FetchPlayers(channel.Server);
                    var version = api.FetchVersion(channel.Server);
                    await Task.WhenAll(players, version);
                    if (!players.Result.IsSuccess || !version.Result.IsSuccess)
                    {
                        embed = BuildFailEmbed(channel.Server, players.Result.Exception ?? version.Result.Exception);
                        break;
                    }

                    embed = new EmbedBuilder()
                        .WithAuthor(channel.Server.Name)
                        .WithTitle($"{players.Result.Response.Data.Count} {"player".Quantize(players.Result.Response.Data.Count)} connected")
                        .WithDescription(string.Join("\n", players.Result.Response.Data.Select(p => $"[{p.Value.Name}](https://steamcommunity.com/profiles/{p.Value.UniqueId}/)")))
                        .WithColor(Color.Green)
                        .WithFooter($"Motor Town - {version.Result.Response.Data.Version}")
                        .WithCurrentTimestamp()
                        .Build();
                    break;
                }
                default:
                    continue;
            }

            if (channel.MessageId == null)
            {
                var message = await textChannel.SendMessageAsync(embed: embed);
                channel.MessageId = message.Id;
                db.Update(channel);
            }
            else
            {
                await textChannel.ModifyMessageAsync(channel.MessageId.Value, (m) => m.Embed = embed);
            }
        }

        await db.SaveChangesAsync();
    }

    private Embed BuildFailEmbed(ServerEntity server, Exception? exception)
    {
        return new EmbedBuilder()
            .WithAuthor(server.Name)
            .WithTitle("Error occurred")
            .WithDescription(exception?.Message ?? "Unknown error")
            .WithColor(Color.Red)
            .WithFooter($"Motor Town")
            .WithCurrentTimestamp()
            .Build();
    }
}