using AutoMapper;
using Shared.BL.DTOs;
using Shared.BL.Models;
using Shared.BL.Services;
using WebAPI.Context;
using WebAPI.Models;

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

        public List<ProfessionalDto> GetProfessionals(int count, int start = 0)
        {
            var professionals = _context.Professionals.Skip(start * count).Take(count).ToList();

            if (!professionals.Any())
            {
                _loggingService.Log("No professionals found in the database.", "info");
                return new List<ProfessionalDto>();
            }

            var professionalDtos = _mapper.Map<List<ProfessionalDto>>(professionals);

            _loggingService.Log($"Retrieved {professionalDtos.Count} professionals from the database.", "info");

            return professionalDtos;
        }


        public ProfessionalDto GetSingleProfessional(int id)
        {
            var professional = _context.Professionals.Find(id);
            if (professional == null)
            {
                _loggingService.Log($"Professional with ID {id} not found.", "warning");
                throw new Exception($"Professional with ID {id} not found.");
            }

            var professionalDto = _mapper.Map<ProfessionalDto>(professional);

            _loggingService.Log($"Retrieved professional with ID {id} from the database.", "info");
            return professionalDto;
        }

        public List<ProfessionalDto> Search(string? name, string? cityName, int count, int start = 0)
        {
            var query = _context.Professionals.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.User.FirstName == name || p.User.LastName == name);
            }

            var professionals = query.Skip(start * count).Take(count).ToList();

            if (professionals == null || professionals.Count == 0)
            {
                _loggingService.Log($"No professionals found for name '{name}' and city '{cityName}'.", "info");
                throw new Exception("No professionals found");
            }

            var professionalDtos = _mapper.Map<List<ProfessionalDto>>(professionals);

            _loggingService.Log($"Retrieved {professionalDtos.Count} professionals for name '{name}' and city '{cityName}'.", "info");

            return professionalDtos;
        }

        public bool CreateProfessional(ProfessionalDto professionalDto)
        {
            if (!_context.Users.Any(u => u.Iduser == professionalDto.UserId))
            {
                throw new ArgumentException("Invalid User ID");
            }

            var professional = new Professional
            {
                UserId = professionalDto.UserId
            };
            _context.Professionals.Add(professional);
            _context.SaveChanges();

            _loggingService.Log($"Professional with ID {professional.IdProfessional} added successfully.", "info");

            return true;
        }

        public bool UpdateProfessional(int id, ProfessionalDto professionalDto)
        {
            var professional = _context.Professionals.Find(id);


            if (professional == null)
            {
                _loggingService.Log($"Professional with ID {id} not found for update.", "warning");
                return false;
            }
            professional.UserId = professionalDto.UserId;

            _context.SaveChangesAsync();
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
