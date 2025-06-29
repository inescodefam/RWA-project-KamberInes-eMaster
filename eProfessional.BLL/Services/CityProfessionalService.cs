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

        public CityProfessionalService(
            IMapper mapper,
            ICityProfessionalRepository cityProfessionalRepository)
        {
            _mapper = mapper;
            _cityProfessionalRepository = cityProfessionalRepository;
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

            _cityProfessionalRepository.Add(entity);
            _cityProfessionalRepository.Save();
            return model;
        }

        public IEnumerable<CityProfessionalDto> GetCityProfessionals()
        {
            var cityProfessionals = _cityProfessionalRepository.GetAll();
            return _mapper.Map<IEnumerable<CityProfessionalDto>>(cityProfessionals);
        }

        public List<CityDto> GetCitiesByProfessionalId(int professionalId)
        {
            var exists = ProfessionalExists(professionalId);
            if (!exists)
            {
                throw new ArgumentException("Professional does not exist.");
            }
            try
            {

                var cityProfessionals = _cityProfessionalRepository.GetCitiesByProfessionalId(professionalId);

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

        public List<ProfessionalDto> GetProfessionalsByCity(int cityId)
        {
            var exists = CityExists(cityId);
            if (!exists)
            {
                throw new ArgumentException("City does not exist.");
            }

            try
            {
                var cityProfessionals = _cityProfessionalRepository.GetProfessionalsByCity(cityId);
                var professionalIds = cityProfessionals.Select(cp => cp.ProfessionalId).Distinct().ToList();
                if (professionalIds == null || !professionalIds.Any())
                {
                    return new List<ProfessionalDto>();
                }
                var professionals = _professionalRepository.GetProfessionalsByIds(professionalIds);

                return _mapper.Map<List<ProfessionalDto>>(professionals);
            }
            catch (Exception)
            {
                return new List<ProfessionalDto>();
            }

        }

        public CityProfessionalDto UpdateCityProfessional(int id, CityProfessionalDto model)
        {
            var existsId = ExistCityProfessionalId(id);
            if (!existsId)
            {
                throw new ArgumentException("CityProfessional does not exist.");
            }

            var existingModel = _cityProfessionalRepository.GetById(id);
            if (existingModel == null)
            {
                throw new ArgumentException("CityProfessional does not exist.");
            }

            existingModel.ProfessionalId = model.ProfessionalId;
            existingModel.CityId = model.CityId;

            try
            {

                _cityProfessionalRepository.Update(existingModel);
                _cityProfessionalRepository.Save();

                return _mapper.Map<CityProfessionalDto>(existingModel);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Unable to update professionals cities.");
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

        private bool ProfessionalExists(int professionalId)
        {
            return _cityProfessionalRepository.ProfessionalExists(professionalId);
        }

        private bool CityExists(int cityId)
        {
            return _cityProfessionalRepository.CityExists(cityId);
        }

    }
}
