using eProfessional.BLL.DTOs;

namespace eProfessional.BLL.Interfaces
{
    public interface ICityProfessionalService
    {
        public IEnumerable<CityProfessionalDto> GetCityProfessionals();
        public List<CityDto> GetCitiesByProfessionalId(int professionalId);

        public List<ProfessionalDto> GetProfessionalsByCity(int cityId);

        public CityProfessionalDto AddCityProfessional(CityProfessionalDto model);
        public CityProfessionalDto UpdateCityProfessional(int id, CityProfessionalDto model);
        public bool DeleteCityProfessional(int idProfessionalCity);
    }
}
