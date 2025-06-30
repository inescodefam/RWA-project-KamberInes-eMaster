using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eProfessional.DAL.Models;
[Table("LOG")]
public partial class Log
{
    [Key]
    public int IdLog { get; set; }

    public DateTime? LogTimeStamp { get; set; }

    public string? LogMessage { get; set; }

    public string? LogLevel { get; set; }
}
