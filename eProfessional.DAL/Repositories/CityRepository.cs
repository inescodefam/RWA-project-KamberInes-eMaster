using eProfessional.DAL.Context;
using eProfessional.DAL.Interfaces;
using eProfessional.DAL.Models;

namespace eProfessional.DAL.Repositories
{
    public class CityRepository : CrudRepository<City>, ICityRepository
    {
        public CityRepository(EProfessionalContext context) : base(context)
        {
        }

        public int GetIdByName(string name)
        {
            var query = _context.Cities.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Name == name);
            }
            var city = query.FirstOrDefault();

            if (city != null)
            {
                return city.Idcity;
            }
            return -1;
        }

        public List<City> Search(string? searchTerm, int count, int start = 0)
        {
            var query = _context.Cities.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.Name.Contains(searchTerm));
            }
            return query.Skip(start * count).Take(count).ToList() ?? new List<City>();
        }

        public int GetCount(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return _context.Cities.Count();
            }
            var query = _context.Cities.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.Name.Contains(searchTerm));
            }
            return query.Count();
        }

        public List<City> GetCitiesByIds(List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return new List<City>();
            }
            return _context.Cities.Where(c => ids.Contains(c.Idcity)).ToList()
                ?? new List<City>();
        }
    }
}
