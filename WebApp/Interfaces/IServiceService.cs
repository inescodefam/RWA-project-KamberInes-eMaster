using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface IServiceService
    {
        public ServiceCreateVM CreateService(ServiceCreateVM vm);
        public bool DeleteService(int id);
        public void EditService(ServiceEditVM vm);
        public ServiceResultVM GetServiceByID(int id);
        public List<ServiceResultVM> GetServiceIndex(int count, int start);
        public List<ServiceResultVM> Search(string serviceTypeName, int count, int start = 0);
    }
}