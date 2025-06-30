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

        public List<CityProfessional> UpdateCitiesForProfessional(int id, List<City> cities)
        {
            var cityProfessionals = _context.CityProfessionals
                .Where(cp => cp.ProfessionalId == id)
                .ToList();

            foreach (var cityProfessional in cityProfessionals)
            {
                _context.CityProfessionals.Remove(cityProfessional);
            }

            foreach (var city in cities)
            {
                var newCityProfessional = new CityProfessional
                {
                    ProfessionalId = id,
                    CityId = city.Idcity
                };
                _context.CityProfessionals.Add(newCityProfessional);
            }
            _context.SaveChanges();


            cityProfessionals = _context.CityProfessionals
                .Where(cp => cp.ProfessionalId == id)
                .ToList();

            return cityProfessionals;
        }


        public List<CityProfessional> UpdateProfessionalsForCity(int id, List<Professional> professionals)
        {
            var cityProfessionals = _context.CityProfessionals
                .Where(cp => cp.CityId == id)
                .ToList();
            foreach (var cityProfessional in cityProfessionals)
            {
                _context.CityProfessionals.Remove(cityProfessional);
            }
            foreach (var professional in professionals)
            {
                var newCityProfessional = new CityProfessional
                {
                    CityId = id,
                    ProfessionalId = professional.IdProfessional
                };
                _context.CityProfessionals.Add(newCityProfessional);
            }
            _context.SaveChanges();
            cityProfessionals = _context.CityProfessionals
                .Where(cp => cp.CityId == id)
                .ToList();

            return cityProfessionals;

        }

        public bool DeleteCitiesForProfessional(int id)
        {
            var cityProfessionals = _context.CityProfessionals
                .Where(cp => cp.ProfessionalId == id)
                .ToList();
            if (!cityProfessionals.Any())
            {
                return false;
            }
            foreach (var cityProfessional in cityProfessionals)
            {
                _context.CityProfessionals.Remove(cityProfessional);
            }
            _context.SaveChanges();
            return true;
        }


        public bool DeleteProfessionalsForCity(int id)
        {
            var cityProfessionals = _context.CityProfessionals
                .Where(cp => cp.CityId == id)
                .ToList();
            if (!cityProfessionals.Any())
            {
                return false;
            }
            foreach (var cityProfessional in cityProfessionals)
            {
                _context.CityProfessionals.Remove(cityProfessional);
            }
            _context.SaveChanges();
            return true;
        }

    }
}
