using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared.BL.DTOs;
using Shared.BL.Models;
using Shared.BL.Services;
using WebAPI.Context;

namespace WebAPI.Services
{
    public class ProfessionalService : IProfessionalService
    {
        private readonly EProfessionalContext _context;
        private readonly IMapper _mapper;
        private readonly LogService _loggingService;
        private readonly ICityProfessionalService _cityProfessionalService;
        private readonly ICityService _cityService;

        public ProfessionalService(EProfessionalContext context, IMapper mapper, LogService logService, ICityProfessionalService cityProfessionalService, ICityService cityService)
        {
            _context = context;
            _mapper = mapper;
            _loggingService = logService;
            _cityProfessionalService = cityProfessionalService;
            _cityService = cityService;
        }
        public async Task<List<ProfessionalDto>> GetProfessionals(int count, int start = 0)
        {
            var professionals = await _context.Professionals.Skip(start * count).Take(count).ToListAsync();

            if (!professionals.Any())
            {
                _loggingService.Log("No professionals found in the database.", "info");
                return new List<ProfessionalDto>();
            }

            var citiesProfessional = await _cityProfessionalService.GetCityProfessionalsAsync();
            var cities = await _cityService.GetAllCitiesAsync();

            var professionalDtos = _mapper.Map<List<ProfessionalDto>>(professionals);

            foreach (var professionalDto in professionalDtos)
            {
                professionalDto.Cities = citiesProfessional
                    .Where(cp => cp.ProfessionalId == professionalDto.IdProfessional)
                    .Select(cp => new CityDto
                    {
                        Idcity = cp.CityId,
                        Name = cities.FirstOrDefault(c => c.Idcity == cp.CityId)?.Name
                    }).ToList();
            }

            _loggingService.Log($"Retrieved {professionalDtos.Count} professionals from the database.", "info");

            return professionalDtos;
        }


        public async Task<ProfessionalDto> GetSingleProfessional(int id)
        {
            var professional = await _context.Professionals.FindAsync(id);
            if (professional == null)
            {
                _loggingService.Log($"Professional with ID {id} not found.", "warning");
                throw new Exception($"Professional with ID {id} not found.");
            }

            var professionalDto = _mapper.Map<ProfessionalDto>(professional);
            var citiesProfessional = await _cityProfessionalService.GetCityProfessionalsAsync();
            var cities = await _cityService.GetAllCitiesAsync();
            professionalDto.Cities = citiesProfessional
                .Where(cp => cp.ProfessionalId == professionalDto.IdProfessional)
                .Select(cp => new CityDto
                {
                    Idcity = cp.CityId,
                    Name = cities.FirstOrDefault(c => c.Idcity == cp.CityId)?.Name
                }).ToList();

            _loggingService.Log($"Retrieved professional with ID {id} from the database.", "info");
            return professionalDto;
        }

        public async Task<List<ProfessionalDto>> Search(string? name, string? cityName, int count, int start = 0)
        {
            var query = _context.Professionals.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.User.FirstName == name || p.User.LastName == name);
            }

            var professionals = await query.Skip(start * count).Take(count).ToListAsync();

            if (professionals == null || professionals.Count == 0)
            {
                _loggingService.Log($"No professionals found for name '{name}' and city '{cityName}'.", "info");
                throw new Exception("No professionals found");
            }

            var professionalDtos = _mapper.Map<List<ProfessionalDto>>(professionals);

            var citiesProfessional = await _cityProfessionalService.GetCityProfessionalsAsync();
            var cities = await _cityService.GetAllCitiesAsync();
            foreach (var professionalDto in professionalDtos)
            {
                professionalDto.Cities = citiesProfessional
                    .Where(cp => cp.ProfessionalId == professionalDto.IdProfessional)
                    .Select(cp => new CityDto
                    {
                        Idcity = cp.CityId,
                        Name = cities.FirstOrDefault(c => c.Idcity == cp.CityId)?.Name
                    }).ToList();
            }

            _loggingService.Log($"Retrieved {professionalDtos.Count} professionals for name '{name}' and city '{cityName}'.", "info");

            return professionalDtos;
        }

        public async Task<bool> CreateProfessional(ProfessionalDto professionalDto)
        {
            var professional = _mapper.Map<Professional>(professionalDto);
            if (!_context.Users.Any(u => u.Iduser == professionalDto.UserId))
            {
                throw new ArgumentException("Invalid User ID");
            }

            await _context.Professionals.AddAsync(professional);
            _context.SaveChanges();
            _loggingService.Log($"Professional with ID {professional.IdProfessional} added successfully.", "info");

            return true;
        }

        public async Task<bool> UpdateProfessional(int id, ProfessionalDto professionalDto)
        {
            var professional = await _context.Professionals.FindAsync(id);
            var cities = professionalDto.Cities;
            var citiesDtos = await _cityService.GetAllCitiesAsync();

            if (cities != null && cities.Count > 0)
            {
                foreach (var city in cities)
                {
                    if (city.Idcity != 0 && citiesDtos.Contains(city))
                    {
                        _loggingService.Log($"City with ID {city.Idcity} already exist.", "warning");
                        return false;
                    }
                    else
                    {
                        var result = await _cityService.CreateCityAsync(city.Name);
                        if (result == null)
                        {
                            _loggingService.Log($"City with {city.Name} can not be added.", "warning");
                            return false;
                        }

                        var cityProfessionalDto = new CityProfessionalDto
                        {
                            ProfessionalId = professional.IdProfessional,
                            CityId = result.Idcity
                        };

                        var cityProfessional = await _cityProfessionalService.AddCityProfessionalAsync(cityProfessionalDto);
                        if (cityProfessional == null)
                        {
                            _loggingService.Log($"CityProfessional with Professional ID {professional.IdProfessional} and City ID {result.Idcity} can not be added.", "warning");
                            return false;

                        }

                    }
                }
            }

            if (professional == null)
            {
                _loggingService.Log($"Professional with ID {id} not found for update.", "warning");
                return false;
            }
            professional.UserId = professionalDto.UserId;

            //_context.Professionals.Update(professional);
            await _context.SaveChangesAsync();
            _loggingService.Log($"Professional with ID {id} updated successfully.", "info");

            return true;
        }

        public bool DeleteProfessional(int id)
        {
            var professional = _context.Professionals.Find(id);
            var cityProfessionals = _context.CityProfessionals.Where(cp => cp.ProfessionalId == id).ToList();
            var services = _context.Services.Where(s => s.ProfessionalId == id).ToList();
            if (professional == null)
            {
                _loggingService.Log($"Professional with ID {id} not found for deletion.", "warning");
                return false;
            }

            try
            {
                _context.Services.RemoveRange(services);
                _context.CityProfessionals.RemoveRange(cityProfessionals);
                _context.Professionals.Remove(professional);
                _context.SaveChanges();

                _loggingService.Log($"Professional with ID {id} deleted successfully.", "info");
                return true;
            }
            catch
            {
                _loggingService.Log($"Error deleting professional with ID {id}.", "error");
                throw new Exception($"Error deleting professional with ID {id}.");
            }

        }

    }
}
