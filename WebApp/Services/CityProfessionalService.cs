using Shared.BL.DTOs;
using Shared.BL.Services;

namespace WebApp.Services
{
    public class CityProfessionalService : ICityProfessionalService
    {
        public Task<CityProfessionalDto> AddCityProfessionalAsync(CityProfessionalDto model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCityProfessionalAsync(int idProfessionalCity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CityProfessionalDto>> GetCityProfessionalsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<CityProfessionalDto?>> GetCitysByProfessionalAsync(int professionalId)
        {
            throw new NotImplementedException();
        }

        public Task<List<CityProfessionalDto>> GetProfessionalsByCityAsync(int cityId)
        {
            throw new NotImplementedException();
        }

        public Task<CityProfessionalDto> UpdateCityProfessionalAsync(int id, CityProfessionalDto model)
        {
            throw new NotImplementedException();
        }
    }
}
