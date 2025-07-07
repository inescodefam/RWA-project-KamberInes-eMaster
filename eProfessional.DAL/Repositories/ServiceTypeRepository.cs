using eProfessional.DAL.Context;
using eProfessional.DAL.Interfaces;
using eProfessional.DAL.Models;

namespace eProfessional.DAL.Repositories
{
    public class ServiceTypeRepository : CrudRepository<ServiceType>, IServiceTypeRepository
    {
        public ServiceTypeRepository(EProfessionalContext context) : base(context)
        {
        }

        public List<ServiceType> GetServiceTypes(int count, int start)
        {
            return _context.ServiceTypes
                .Skip(start * count)
                .Take(count)
                .ToList();
        }

        public int ServiceTypeCount()
        {
            return _context.ServiceTypes.Count();
        }
    }
}
