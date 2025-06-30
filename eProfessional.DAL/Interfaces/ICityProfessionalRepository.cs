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
        List<CityProfessional> UpdateCitiesForProfessional(int id, List<City> cities);
        List<CityProfessional> UpdateProfessionalsForCity(int id, List<Professional> professionals);
        bool DeleteCitiesForProfessional(int id);
        bool DeleteProfessionalsForCity(int id);
    }
}
