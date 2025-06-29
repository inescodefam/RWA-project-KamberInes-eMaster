using eProfessional.DAL.Models;

namespace eProfessional.DAL.Interfaces
{
    public interface ICityRepository : ICrudRepository<City>
    {
        public int GetIdByName(string name);
        public List<City> Search(string? searchTerm, int count, int start = 0);

        public List<City> GetCitiesByIds(List<int> ids);
    }
}
