using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerMonitor.Database.Entities;

public class ChannelEntity
{
    [Key]
    public ulong Id { get; set; }
    public ulong? MessageId { get; set; }
    [ForeignKey(nameof(Server))]
    public int ServerId { get; set; }
    public virtual ServerEntity Server { get; set; } = null!;
}