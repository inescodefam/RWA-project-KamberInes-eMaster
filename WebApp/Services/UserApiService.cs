using AutoMapper;
using Shared.BL.DTOs;
using Shared.BL.Services;

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
        public async Task<List<UserDto>> GetUsers(int count, int start = 0)
        {
            var response = await _httpClient.GetAsync($"api/user?count={count}&start={start}");
            response.EnsureSuccessStatusCode();
            var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
            return users;
        }

        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _httpClient.GetAsync($"api/user/{id}");
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
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

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var response = await _httpClient.GetAsync($"api/user/email/{email}");
            response.EnsureSuccessStatusCode();
            UserDto userDto = await response.Content.ReadFromJsonAsync<UserDto>();
            return userDto;
        }

    }
}
