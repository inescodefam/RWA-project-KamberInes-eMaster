using eProfessional.BLL.DTOs;

namespace eProfessional.BLL.Interfaces
{
    public interface ICityProfessionalService
    {
        public List<CityProfessionalDataDto> GetCityProfessionals(int count, int start);
        public List<CityDto> GetCitiesByProfessional(int professionalId);

        public List<ProfessionalDataDto> GetProfessionalsByCity(int cityId);

        public CityProfessionalDto AddCityProfessional(CityProfessionalDto model);
        public CityProfessionalDataDto UpdateCityProfessional(CityProfessionalDto model);

        public List<CityProfessionalDataDto> UpdateCitiesByProfessional(int professionalId, List<int> citiesIds);
        public List<CityProfessionalDataDto> UpdateProfessionalsByCity(int cityId, List<int> professionalsIds);
        public bool DeleteCityProfessional(int idProfessionalCity);
        public bool DeleteCitiesForProfessional(int professionalId);
        public bool DeleteProfessionalsForCity(int cityId);
    }
}
