using eProfessional.DAL.Context;
using eProfessional.DAL.Interfaces;
using eProfessional.DAL.Models;

namespace eProfessional.DAL.Repositories
{
    public class ServiceRepository : CrudRepository<Service>, IServiceRepository
    {
        public ServiceRepository(EProfessionalContext context) : base(context)
        { }

        public List<Service> GetServiceByServiceType(ServiceType type, int count, int start = 0)
        {
            var list = _dbSet
                .Where(s => s.ServiceTypeId == type.IdserviceType)
                .ToList();

            if (list == null)
            {
                new List<Service>();
            }

            return list
                .Skip(start * count)
                .Take(count)
                .ToList();
        }

        public List<Service> GetServices(int count, int start = 0)
        {
            return _dbSet
                .Skip(start * count)
                .Take(count)
                .ToList();
        }

        public List<Service> Search(string searchTerm, int count, int start)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return GetAll().Skip(start * count).Take(count).ToList();
            }
            return _dbSet
                .Where(s => s.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .Skip(start * count)
                .Take(count)
                .ToList();
        }
    }
}
