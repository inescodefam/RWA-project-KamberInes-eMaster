using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface ICityProfessionalService
    {
        public IEnumerable<CityProfessionalDataVM> GetCityProfessionals(int page, int pageSize);
        public List<CityVM> GetCitysByProfessional(int professionalId);

        public List<ProfessionalDataVM> GetProfessionalsByCity(int cityId);

        public CityProfessionalVM AddCityProfessional(CityProfessionalVM model);
        public CityProfessionalDataVM UpdateCityProfessional(int id, CityProfessionalDataVM model);
        public List<CityProfessionalDataVM> UpdateCitiesByProfessional(int professionalId, List<int> citiesIds);
        public List<CityProfessionalDataVM> UpdateProfessionalsByCity(int cityId, List<int> professionalsIds);
        public bool DeleteCityProfessional(int idProfessionalCity);
        public bool DeleteCitiesForProfessional(int professionalId);
        public bool DeleteProfessionalsForCity(int cityId);
    }
}
