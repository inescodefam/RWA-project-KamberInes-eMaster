using AutoMapper;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using eProfessional.DAL.Interfaces;
using eProfessional.DAL.Models;

namespace eProfessional.BLL.Services
{
    public class ProfessionalService : IProfessionalService
    {
        private readonly IMapper _mapper;
        private readonly IProfessionalRepository _professionalRepository;
        private readonly ILogRepository _loggingRepository;

        public ProfessionalService(IProfessionalRepository professionalRepository, IMapper mapper, ILogRepository loggingRepository)
        {
            _mapper = mapper;
            _professionalRepository = professionalRepository;
            _loggingRepository = loggingRepository;
        }

        public List<ProfessionalDto> GetProfessionals(int count, int start = 0)
        {
            var professionals = _professionalRepository.GetProfessionals(count, start).ToList();

            if (!professionals.Any())
            {
                _loggingRepository.CreateLog("No professionals found in the database.", "info");
                return new List<ProfessionalDto>();
            }

            var professionalDtos = _mapper.Map<List<ProfessionalDto>>(professionals);

            _loggingRepository.CreateLog($"Retrieved {professionalDtos.Count} professionals from the database.", "info");

            return professionalDtos;
        }


        public ProfessionalDto GetSingleProfessional(int id)
        {
            var professional = _professionalRepository.GetById(id);
            if (professional == null)
            {
                _loggingRepository.CreateLog($"Professional with ID {id} not found.", "warning");
                throw new Exception($"Professional with ID {id} not found.");
            }

            var professionalDto = _mapper.Map<ProfessionalDto>(professional);

            _loggingRepository.CreateLog($"Retrieved professional with ID {id} from the database.", "info");
            return professionalDto;
        }

        public List<ProfessionalDto> Search(string? name, string? cityName, int count, int start = 0)
        {

            var professionals = _professionalRepository.SearchProfessionals(name, cityName, count, start);

            if (professionals == null || professionals.Count == 0)
            {
                _loggingRepository.CreateLog($"No professionals found for name '{name}' and city '{cityName}'.", "info");

                return new List<ProfessionalDto>();
            }

            var professionalDtos = _mapper.Map<List<ProfessionalDto>>(professionals);

            _loggingRepository.CreateLog($"Retrieved {professionalDtos.Count} professionals for name '{name}' and city '{cityName}'.", "info");

            return professionalDtos;
        }

        public bool CreateProfessional(ProfessionalDto professionalDto)
        {
            var professional = new Professional
            {
                UserId = professionalDto.UserId
            };

            _professionalRepository.Add(professional);
            _professionalRepository.Save();

            _loggingRepository.CreateLog($"Professional with ID {professional.IdProfessional} added successfully.", "info");

            return true;
        }

        public bool UpdateProfessional(int id, ProfessionalDto professionalDto)
        {
            var professional = _professionalRepository.GetById(id);

            if (professional == null)
            {
                _loggingRepository.CreateLog($"Professional with ID {id} not found for update.", "warning");
                return false;
            }
            professional.UserId = professionalDto.UserId;

            _professionalRepository.Update(professional);
            _professionalRepository.Save();
            _loggingRepository.CreateLog($"Professional with ID {id} updated successfully.", "info");

            return true;
        }

        public bool DeleteProfessional(int id)
        {
            var professional = _professionalRepository.GetById(id);

            if (professional == null)
            {
                _loggingRepository.CreateLog($"Professional with ID {id} not found for deletion.", "warning");
                return false;
            }

            try
            {
                _professionalRepository.Delete(professional);
                _professionalRepository.Save();

                _loggingRepository.CreateLog($"Professional with ID {id} deleted successfully.", "info");
                return true;
            }
            catch
            {
                _loggingRepository.CreateLog($"Error deleting professional with ID {id}.", "error");
                return false;
            }
        }

    }
}
