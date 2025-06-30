using eProfessional.BLL.DTOs;

namespace eProfessional.BLL.Interfaces
{
    public interface IProfessionalService
    {
        List<ProfessionalDataDto> GetProfessionals(int count, int start = 0);
        ProfessionalDataDto GetSingleProfessional(int id);
        List<ProfessionalDataDto> Search(string? Name, string? serviceType, int count, int start = 0);
        bool CreateProfessional(ProfessionalDataDto professionalDto);
        bool UpdateProfessional(ProfessionalDataDto professionalDto);
        bool DeleteProfessional(int id);
    }
}
