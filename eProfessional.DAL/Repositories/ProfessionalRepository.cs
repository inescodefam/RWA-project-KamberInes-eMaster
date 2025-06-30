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

        public List<Professional> GetProfessionalsByIds(List<int> ids)
        {
            return _context.Professionals
                .Where(p => ids.Contains(p.IdProfessional))
                .ToList();
        }

        public List<Professional> SearchProfessionals(string? searchTerm, string? serviceType, int count, int start = 0)
        {
            var query = _context.Professionals.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.User.FirstName == searchTerm || p.User.LastName == searchTerm);
            }

            if (!string.IsNullOrEmpty(serviceType))
            {
                query = query.Where(p => p.Services.Any(s => s.ServiceType.ServiceTypeName == serviceType));
            }

            var professionals = query.Skip(start * count).Take(count).Distinct().ToList();

            return professionals ?? new List<Professional>();
        }
    }
}
