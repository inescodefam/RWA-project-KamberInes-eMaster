using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface IProfessionalService
    {
        ProfessionalDataVM GetSingleProfessional(int id);
        List<ProfessionalIndexVM> GetProfessionals(int pageSize, int page = 0);
        List<ProfessionalDataVM> Search(string? Name, string? cityName, int pageSize, int page = 0);
        bool CreateProfessional(CreateProfessionalVM professionalDto);
        bool UpdateProfessional(ProfessionalDataVM professionalDto);
        bool DeleteProfessional(int id);
    }
}
