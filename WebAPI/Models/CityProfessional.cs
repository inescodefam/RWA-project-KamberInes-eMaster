using Shared.BL.Models;

namespace WebAPI.Models;

public partial class CityProfessional
{
    public int IdProfessionalCity { get; set; }

    public int ProfessionalId { get; set; }

    public int? CityId { get; set; }

    public virtual City? City { get; set; }

    public virtual Professional? Professional { get; set; }
}
