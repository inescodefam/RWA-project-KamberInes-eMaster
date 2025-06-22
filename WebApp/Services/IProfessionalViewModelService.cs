using WebApp.Models;

namespace WebApp.Services
{
    public interface IProfessionalViewModelService
    {
        Task<ProfessionalIndexVM> GetProfessionalIndexVM(
            List<ProfessionalVM> professionalVm,
            int professionalCount,
            int userCount,
            int cityCount);

    }
}