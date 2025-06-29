using eProfessional.DAL.Context;
using eProfessional.DAL.Interfaces;
using eProfessional.DAL.Models;

namespace eProfessional.DAL.Repositories
{
    public class CityProfessionalRepository : CrudRepository<CityProfessional>, ICityProfessionalRepository
    {
        public CityProfessionalRepository(EProfessionalContext context) : base(context)
        { }

        public bool ExsistsCityProfessional(CityProfessional model)
        {
            return _context.CityProfessionals.Any(cp => cp.ProfessionalId == model.ProfessionalId && cp.CityId == model.CityId);
        }

        public bool ExsistsCityProfessionalId(int cityProfessionalId)
        {
            return _context.CityProfessionals.Any(cp => cp.IdProfessionalCity == cityProfessionalId);
        }

        public bool ProfessionalExists(int professionalId)
        {
            return _context.Professionals.Any(p => p.IdProfessional == professionalId);
        }

        public bool CityExists(int cityId)
        {
            return _context.Cities.Any(c => c.Idcity == cityId);
        }

        public List<CityProfessional> GetCitiesByProfessionalId(int professionalId)
        {
            return _context.CityProfessionals
                .Where(cp => cp.ProfessionalId == professionalId)
                .ToList();
        }

        public List<CityProfessional> GetProfessionalsByCity(int cityId)
        {
            return _context.CityProfessionals
                .Where(cp => cp.CityId == cityId)
                .ToList();
        }
    }
}
