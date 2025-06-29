using AutoMapper;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using eProfessional.DAL.Context;
using eProfessional.DAL.Interfaces;
using eProfessional.DAL.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace eProfessional.BLL.Services
{
    public class RoleService : IRoleService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;

        public RoleService(EProfessionalContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, IRoleRepository roleRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        public string GetCurrentUserRole()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userRole = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(userRole))
            {
                return "";
            }

            return userRole;
        }

        public List<RoleDto> GetUserRole()
        {
            var userRole = _roleRepository.GetAll();
            var userRolesDtos = _mapper.Map<List<RoleDto>>(userRole);
            if (userRole == null || userRole.Count() == 0)
            {
                return new List<RoleDto>();
            }

            return userRolesDtos;
        }

        public bool AssignRoleToUser(RoleDto roleDto)
        {
            _roleRepository.AddUserRole(_mapper.Map<Role>(roleDto));
            return true;
        }

        public bool UpdateRole(RoleDto newRole)
        {
            if (newRole == null || string.IsNullOrEmpty(newRole.RoleName))
            {
                return false;
            }
            _roleRepository.Update(_mapper.Map<Role>(newRole));
            _roleRepository.Save();
            return true;
        }

        public bool DeleteRole(int roleId)
        {
            Role roletoDelete = _roleRepository.GetById(roleId);
            if (roletoDelete == null)
            {
                return false;
            }

            _roleRepository.Delete(roletoDelete);
            _roleRepository.Save();

            return true;
        }

    }
}
