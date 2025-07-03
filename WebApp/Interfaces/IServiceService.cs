using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface IServiceService
    {
        public ServiceCreateVM CreateService(ServiceCreateVM vm);
        public bool DeleteService(int id);
        public bool EditService(ServiceEditResultVM vm);
        public ServiceResultVM GetServiceByID(int id);
        public ServiceEditVM GetServiceByIdEditVm(int id);
        public List<ServiceResultVM> GetServiceIndex(int count, int start);
        public List<ServiceResultVM> Search(string serviceTypeName, int cityId, int count, int start = 0);
    }
}