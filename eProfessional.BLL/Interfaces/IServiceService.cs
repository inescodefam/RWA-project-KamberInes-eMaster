using eProfessional.BLL.DTOs;

namespace eProfessional.BLL.Interfaces
{
    public interface IServiceService
    {
        public List<ServiceDto> SearchServices(string searchTerm, int count, int start);
        public int GetServicesCount();
        public List<ServiceDto> GetServicesCount(int count, int start = 0);

        public List<ServiceDto> GetServiceByServiceType(string type, int count, int start = 0);

        public int GetServiceByServiceTypeCount(string type);

        public ServiceDto CreateService(ServiceDto serviceDto);

        public void UpdateService(ServiceDto serviceDto);

        public bool DeleteService(int id);

        public ServiceDto GetServiceByServiceId(int id);
    }
}
