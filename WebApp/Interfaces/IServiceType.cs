using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface IServiceType
    {
        public List<ServiceTypeVM> GetServiceTypes(int count, int start);
        public ServiceTypeVM GetServiceTypeById(int id);
        public int GetTotalServiceTypesCount();
        public ServiceTypeVM CreateServiceType(ServiceTypeVM serviceTypeDto);
        public ServiceTypeVM UpdateServiceType(ServiceTypeVM serviceTypeDto);
        public bool DeleteServiceType(int id);
    }
}
