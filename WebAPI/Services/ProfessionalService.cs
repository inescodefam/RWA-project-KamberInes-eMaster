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

        public ProfessionalService(EProfessionalContext context, IMapper mapper, LogService logService)
        {
            _context = context;
            _mapper = mapper;
            _loggingService = logService;
        }
        public async Task<List<ProfessionalDto>> GetProfessionals(int count, int start = 0)
        {
            var professionals = await _context.Professionals.Skip(start * count).Take(count).ToListAsync();

            if (!professionals.Any())
            {
                _loggingService.Log("No professionals found in the database.", "info");
                return new List<ProfessionalDto>();
            }

            var professionalDto = _mapper.Map<List<ProfessionalDto>>(professionals);

            _loggingService.Log($"Retrieved {professionalDto.Count} professionals from the database.", "info");

            return professionalDto;
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
            if (!string.IsNullOrEmpty(cityName))
            {
                query = query.Where(p => p.City.Name.Contains(cityName));
            }

            var professionals = await query.Skip(start * count).Take(count).ToListAsync();
            if (professionals == null || professionals.Count == 0)
            {
                _loggingService.Log($"No professionals found for name '{name}' and city '{cityName}'.", "info");
                throw new Exception("No professionals found");
            }

            var professionalDtos = _mapper.Map<List<ProfessionalDto>>(professionals);
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

            if (!_context.Cities.Any(c => c.Idcity == professionalDto.CityId))
            {
                throw new ArgumentException("Invalid City ID");
            }

            await _context.Professionals.AddAsync(professional);
            _context.SaveChanges();
            _loggingService.Log($"Professional with ID {professional.IdProfessional} added successfully.", "info");

            return true;
        }
        public async Task<bool> UpdateProfessional(int id, ProfessionalDto professionalDto)
        {
            var professional = await _context.Professionals.FindAsync(id);
            if (professional == null)
            {
                _loggingService.Log($"Professional with ID {id} not found for update.", "warning");
                return false;
            }
            professional.UserId = professionalDto.UserId;
            professional.CityId = professionalDto.CityId;

            //_context.Professionals.Update(professional);
            await _context.SaveChangesAsync();
            _loggingService.Log($"Professional with ID {id} updated successfully.", "info");

            return true;
        }

        public bool DeleteProfessional(int id)
        {
            var professional = _context.Professionals.Find(id);
            if (professional == null)
            {

                _loggingService.Log($"Professional with ID {id} not found for deletion.", "warning");
                return false;
            }

            try
            {
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
