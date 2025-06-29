using eProfessional.DAL.Models;

namespace eProfessional.DAL.Interfaces
{
    public interface ICityProfessionalRepository : ICrudRepository<CityProfessional>
    {
        bool ExsistsCityProfessional(CityProfessional model);
        bool ExsistsCityProfessionalId(int cityProfessionalId);
        bool ProfessionalExists(int professionalId);
        bool CityExists(int cityId);
        List<CityProfessional> GetCitiesByProfessionalId(int professionalId);
        List<CityProfessional> GetProfessionalsByCity(int cityId);
    }
}
