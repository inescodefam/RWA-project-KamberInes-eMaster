using eProfessional.DAL.Models;

namespace eProfessional.DAL.Interfaces
{
    public interface ICityProfessionalRepository : ICrudRepository<CityProfessional>
    {
        bool ExsistsCityProfessional(CityProfessional model);
        bool ExsistsCityProfessionalId(int cityProfessionalId);
        bool ProfessionalExists(int professionalId);
        Professional ProfessionalExists(string name);
        bool CityExists(int cityId);
        City CityExists(string city);
        List<CityProfessional> GetCitiesByProfessionalId(int professionalId);
        List<CityProfessional> GetProfessionalsByCity(int cityId);
        List<CityProfessional> UpdateCitiesForProfessional(int id, List<int> citiesIds);
        List<CityProfessional> UpdateProfessionalsForCity(int id, List<int> professionalsIds);
        bool DeleteCitiesForProfessional(int id);
        bool DeleteProfessionalsForCity(int id);
        List<CityProfessional> GetAllCityProfessionals(int count, int start);
    }
}
