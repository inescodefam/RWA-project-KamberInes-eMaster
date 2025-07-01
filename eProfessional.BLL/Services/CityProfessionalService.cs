using AutoMapper;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using eProfessional.DAL.Interfaces;
using eProfessional.DAL.Models;

namespace eProfessional.BLL.Services
{
    public class CityProfessionalService : ICityProfessionalService
    {
        private readonly IMapper _mapper;
        private readonly ICityProfessionalRepository _cityProfessionalRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IProfessionalRepository _professionalRepository;
        private readonly IUserRepository _userRepository;


        public CityProfessionalService(
            IMapper mapper,
            ICityProfessionalRepository cityProfessionalRepository,
            ICityRepository cityRepository,
            IProfessionalRepository professionalRepository,
            IUserRepository userRepository
            )
        {
            _mapper = mapper;
            _cityProfessionalRepository = cityProfessionalRepository;
            _cityRepository = cityRepository;
            _professionalRepository = professionalRepository;
            _userRepository = userRepository;
        }

        public CityProfessionalDto AddCityProfessional(CityProfessionalDto model)
        {
            var professionalExists = _cityProfessionalRepository.ProfessionalExists(model.ProfessionalId);
            var cityExists = _cityProfessionalRepository.CityExists(model.CityId);

            if (!professionalExists || !cityExists)
                throw new ArgumentException("Professional or City does not exist.");

            var exists = ExsistsCityProfessional(model);
            if (exists)
            {
                throw new InvalidOperationException("This professional is already associated with this city.");
            }
            var entity = _mapper.Map<CityProfessional>(model);

            _cityProfessionalRepository.Add(entity);
            _cityProfessionalRepository.Save();
            return model;
        }

        public List<CityProfessionalDataDto> GetCityProfessionals(int count, int start)
        {
            List<CityProfessional> cityProfessionals = _cityProfessionalRepository.GetAllCityProfessionals(count, start);
            List<CityProfessionalDataDto> cityProfessionalDto = new List<CityProfessionalDataDto>();

            if (cityProfessionals.Count() == 0)
            {
                return cityProfessionalDto;
            }
            cityProfessionalDto = MapToCityProfessionalDataDto(cityProfessionals);

            return cityProfessionalDto;
        }


        public List<CityDto> GetCitiesByProfessional(string professionalName)
        {
            var exists = _cityProfessionalRepository.ProfessionalExists(professionalName);
            if (exists == null)
            {
                throw new ArgumentException("Professional does not exist.");
            }
            try
            {

                var cityProfessionals = _cityProfessionalRepository.GetCitiesByProfessionalId(exists.IdProfessional);

                if (cityProfessionals == null || !cityProfessionals.Any())
                {
                    return new List<CityDto>();
                }
                var cityIds = cityProfessionals.Select(cp => cp.CityId).Distinct().ToList();

                var cities = _cityRepository.GetCitiesByIds(cityIds);

                return _mapper.Map<List<CityDto>>(cities);
            }
            catch (Exception)
            {
                return new List<CityDto>();
            }
        }

        public List<ProfessionalDataDto> GetProfessionalsByCity(string city)
        {
            var exists = _cityProfessionalRepository.CityExists(city);
            if (exists == null)
            {
                throw new ArgumentException("City does not exist.");
            }

            try
            {
                var cityProfessionals = _cityProfessionalRepository.GetProfessionalsByCity(exists.Idcity);

                var professionalIds = cityProfessionals.Select(cp => cp.ProfessionalId).Distinct().ToList();

                if (professionalIds == null || !professionalIds.Any())
                {
                    return new List<ProfessionalDataDto>();
                }

                var professionals = _professionalRepository.GetProfessionalsByIds(professionalIds);
                List<ProfessionalDataDto> professionalData = MapUserDataToProfeassionals(professionals);

                return professionalData;
            }
            catch (Exception)
            {
                return new List<ProfessionalDataDto>();
            }

        }

        public CityProfessionalDataDto UpdateCityProfessional(CityProfessionalDto model)
        {
            if (model == null || model.IdProfessionalCity <= 0)
            {
                throw new ArgumentException("Invalid CityProfessional data.");
            }
            var existingModel = _cityProfessionalRepository.GetById(model.IdProfessionalCity);
            if (existingModel == null)
            {
                throw new ArgumentException("CityProfessional does not exist.");
            }

            try
            {
                _mapper.Map(model, existingModel);
                _cityProfessionalRepository.Save();

                return MapToCityProfessionalDataDto(existingModel);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Unable to update professionals cities.");
            }
        }

        public List<CityProfessionalDataDto> UpdateCitiesByProfessional(int professionalId, List<int> citiesIds)
        {
            var exists = _cityProfessionalRepository.ProfessionalExists(professionalId);
            if (!exists)
            {
                throw new ArgumentException("Professional does not exist.");
            }
            try
            {

                var cityProfessionals = _cityProfessionalRepository.UpdateCitiesForProfessional(professionalId, citiesIds);

                if (cityProfessionals == null || !cityProfessionals.Any())
                {
                    return new List<CityProfessionalDataDto>();
                }

                return MapToCityProfessionalDataDto(cityProfessionals);
            }
            catch (Exception)
            {
                return new List<CityProfessionalDataDto>();
            }
        }


