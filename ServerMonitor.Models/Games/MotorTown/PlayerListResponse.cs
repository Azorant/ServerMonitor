using System.Text.Json.Serialization;

namespace ServerMonitor.Models.Games.MotorTown;

public class PlayerListResponse
{
    public Dictionary<string, PlayerData> Data { get; set; }
}

public class PlayerData
{
    public string Name { get; set; }
    [JsonPropertyName("unique_id")]
    public string UniqueId { get; set; }
}