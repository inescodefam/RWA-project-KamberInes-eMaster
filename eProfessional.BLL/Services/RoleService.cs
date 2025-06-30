using AutoMapper;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
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
        private readonly IUserRepository _userRepository;

        public RoleService(
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IRoleRepository roleRepository,
            IUserRepository userRepository
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
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
            var user = _userRepository.GetById(roleDto.UserId);
            if (user == null) throw new InvalidOperationException("User does not exist");

            _roleRepository.AddUserRole(_mapper.Map<Role>(roleDto));
            return true;
        }

        public bool UpdateRole(RoleDto newRole)
        {
            if (newRole == null || string.IsNullOrEmpty(newRole.RoleName))
            {
                return false;
            }
            var role = _roleRepository.GetById(newRole.Idrole);
            newRole.UserId = role.UserId;

            if (role == null) throw new InvalidOperationException("Role does not exist!");

            _mapper.Map(newRole, role);
            _roleRepository.Save();
            return true;
        }

        public bool DeleteRole(int roleId)
        {
            Role roletoDelete = _roleRepository.GetById(roleId);
            if (roletoDelete == null)
            {
                throw new InvalidOperationException("Role does not exist!");
            }

            _roleRepository.Delete(roletoDelete);
            _roleRepository.Save();

            return true;
        }

    }
}
