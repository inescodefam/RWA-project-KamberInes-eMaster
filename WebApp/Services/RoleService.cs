using AutoMapper;
using WebAPI.DTOs;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Services
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly ApiService _apiService;
        public RoleService(IMapper mapper, ApiService api)
        {
            _mapper = mapper;
            _apiService = api;
        }

        public bool AssignRoleToUser(RoleVM roleVM)
        {
            var response = _apiService.PostData<RoleApiDto, RoleVM>("api/role", roleVM);
            if (response == null)
            {
                throw new Exception("Failed to assign role to user.");
            }
            return true;
        }

        public bool DeleteRole(int roleId)
        {
            var response = _apiService.DeleteData($"api/role/{roleId}");
            if (!response)
            {
                throw new Exception($"Failed to delete role with ID {roleId}.");
            }
            return true;
        }

        public List<RoleVM> GetUserRole()
        {
            var response = _apiService.FetchDataList<RoleApiDto, RoleVM>("api/role");
            if (response == null || !response.Any())
            {
                return new List<RoleVM>();
            }
            return response;
        }

        public bool UpdateRole(RoleVM roleVM)
        {
            var response = _apiService.PutData<RoleApiDto, RoleVM>($"api/role", roleVM);
            if (response == null)
            {
                throw new Exception($"Failed to update role.");
            }
            return true;
        }
    }
}
