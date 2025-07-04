using AutoMapper;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using eProfessional.DAL.Interfaces;
using eProfessional.DAL.Models;

namespace eProfessional.BLL.Services
{
    public class ServiceTypesServicecs : IServiceType
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IMapper _mapper;

        public ServiceTypesServicecs(IServiceTypeRepository serviceTypeRepository, IMapper mapper)
        {
            _mapper = mapper;
            _serviceTypeRepository = serviceTypeRepository;
        }

        public List<ServiceTypeDto> GetServiceTypes(int count, int start)
        {
            var serviceTypes = _serviceTypeRepository.GetServiceTypes(count, start);

            if (serviceTypes == null || !serviceTypes.Any())
            {
                return new List<ServiceTypeDto>();
            }
            var serviceTypesDto = _mapper.Map<List<ServiceTypeDto>>(serviceTypes);
            return serviceTypesDto;
        }

        public ServiceTypeDto GetServiceTypeById(int id)
        {
            var serviceType = _serviceTypeRepository.GetById(id);
            if (serviceType == null)
            {
                throw new KeyNotFoundException("Service type not found.");
            }
            var serviceTypeDto = _mapper.Map<ServiceTypeDto>(serviceType);
            return serviceTypeDto;
        }


        public ServiceTypeDto CreateServiceType(ServiceTypeDto serviceTypeDto)
        {
            var entity = new ServiceType
            {
                ServiceTypeName = serviceTypeDto.ServiceTypeName
            };

            var existsingServiceType = _serviceTypeRepository.GetServiceTypes(10000, 0).Any(st => st.ServiceTypeName == serviceTypeDto.ServiceTypeName);

            if (existsingServiceType)
            {
                throw new InvalidOperationException("Service type with this name already exists.");
            }

            _serviceTypeRepository.Add(entity);
            _serviceTypeRepository.Save();

            serviceTypeDto.IdserviceType = entity.IdserviceType;

            return serviceTypeDto;
        }
        public ServiceTypeDto UpdateServiceType(ServiceTypeDto serviceTypeDto)
        {
            ServiceType entity = _serviceTypeRepository.GetById(serviceTypeDto.IdserviceType);

            if (entity == null)
            {
                throw new KeyNotFoundException("Service type not found.");
            }
            _mapper.Map(serviceTypeDto, entity);
            _serviceTypeRepository.Save();

            entity = _serviceTypeRepository.GetById(serviceTypeDto.IdserviceType);
            ServiceTypeDto updatedServiceTypeDto = _mapper.Map<ServiceTypeDto>(entity);
            return updatedServiceTypeDto;
        }

        public bool DeleteServiceType(int id)
        {
            try
            {
                var serviceType = _serviceTypeRepository.GetById(id);
                _serviceTypeRepository.Delete(serviceType);
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
