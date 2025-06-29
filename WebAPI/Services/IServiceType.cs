using Shared.BL.DTOs;

namespace Shared.BL.Services
{
    public interface IServiceType
    {
        public List<ServiceTypeDto> GetServiceTypes(int count, int start);
        public ServiceTypeDto GetServiceTypeById(int id);

        public ServiceTypeDto CreateServiceType(ServiceTypeDto serviceTypeDto);
        public ServiceTypeDto UpdateServiceType(ServiceTypeDto serviceTypeDto);
        public bool DeleteServiceType(int id);
    }
}
