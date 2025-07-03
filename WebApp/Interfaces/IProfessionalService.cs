using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface IProfessionalService
    {
        ProfessionalDataVM GetSingleProfessional(int id);
        ProfessionalIndexVM GetProfessionals(int pageSize, int page = 0);
        ProfessionalIndexVM Search(string? Name, string? cityName, int pageSize, int page = 0);
        bool CreateProfessional(ProfessionalBaseVm professionalDto);
        bool UpdateProfessional(ProfessionalDataVM professionalDto);
        bool DeleteProfessional(int id);
    }
}