        public List<CityProfessionalDataDto> UpdateProfessionalsByCity(int cityId, List<int> professionalsIds)
        {
            var exists = _cityProfessionalRepository.CityExists(cityId);
            if (!exists)
            {
                throw new ArgumentException("Professional does not exist.");
            }
            try
            {

                var cityProfessionals = _cityProfessionalRepository.UpdateProfessionalsForCity(cityId, professionalsIds);

                if (cityProfessionals == null || !cityProfessionals.Any())
                {
                    return new List<CityProfessionalDataDto>();
                }

                return MapToCityProfessionalDataDto(cityProfessionals);
            }
            catch (Exception)
            {
                return new List<CityProfessionalDataDto>();
            }
        }

        public bool DeleteCityProfessional(int idProfessionalCity)
        {
            var existsId = ExistCityProfessionalId(idProfessionalCity);
            if (!existsId)
            {
                throw new ArgumentException("There are no Professionals related to the City relation does not exist.");
            }
            var cityProfessional = _cityProfessionalRepository.GetById(idProfessionalCity);

            try
            {
                _cityProfessionalRepository.Delete(cityProfessional);
                _cityProfessionalRepository.Save();

            }
            catch (Exception)
            {

                throw new InvalidOperationException("Unable to delete.");
            }
            return true;
        }

        public bool DeleteCitiesForProfessional(int professionalId)
        {
            var exists = _cityProfessionalRepository.ProfessionalExists(professionalId);
            if (!exists)
            {
                throw new ArgumentException("Professional does not exist.");
            }
            return _cityProfessionalRepository.DeleteCitiesForProfessional(professionalId);
        }

        public bool DeleteProfessionalsForCity(int cityId)
        {
            var exists = _cityProfessionalRepository.CityExists(cityId);
            if (!exists)
            {
                throw new ArgumentException("City does not exist.");
            }

            return _cityProfessionalRepository.DeleteProfessionalsForCity(cityId);
        }


        // private methods ------------------------------------------

        private bool ExistCityProfessionalId(int idProfessionalCity)
        {
            return _cityProfessionalRepository.ExsistsCityProfessionalId(idProfessionalCity);
        }

        private bool ExsistsCityProfessional(CityProfessionalDto model)
        {
            var exists = _cityProfessionalRepository.ExsistsCityProfessional(new CityProfessional
            {
                ProfessionalId = model.ProfessionalId,
                CityId = model.CityId
            });
            return exists;
        }


        private List<CityProfessionalDataDto> MapToCityProfessionalDataDto(List<CityProfessional> cityProfessionals)
        {
            List<CityProfessionalDataDto> cityProfessionalDataDto = new List<CityProfessionalDataDto>();

            foreach (var cityProfessional in cityProfessionals)
            {
                var cityDto = _mapper.Map<CityDto>(_cityRepository.GetById(cityProfessional.CityId));
                var professionalDto = _mapper.Map<ProfessionalDto>(_professionalRepository.GetById(cityProfessional.ProfessionalId));
                var userDto = _mapper.Map<UserDto>(_userRepository.GetById(professionalDto.UserId));

                cityProfessionalDataDto.Add(new CityProfessionalDataDto
                {
                    IdProfessionalCity = cityProfessional.IdProfessionalCity,
                    ProfessionalId = cityProfessional.ProfessionalId,
                    City = cityDto.Name,
                    CityId = cityProfessional.CityId,
                    Username = userDto.Username,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    Phone = userDto.Phone
                });
            }
            return cityProfessionalDataDto;
        }

        private CityProfessionalDataDto MapToCityProfessionalDataDto(CityProfessional cityProfessional)
        {
            CityProfessionalDataDto cityProfessionalDataDto;

            var cityDto = _mapper.Map<CityDto>(_cityRepository.GetById(cityProfessional.CityId));
            var professionalDto = _mapper.Map<ProfessionalDto>(_professionalRepository.GetById(cityProfessional.ProfessionalId));
            var userDto = _mapper.Map<UserDto>(_userRepository.GetById(professionalDto.UserId));

            cityProfessionalDataDto = new CityProfessionalDataDto
            {
                IdProfessionalCity = cityProfessional.IdProfessionalCity,
                ProfessionalId = cityProfessional.ProfessionalId,
                City = cityDto.Name,
                CityId = cityProfessional.CityId,
                Username = userDto.Username,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Phone = userDto.Phone
            };

            return cityProfessionalDataDto;
        }


        private List<ProfessionalDataDto> MapUserDataToProfeassionals(List<Professional> professionals)
        {
            List<User> users = _userRepository.GetAll().ToList();
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
    }
}
