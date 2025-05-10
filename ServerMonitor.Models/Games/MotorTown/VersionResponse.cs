namespace ServerMonitor.Models.Games.MotorTown;

public class VersionResponse
{
    public VersionData Data { get; set; }
}

public class VersionData
{
    public string Version { get; set; }
}