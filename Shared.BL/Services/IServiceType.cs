using Shared.BL.DTOs;

namespace Shared.BL.Services
{
    public interface IServiceType
    {
        public Task<List<ServiceTypeDto>> GetServiceTypes(int count, int start);
        public Task<ServiceTypeDto> GetServiceTypeById(int id);

        public Task<ServiceTypeDto> CreateServiceType(ServiceTypeDto serviceTypeDto);
        public Task<ServiceTypeDto> UpdateServiceType(ServiceTypeDto serviceTypeDto);
        public Task<bool> DeleteServiceType(int id);
    }
}
