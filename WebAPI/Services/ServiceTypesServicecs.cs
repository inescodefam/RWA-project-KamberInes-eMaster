using AutoMapper;
using Shared.BL.DTOs;
using Shared.BL.Models;
using Shared.BL.Services;
using WebAPI.Models;

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

        public Task<bool> DeleteServiceType(int id)
        {
            throw new NotImplementedException();
        }



        public Task<ServiceTypeDto> UpdateServiceType(ServiceTypeDto serviceTypeDto)
        {
            throw new NotImplementedException();
        }
    }
}
