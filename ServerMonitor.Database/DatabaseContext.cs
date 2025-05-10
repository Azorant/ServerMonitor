using Microsoft.EntityFrameworkCore;
using Serilog;
using ServerMonitor.Database.Entities;

namespace ServerMonitor.Database;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<ServerEntity> Servers { get; set; }
    public DbSet<ChannelEntity> Channels { get; set; }

    public async Task ApplyMigrations()
    {
        var pending = (await Database.GetPendingMigrationsAsync()).ToList();
        if (pending.Any())
        {
            Log.Information($"Applying {pending.Count} migrations: {string.Join(", ", pending)}");
            await Database.MigrateAsync();
            Log.Information("Migrations applied");
        }
        else
        {
            Log.Information("No migrations to apply.");
        }
    }
};