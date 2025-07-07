using eProfessional.DAL.Models;

namespace eProfessional.DAL.Interfaces
{
    public interface IServiceRepository : ICrudRepository<Service>
    {
        public List<Service> Search(string searchTerm, int count, int start);

        public List<Service> GetServices(int count, int start = 0);

        public List<Service> GetServiceByServiceType(string type, int count, int start = 0);

        public int GetServiceByServiceTypeCount(string type);
        public List<Service> GetServicesByProfessionalId(int id);
    }
}
