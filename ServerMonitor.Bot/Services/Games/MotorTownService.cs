using System.Text.Json;
using ServerMonitor.Database.Entities;
using ServerMonitor.Models;
using ServerMonitor.Models.Games.MotorTown;

namespace ServerMonitor.Bot.Services.Games;

public class MotorTownService : BaseGameService
{
    public Task<RestResponse<PlayerListResponse>> FetchPlayers(ServerEntity server)
    {
        return Request<PlayerListResponse>($"http://{server.Host}/player/list?password={server.Password}");
    }
    
    public Task<RestResponse<VersionResponse>> FetchVersion(ServerEntity server)
    {
        return Request<VersionResponse>($"http://{server.Host}/version?password={server.Password}");
    }
}