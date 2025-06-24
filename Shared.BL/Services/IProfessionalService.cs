using Shared.BL.DTOs;

namespace Shared.BL.Services
{
    public interface IProfessionalService
    {
        Task<List<ProfessionalDto>> GetProfessionals(int count, int start = 0);
        Task<ProfessionalDto> GetSingleProfessional(int id);
        Task<List<ProfessionalDto>> SearchProfessionals(string? Name, string? cityName, int count, int start = 0);
        Task<bool> CreateProfessional(ProfessionalDto professionalDto);
        bool UpdateProfessional(int id, ProfessionalDto professionalDto);
        bool DeleteProfessional(int id);
    }
}
