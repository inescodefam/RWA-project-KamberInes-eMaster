using AutoMapper;
using WebAPI.DTOs;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Services
{
    public class UserService : IUserService
    {

        private readonly IMapper _mapper;
        private readonly ApiService _apiFetchService;

        public UserService(IHttpClientFactory httpClientFactory, IMapper mapper, ApiService apiFetchService)
        {
            _mapper = mapper;
            _apiFetchService = apiFetchService;
        }
        public List<UserVM> GetUsers(int pageSize, int page)
        {
            if (pageSize < 1) pageSize = 10;
            var response = _apiFetchService.FetchDataList<UserApiDto, UserVM>($"api/user?count={pageSize}&start={page}");
            return response ?? new List<UserVM>();
        }

        public List<UserVM> GetAllUsers()
        {
            var response = _apiFetchService.FetchDataList<UserApiDto, UserVM>($"api/user/all");
            return response ?? new List<UserVM>();
        }

        public UserVM GetUserById(int id)
        {
            var resposne = _apiFetchService.Fetch<UserApiDto, UserVM>($"api/user/{id}");
            return resposne;
        }

        public bool UpdateUser(UserVM user)
        {
            var response = _apiFetchService.PutData<UserApiDto, UserVM>("api/user", user);
            if (response == null)
            {
                throw new Exception("Failed to update user.");
            }
            return response != null;
        }

        public bool DeleteUser(int userId)
        {
            var response = _apiFetchService.DeleteData($"api/user/delete/{userId}");
            if (response)
            {
                throw new Exception("Failed to delete user.");
            }
            return response;
        }

        public UserVM GetUserByEmail(string email)
        {
            var response = _apiFetchService.Fetch<UserApiDto, UserVM>($"api/user/email/{email}");
            if (response == null)
            {
                throw new Exception($"User with email {email} not found.");
            }
            return response;
        }

        public UserVM GetUserByUsername(string username)
        {
            var response = _apiFetchService.Fetch<UserApiDto, UserVM>($"api/user/username/{username}");
            if (response == null)
            {
                throw new Exception($"User with username {username} not found.");
            }
            return response;
        }

    }
}
