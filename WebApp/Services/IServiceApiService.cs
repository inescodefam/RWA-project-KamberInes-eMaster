using WebApp.Models;

namespace WebApp.Services
{
    public interface IServiceApiService
    {
        public Task<HttpResponseMessage> GetServiceIndex(int count, int start);
        public Task<ServiceSearchVM> SearchAsync(int? cityId, string serviceTypeName, int count = 1000, int start = 0);

        public Task<ServiceCreateVM> GetCreateServiceView();

        public Task<ServiceCreateVM> CreateService(ServiceCreateVM vm);

        public Task<ServiceEditVM> GetEditView();

        public Task<HttpResponseMessage> EditService(ServiceEditVM vm);

        public Task<bool> DeleteService(int id);
    }
}