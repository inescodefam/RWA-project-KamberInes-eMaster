using eProfessional.BLL.DTOs;

namespace eProfessional.BLL.Interfaces
{
    public interface IServiceType
    {
        public List<ServiceTypeDto> GetServiceTypes(int count, int start);

        public int GetTotalServiceTypesCount();
        public ServiceTypeDto GetServiceTypeById(int id);

        public ServiceTypeDto CreateServiceType(ServiceTypeDto serviceTypeDto);
        public ServiceTypeDto UpdateServiceType(ServiceTypeDto serviceTypeDto);
        public bool DeleteServiceType(int id);
    }
}
