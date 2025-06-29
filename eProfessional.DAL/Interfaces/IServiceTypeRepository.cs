using eProfessional.DAL.Models;

namespace eProfessional.DAL.Interfaces
{
    public interface IServiceTypeRepository : ICrudRepository<ServiceType>
    {
        public List<ServiceType> GetServiceTypes(int count, int start);

    }
}
