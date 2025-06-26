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

        public async Task<CityProfessionalDto> AddCityProfessionalAsync(CityProfessionalDto model)
        {
            var professionalExists = await ProfessionalExists(model.ProfessionalId);
            var cityExists = await CityExists(model.CityId);
            if (!professionalExists || !cityExists)
                throw new ArgumentException("Professional or City does not exist.");

            var exists = await ExsistsCityProfessional(model);
            if (exists)
            {
                throw new InvalidOperationException("This professional is already associated with this city.");
            }
            var entity = _mapper.Map<CityProfessional>(model);

            await _context.CityProfessionals.AddAsync(entity);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<IEnumerable<CityProfessionalDto>> GetCityProfessionalsAsync()
        {
            var cityProfessionals = await _context.CityProfessionals.ToListAsync();
            return _mapper.Map<IEnumerable<CityProfessionalDto>>(cityProfessionals);
        }

        public async Task<List<CityProfessionalDto?>> GetCitysByProfessionalAsync(int professionalId)
        {
            var exists = await ProfessionalExists(professionalId);
            if (!exists)
            {
                throw new ArgumentException("Professional does not exist.");
            }
            var cityProfessionals = await _context.CityProfessionals
                .Where(cp => cp.ProfessionalId == professionalId)
                .ToListAsync();
            return _mapper.Map<List<CityProfessionalDto?>>(cityProfessionals);
        }

        public async Task<List<CityProfessionalDto>> GetProfessionalsByCityAsync(int cityId)
        {
            var exists = await CityExists(cityId);
            if (!exists)
            {
                throw new ArgumentException("City does not exist.");
            }
            var cityProfessionals = await _context.CityProfessionals
                .Where(cp => cp.CityId == cityId)
                .ToListAsync();
            return _mapper.Map<List<CityProfessionalDto>>(cityProfessionals);
        }

        public async Task<CityProfessionalDto> UpdateCityProfessionalAsync(int id, CityProfessionalDto model)
        {
            var existsId = await ExistCityProfessionalId(id);
            if (!existsId)
            {
                throw new ArgumentException("CityProfessional does not exist.");
            }

            var existingModel = await _context.CityProfessionals.FirstOrDefaultAsync(cp => cp.IdProfessionalCity == id);
            if (existingModel == null)
            {
                throw new ArgumentException("CityProfessional does not exist.");
            }

            existingModel.ProfessionalId = model.ProfessionalId;
            existingModel.CityId = model.CityId;
            //_context.CityProfessionals.Update(exist);
            await _context.SaveChangesAsync();
            return _mapper.Map<CityProfessionalDto>(existingModel);
        }

        public async Task<bool> DeleteCityProfessionalAsync(int idProfessionalCity)
        {
            var existsId = await ExistCityProfessionalId(idProfessionalCity);
            if (!existsId)
            {
                throw new ArgumentException("City Professional relation does not exist.");
            }
            var cityProfessional = await _context.CityProfessionals.FirstOrDefaultAsync(cp => cp.IdProfessionalCity == idProfessionalCity);
            if (cityProfessional == null)
            {
                throw new ArgumentException("City Professional relation does not exist.");
            }
            _context.CityProfessionals.Remove(cityProfessional);
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<bool> ExistCityProfessionalId(int idProfessionalCity)
        {
            return await _context.CityProfessionals.AnyAsync(cp => cp.IdProfessionalCity == idProfessionalCity);
        }

        private async Task<bool> ExsistsCityProfessional(CityProfessionalDto model)
        {
            return await _context.CityProfessionals.AnyAsync(cp => cp.ProfessionalId == model.ProfessionalId && cp.CityId == model.CityId);
        }

        private async Task<bool> ProfessionalExists(int? professionalId)
        {
            if (professionalId == null)
                return false;
            return await _context.Professionals.AnyAsync(p => p.IdProfessional == professionalId);
        }

        private async Task<bool> CityExists(int? cityId)
        {
            if (cityId == null)
                return false;
            return await _context.Cities.AnyAsync(c => c.Idcity == cityId);
        }

    }
}
