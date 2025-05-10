using Prometheus;

namespace Template.Bot.Services;

public class PrometheusService
{
    public Gauge Guilds { get; set; } = Metrics.CreateGauge("guilds", "How many guilds the bot is in");
    public Counter Commands { get; set; } = Metrics.CreateCounter("commands_total", "Commands ran", labelNames: ["command"]);
    public Gauge Latency { get; set; } = Metrics.CreateGauge("latency", "Websocket latency");
}