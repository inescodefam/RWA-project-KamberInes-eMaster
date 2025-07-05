using eProfessional.DAL.Context;
using eProfessional.DAL.Interfaces;
using eProfessional.DAL.Models;

namespace eProfessional.DAL.Repositories
{
    public class ProfessionalRepository : CrudRepository<Professional>, IProfessionalRepository
    {
        public ProfessionalRepository(EProfessionalContext context) : base(context)
        { }

        public List<Professional> GetProfessionals(int count, int start = 0)
        {
            var professionals = _dbSet.Skip(start * count).Take(count).ToList();

            return professionals;
        }

        public List<Professional> Get()
        {
            var professionals = GetAll().ToList();

            return professionals;
        }

        public List<Professional> GetProfessionalsByIds(List<int> ids)
        {
            return _context.Professionals
                .Where(p => ids.Contains(p.IdProfessional))
                .ToList();
        }

        public List<Professional> SearchProfessionals(string? searchTerm, string? cityName, int count, int start = 0)
        {
            var query = _context.Professionals.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.User.FirstName.Contains(searchTerm) || p.User.LastName.Contains(searchTerm) || p.User.Username.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(cityName))
            {
                query = query.Where(p => p.CityProfessionals.Any(cp => cp.City.Name.Equals(cityName)));
            }

            var professionals = query.Skip(start * count).Take(count).Distinct().ToList();

            return professionals ?? new List<Professional>();
        }
    }
}
