using Shared.BL.Models;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class ServicesService
    {
        private readonly EProfessionalContext _context;

        public ServicesService(EProfessionalContext context)
        {
            _context = context;
        }

        public List<Service> GetServices(int count, int start = 0)
        {
            try
            {
                return _context.Services
                    .Skip(start * count)
                    .Take(count)
                    .ToList();
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while retrieving the services.");
            }
        }

        public List<Service> GetServiceByServiceType(string type)
        {
            try
            {
                var serviceTypeIds = _context.ServiceTypes
                       .Where(st => st.ServiceTypeName == type)
                       .Select(st => (int?)st.IdserviceType);

                var services = _context.Services
                    .Where(s => serviceTypeIds.Contains(s.ServiceTypeId))
                    .ToList();

                return services;
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while retrieving the service by service type.");
            }
        }

        public Service CreateService(Service service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service), "Service cannot be null.");
            }
            try
            {
                _context.Services.Add(service);
                _context.SaveChanges();
                return service;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the service.", ex);
            }
        }

        public void UpdateService(int id, Service service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service), "Service cannot be null.");
            }
            try
            {
                _context.Services.Update(service);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the service.", ex);
            }
        }

        public void DeleteService(int id)
        {
            try
            {
                var service = _context.Services.Find(id);
                if (service == null)
                {
                    throw new KeyNotFoundException($"Service with ID {id} not found.");
                }
                _context.Services.Remove(service);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the service.", ex);
            }
        }

    }
}
