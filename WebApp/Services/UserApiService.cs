using AutoMapper;
using Shared.BL.DTOs;
using Shared.BL.Services;
using WebApp.Models;

namespace WebApp.Services
{
    public class UserApiService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly ApiFetchService _apiFetchService;

        public UserApiService(IHttpClientFactory httpClientFactory, IMapper mapper, ApiFetchService apiFetchService)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _mapper = mapper;
            _apiFetchService = apiFetchService;
        }
        public Task<List<UserDto>> GetUsers(int count, int start = 0)
        {
            var users = _apiFetchService.FetchList<UserDto, UserVM>($"api/user?count={count}&start={start}");
            return _mapper.Map<Task<List<UserDto>>>(users);
        }

        public Task<UserDto> GetUserById(int id)
        {
            var user = _apiFetchService.Fetch<UserDto, UserVM>($"api/user/{id}");
            return _mapper.Map<Task<UserDto>>(user);
        }

        public bool UpdateUser(UserDto userDto)
        {
            var response = _httpClient.PutAsync("api/user", JsonContent.Create(userDto));
            return response.Result.IsSuccessStatusCode;
        }

        public void DeleteUser(UserDto userDto)
        {
            var response = _httpClient.DeleteAsync($"api/user/{userDto.Iduser}");
            if (!response.Result.IsSuccessStatusCode)
                throw new Exception("Unable to delete user.");
        }

        public Task<UserDto> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

    }
}
