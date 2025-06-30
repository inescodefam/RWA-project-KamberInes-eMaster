using eProfessional.BLL.DTOs;

namespace eProfessional.BLL.Interfaces
{
    public interface ICityProfessionalService
    {
        public IEnumerable<CityProfessionalDto> GetCityProfessionals();
        public List<CityDto> GetCitiesByProfessionalId(int professionalId);

        public List<ProfessionalDto> GetProfessionalsByCity(int cityId);

        public CityProfessionalDto AddCityProfessional(CityProfessionalDto model);
        public CityProfessionalDto UpdateCityProfessional(CityProfessionalDto model);

        public List<CityProfessionalDto> UpdateCitiesByProfessionalId(int professionalId, List<CityDto> citiesDtos);
        public List<CityProfessionalDto> UpdateProfessionalsByCityId(int cityId, List<ProfessionalDto> professionalsDtos);
        public bool DeleteCityProfessional(int idProfessionalCity);
        public bool DeleteCitiesForProfessional(int professionalId);
        public bool DeleteProfessionalsForCity(int cityId);
    }
}
