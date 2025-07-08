using eProfessional.DAL.Models;

namespace eProfessional.DAL.Interfaces
{
    public interface IProfessionalRepository : ICrudRepository<Professional>
    {
        public List<Professional> GetProfessionals(int count, int start = 0);
        public List<Professional> Get();
        public List<Professional> SearchProfessionals(string? searchTerm, string? serviceType, int count, int start = 0);
        public int SearchCount(string? searchTerm, string? serviceType);
        public List<Professional> GetProfessionalsByIds(List<int> ids);
    }
}
