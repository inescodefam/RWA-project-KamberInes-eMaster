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
        private readonly IUserRepository _userRepository;

        public ProfessionalService(
            IProfessionalRepository professionalRepository,
            IMapper mapper,
            ILogRepository loggingRepository,
            IUserRepository userRepository
            )
        {
            _mapper = mapper;
            _professionalRepository = professionalRepository;
            _loggingRepository = loggingRepository;
            _userRepository = userRepository;
        }

        public List<ProfessionalDataDto> GetProfessionals(int count, int start = 0)
        {
            var professionals = _professionalRepository.GetProfessionals(count, start).ToList();
            var users = _userRepository.GetAll().ToList();

            if (!professionals.Any())
            {
                _loggingRepository.CreateLog("No professionals found in the database.", "info");
                return new List<ProfessionalDataDto>();
            }

            List<ProfessionalDataDto> professionalDtos = MapUserProfessionalList(professionals, users);

            _loggingRepository.CreateLog($"Retrieved {professionalDtos.Count} professionals from the database.", "info");

            return professionalDtos;
        }

        public ProfessionalDataDto GetSingleProfessional(int id)
        {
            var professional = _professionalRepository.GetById(id);
            if (professional == null)
            {
                _loggingRepository.CreateLog($"Professional with ID {id} not found.", "warning");
                throw new Exception($"Professional with ID {id} not found.");
            }

            var user = _userRepository.GetById(professional.UserId);

            var professionalDataDto = MapUserProfessional(professional, user);

            _loggingRepository.CreateLog($"Retrieved professional with ID {id} from the database.", "info");
            return professionalDataDto;
        }

        public List<ProfessionalDataDto> Search(string? name, string? serviceType, int count, int start = 0)
        {

            var professionals = _professionalRepository.SearchProfessionals(name, serviceType, count, start);

            if (professionals == null || professionals.Count == 0)
            {
                _loggingRepository.CreateLog($"No professionals found for name '{name}'.", "info");

                return new List<ProfessionalDataDto>();
            }

            var users = _userRepository.GetAll().ToList();

            List<ProfessionalDataDto> professionalDtos = MapUserProfessionalList(
               professionals,
               users
            );

            _loggingRepository.CreateLog($"Retrieved {professionalDtos.Count} professionals for name '{name}'.", "info");

            return professionalDtos;
        }

        public ProfessionalDto CreateProfessional(ProfessionalBaseDto professionalDto)
        {
            var user = _userRepository.GetById(professionalDto.UserId);
            if (user == null)
            {

                _loggingRepository.CreateLog($"User with ID {user.Iduser} not found for professional creation. Create user for professional.", "warning");
                throw new Exception($"User with username {user.Iduser} not found. Please register user first or create professional from existing user.");
            }

            var exsists = _professionalRepository.GetAll()
                .Any(p => p.UserId == professionalDto.UserId); // inače bi ovo trebalo omogučiti jer zašto ne bi jedan user imao dvije firme ili obrta

            if (!exsists)
            {

                var professional = new Professional
                {
                    UserId = professionalDto.UserId
                };

                _professionalRepository.Add(professional);
                _professionalRepository.Save();

                _loggingRepository.CreateLog($"Professional with ID {professional.IdProfessional} added successfully.", "info");
                return _mapper.Map<ProfessionalDto>(professional);
            }
            else
            {
                _loggingRepository.CreateLog($"Professional with User ID {professionalDto.UserId} already exists.", "warning");
                return null;
                //throw new Exception($"Professional with User ID {professionalDto.UserId} already exists.");
            }
        }

        public bool UpdateProfessional(ProfessionalDataDto professionalDataDto)
        {
            var professional = _professionalRepository.GetById(professionalDataDto.IdProfessional);

            if (professional == null)
            {
                _loggingRepository.CreateLog($"Professional with ID {professionalDataDto.IdProfessional} not found for update.", "warning");
                return false;
            }

            var user = _userRepository.GetById(professionalDataDto.UserId);
            if (user == null)
            {
                _loggingRepository.CreateLog($"User with ID {professionalDataDto.UserId} not found for professional update.", "warning");
                throw new Exception($"User with ID {professionalDataDto.UserId} not found.");
            }

            var newUserDto = MapUserFromProfessionalData(professionalDataDto);
            _mapper.Map(newUserDto, user);
            _userRepository.Save();


            ProfessionalDto professionalDto = new ProfessionalDto
            {
                IdProfessional = professionalDataDto.IdProfessional,
                UserId = professionalDataDto.UserId
            };
            _mapper.Map(professionalDto, professional);
            _professionalRepository.Save();

            _loggingRepository.CreateLog($"Professional with ID {professionalDto.IdProfessional} updated successfully.", "info");

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


        // --------------- private methods ---------------

        private List<ProfessionalDataDto> MapUserProfessionalList(
            List<Professional> professionals,
            List<User> users
            )
        {
            List<ProfessionalDataDto> professionalDtos = new List<ProfessionalDataDto>();

            foreach (var professional in professionals)
            {
                var user = users.FirstOrDefault(u => u.Iduser == professional.UserId);
                if (user != null)
                {
                    var professionalDto = _mapper.Map<ProfessionalDataDto>(professional);
                    professionalDto.UserName = user.Username;
                    professionalDto.Email = user.Email;
                    professionalDto.PhoneNumber = user.Phone ?? "";
                    professionalDto.FirstName = user.FirstName ?? "";
                    professionalDto.LastName = user.LastName ?? "";
                    professionalDtos.Add(professionalDto);
                }
            }

            return professionalDtos;
        }


        private ProfessionalDataDto MapUserProfessional(
            Professional professional,
            User user
            )
        {

            if (user == null)
            {
                _loggingRepository.CreateLog($"User with ID {professional.UserId} not found for professional ID {professional.IdProfessional}.", "warning");
                throw new Exception($"User with ID {professional.UserId} not found.");
            }

            ProfessionalDataDto professionalDto = new ProfessionalDataDto
            {
                IdProfessional = professional.IdProfessional,
                UserId = professional.UserId,
            };
            professionalDto.UserName = user.Username;
            professionalDto.Email = user.Email;
            professionalDto.PhoneNumber = user.Phone ?? "";
            professionalDto.FirstName = user.FirstName ?? "";
            professionalDto.LastName = user.LastName ?? "";

            return professionalDto;

        }

        private UserDto MapUserFromProfessionalData(
            ProfessionalDataDto professionalDto
            )
        {
            UserDto newUser = new UserDto
            {
                Username = professionalDto.UserName,
                Email = professionalDto.Email,
                Phone = professionalDto.PhoneNumber,
                FirstName = professionalDto.FirstName,
                LastName = professionalDto.LastName
            };

            if (professionalDto.UserId != 0)
            {
                newUser.Iduser = professionalDto.UserId;
            }

            return newUser;

        }
    }
}
