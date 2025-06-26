using Shared.BL.DTOs;
using Shared.BL.Services;

namespace WebApp.Services
{
    public class RoleApiService : IRoleService
    {
        private readonly HttpClient _httpClient;

        public RoleApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<bool> AssignRoleToUser(RoleDto roleDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/role", roleDto);
            return response.IsSuccessStatusCode;
        }

        public Task<bool> DeleteRole(int roleId)
        {
            throw new NotImplementedException();
        }

        public string GetUserRole()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateRole(int roleId, string newRoleName)
        {
            throw new NotImplementedException();
        }
    }
}
