using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Services
{
    public class ApiFetchService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public ApiFetchService(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _mapper = mapper;
        }

        public async Task<IActionResult> FetchList<TDto, TVm>(string url, Controller controller)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return controller.RedirectToAction("Login", "Auth");

                response.EnsureSuccessStatusCode();
                var dtos = await response.Content.ReadFromJsonAsync<List<TDto>>();
                var vms = _mapper.Map<List<TVm>>(dtos);
                return controller.View(vms);
            }
            catch
            {
                return controller.RedirectToAction("Login", "Auth");
            }
        }

        public async Task<List<TVm>> FetchDataList<TDto, TVm>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();
                var dtos = await response.Content.ReadFromJsonAsync<List<TDto>>();
                var vms = _mapper.Map<List<TVm>>(dtos);
                return vms;
            }
            catch
            {
                throw new Exception("Failed to fetch data from API. Please check the URL or your network connection.");
            }
        }
    }
}
