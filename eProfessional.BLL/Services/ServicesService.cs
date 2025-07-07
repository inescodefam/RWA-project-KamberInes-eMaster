using AutoMapper;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using eProfessional.DAL.Interfaces;
using eProfessional.DAL.Models;

namespace eProfessional.BLL.Services
{
    public class ServicesService : IServiceService
    {
        private readonly IMapper _mapper;
        private readonly IServiceRepository _serviceRepository;
        private readonly IProfessionalRepository _professionalRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;

        public ServicesService(
            IServiceRepository serviceRepository,
            IProfessionalRepository professionalRepository,
            IServiceTypeRepository serviceType,
            IMapper mapper)
        {
            _mapper = mapper;
            _serviceRepository = serviceRepository;
            _professionalRepository = professionalRepository;
            _serviceTypeRepository = serviceType;
        }

        public List<ServiceDto> SearchServices(string searchTerm, int count, int start)
        {
            var services = _serviceRepository.Search(searchTerm, count, start);
            if (services == null || !services.Any())
            {
                return new List<ServiceDto>();
            }

            try
            {
                return _mapper.Map<List<ServiceDto>>(services);
            }
            catch (Exception)
            {
                return new List<ServiceDto>();
            }
        }

        public int GetServicesCount()
        {
            return _serviceRepository.GetAll().Count();
        }


        public List<ServiceDto> GetServicesCount(int count, int start = 0)
        {
            try
            {
                var services = _serviceRepository.GetServices(count, start);
                var servicesDtos = _mapper.Map<List<ServiceDto>>(services);
                return servicesDtos ?? new List<ServiceDto>();
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while retrieving the services.");
            }
        }

        public List<ServiceDto> GetServiceByServiceType(string type, int count, int start = 0)
        {
            try
            {
                var services = _serviceRepository.GetServiceByServiceType(type, count, start);

                var servicesDtos = _mapper.Map<List<ServiceDto>>(services);
                return servicesDtos;
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while retrieving the service by service type.");
            }
        }

        public int GetServiceByServiceTypeCount(string type)
        {
            return _serviceRepository.GetServiceByServiceTypeCount(type);
        }

        public ServiceDto CreateService(ServiceDto serviceDto)
        {
            if (serviceDto == null)
            {
                throw new ArgumentNullException(nameof(serviceDto), "Service cannot be null.");
            }
            if (_professionalRepository.GetById(serviceDto.ProfessionalId) == null)
                throw new InvalidOperationException("Professional doesn't exist.");

            if (_serviceTypeRepository.GetById(serviceDto.ServiceTypeId) == null)
                throw new InvalidOperationException("Service type doesn't exist");

            try
            {
                Service service = _mapper.Map<Service>(serviceDto);
                _serviceRepository.Add(service);
                _serviceRepository.Save();

                serviceDto.IdService = service.IdService;
                return serviceDto;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the service.", ex);
            }
        }

        public void UpdateService(ServiceDto serviceDto)
        {
            if (serviceDto == null)
            {
                throw new ArgumentNullException(nameof(serviceDto), "Service cannot be null.");
            }
            try
            {
                var existingService = _serviceRepository.GetById(serviceDto.IdService);
                if (existingService == null)
                    throw new Exception($"Service with ID {serviceDto.IdService} not found.");

                _mapper.Map(serviceDto, existingService);

                _serviceRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the service.", ex);
            }
        }

        public bool DeleteService(int id)
        {
            try
            {
                var service = _serviceRepository.GetById(id);

                if (service == null)
                {
                    throw new KeyNotFoundException($"Service with ID {id} not found.");
                }
                _serviceRepository.Delete(service);
                _serviceRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the service.", ex);
            }
            return true;
        }

        public ServiceDto GetServiceByServiceId(int id)
        {

            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id), "Service id cannot be 0.");
            }
            try
            {
                ServiceDto serviceDto = _mapper.Map<ServiceDto>(_serviceRepository.GetById(id));
                return serviceDto;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the service.", ex);
            }
        }

        public List<ServiceDto> GetServicesByProfessionalId(int id)
        {
            if (id == 0)
            {
                return new List<ServiceDto>();
            }
            try
            {
                var services = _serviceRepository.GetServicesByProfessionalId(id);
                if (services == null || !services.Any())
                {
                    return new List<ServiceDto>();
                }
                return _mapper.Map<List<ServiceDto>>(services);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the services by professional ID.", ex);
            }
        }
    }
}
