using AutoMapper;
using Shared.BL.DTOs;
using Shared.BL.Models;
using Shared.BL.Services;
using WebAPI.Context;

namespace WebAPI.Services
{
    public class ServiceTypesServicecs : IServiceType
    {
        private readonly EProfessionalContext _context;
        private readonly IMapper _mapper;

        public ServiceTypesServicecs(EProfessionalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ServiceTypeDto>> GetServiceTypes(int count, int start)
        {
            var serviceTypes = _context.ServiceTypes
                     .Skip(start * count)
                     .Take(count)
                     .Select(st => new ServiceTypeDto
                     {
                         IdserviceType = st.IdserviceType,
                         ServiceTypeName = st.ServiceTypeName
                     })
                     .ToList();

            var serviceTypesDto = _mapper.Map<List<ServiceTypeDto>>(serviceTypes);
            return serviceTypesDto;
        }

        public Task<ServiceTypeDto> GetServiceTypeById(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<ServiceTypeDto> CreateServiceType(ServiceTypeDto serviceTypeDto)
        {
            var entity = new ServiceType
            {
                ServiceTypeName = serviceTypeDto.ServiceTypeName
            };

            _context.ServiceTypes.Add(entity);
            _context.SaveChanges();

            serviceTypeDto.IdserviceType = entity.IdserviceType;

            return serviceTypeDto;
        }
        public async Task<ServiceTypeDto> UpdateServiceType(ServiceTypeDto serviceTypeDto)
        {
            ServiceType entity = _context.ServiceTypes.FirstOrDefault(st => st.IdserviceType == serviceTypeDto.IdserviceType);

            if (entity == null)
            {
                throw new KeyNotFoundException("Service type not found.");
            }
            entity.ServiceTypeName = serviceTypeDto.ServiceTypeName;
            await _context.SaveChangesAsync();
            entity = _context.ServiceTypes.FirstOrDefault(st => st.IdserviceType == serviceTypeDto.IdserviceType);
            ServiceTypeDto updatedServiceTypeDto = _mapper.Map<ServiceTypeDto>(entity);
            return updatedServiceTypeDto;
        }

        public async Task<bool> DeleteServiceType(int id)
        {
            _context.ServiceTypes.Remove(new ServiceType { IdserviceType = id });
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
