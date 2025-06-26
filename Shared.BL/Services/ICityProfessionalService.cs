using Shared.BL.DTOs;

namespace Shared.BL.Services
{
    public interface ICityProfessionalService
    {
        public Task<IEnumerable<CityProfessionalDto>> GetCityProfessionalsAsync();
        public Task<List<CityProfessionalDto?>> GetCitysByProfessionalAsync(int professionalId);

        public Task<List<CityProfessionalDto>> GetProfessionalsByCityAsync(int cityId);

        public Task<CityProfessionalDto> AddCityProfessionalAsync(CityProfessionalDto model);
        public Task<CityProfessionalDto> UpdateCityProfessionalAsync(int id, CityProfessionalDto model);
        public Task<bool> DeleteCityProfessionalAsync(int idProfessionalCity);
    }
}
