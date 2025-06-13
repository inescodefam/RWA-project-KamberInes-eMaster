using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.BL.Models;
[Table("LOG")]
public partial class Log
{
    [Key]
    public int IdLog { get; set; }

    public DateTime? LogTimeStamp { get; set; }

    [StringLength(200)]
    public string? LogMessage { get; set; }

    [StringLength(50)]
    public string? LogLevel { get; set; }
}
