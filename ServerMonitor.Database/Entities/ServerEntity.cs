using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ServerMonitor.Models;

namespace ServerMonitor.Database.Entities;

public class ServerEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [MaxLength(256)]
    public required string Name { get; set; }
    public Game Game { get; set; }
    [MaxLength(256)]
    public required string Host { get; set; }
    [MaxLength(256)]
    public string? Password { get; set; } = null;
}