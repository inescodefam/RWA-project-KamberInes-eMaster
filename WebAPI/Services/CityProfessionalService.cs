using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared.BL.DTOs;
using Shared.BL.Services;
using WebAPI.Context;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class CityProfessionalService : ICityProfessionalService
    {
        private readonly EProfessionalContext _context;
        private readonly IMapper _mapper;


        public CityProfessionalService(IMapper mapper, EProfessionalContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public CityProfessionalDto AddCityProfessional(CityProfessionalDto model)
        {
            var professionalExists = ProfessionalExists(model.ProfessionalId);
            var cityExists = CityExists(model.CityId);
            if (!professionalExists || !cityExists)
                throw new ArgumentException("Professional or City does not exist.");

            var exists = ExsistsCityProfessional(model);
            if (exists)
            {
                throw new InvalidOperationException("This professional is already associated with this city.");
            }
            var entity = _mapper.Map<CityProfessional>(model);

            _context.CityProfessionals.Add(entity);
            _context.SaveChanges();
            return model;
        }

        public IEnumerable<CityProfessionalDto> GetCityProfessionals()
        {
            var cityProfessionals = _context.CityProfessionals.ToList();
            return _mapper.Map<IEnumerable<CityProfessionalDto>>(cityProfessionals);
        }

        public List<CityProfessionalDto?> GetCitysByProfessional(int professionalId)
        {
            var exists = ProfessionalExists(professionalId);
            if (!exists)
            {
                throw new ArgumentException("Professional does not exist.");
            }
            var cityProfessionals = _context.CityProfessionals
                .Where(cp => cp.ProfessionalId == professionalId)
                .ToListAsync();
            return _mapper.Map<List<CityProfessionalDto?>>(cityProfessionals);
        }

        public List<CityProfessionalDto> GetProfessionalsByCity(int cityId)
        {
            var exists = CityExists(cityId);
            if (!exists)
            {
                throw new ArgumentException("City does not exist.");
            }
            var cityProfessionals = _context.CityProfessionals
                .Where(cp => cp.CityId == cityId)
                .ToListAsync();
            return _mapper.Map<List<CityProfessionalDto>>(cityProfessionals);
        }

        public CityProfessionalDto UpdateCityProfessional(int id, CityProfessionalDto model)
        {
            var existsId = ExistCityProfessionalId(id);
            if (!existsId)
            {
                throw new ArgumentException("CityProfessional does not exist.");
            }

            var existingModel = _context.CityProfessionals.FirstOrDefault(cp => cp.IdProfessionalCity == id);
            if (existingModel == null)
            {
                throw new ArgumentException("CityProfessional does not exist.");
            }

            existingModel.ProfessionalId = model.ProfessionalId;
            existingModel.CityId = model.CityId;
            _context.SaveChanges();
            return _mapper.Map<CityProfessionalDto>(existingModel);
        }

        public bool DeleteCityProfessional(int idProfessionalCity)
        {
            var existsId = ExistCityProfessionalId(idProfessionalCity);
            if (!existsId)
            {
                throw new ArgumentException("City Professional relation does not exist.");
            }
            var cityProfessional = _context.CityProfessionals.FirstOrDefault(cp => cp.IdProfessionalCity == idProfessionalCity);
            if (cityProfessional == null)
            {
                throw new ArgumentException("City Professional relation does not exist.");
            }
            _context.CityProfessionals.Remove(cityProfessional);
            return _context.SaveChanges() > 0;
        }

        private bool ExistCityProfessionalId(int idProfessionalCity)
        {
            return _context.CityProfessionals.Any(cp => cp.IdProfessionalCity == idProfessionalCity);
        }

        private bool ExsistsCityProfessional(CityProfessionalDto model)
        {
            return _context.CityProfessionals.Any(cp => cp.ProfessionalId == model.ProfessionalId && cp.CityId == model.CityId);
        }

        private bool ProfessionalExists(int? professionalId)
        {
            if (professionalId == null)
                return false;
            return _context.Professionals.Any(p => p.IdProfessional == professionalId);
        }

        private bool CityExists(int? cityId)
        {
            if (cityId == null)
                return false;
            return _context.Cities.Any(c => c.Idcity == cityId);
        }

    }
}
