using eProfessional.DAL.Models;

namespace eProfessional.DAL.Interfaces
{
    public interface IProfessionalRepository : ICrudRepository<Professional>
    {
        public List<Professional> GetProfessionals(int count, int start = 0);
        public List<Professional> SearchProfessionals(string? searchTerm, string? cityName, int count, int start = 0);
    }
}
