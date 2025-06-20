using AutoMapper;

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

        public async Task<List<TVm>> FetchList<TDto, TVm>(string url) // add controller parameter if needed
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                //if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                //    return controller.RedirectToAction("Login", "Auth");

                response.EnsureSuccessStatusCode();
                var dtos = await response.Content.ReadFromJsonAsync<List<TDto>>();
                var vms = _mapper.Map<List<TVm>>(dtos);
                // return controller.View(vms);
                return vms;
            }
            catch
            {
                throw new Exception("Failed to fetch data from API. Please check the URL or your network connection.");
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

        internal async Task<T2> Fetch<T1, T2>(string v)
        {
            try
            {
                var response = await _httpClient.GetAsync(v);
                response.EnsureSuccessStatusCode();
                var dto = await response.Content.ReadFromJsonAsync<T1>();
                var vm = _mapper.Map<T2>(dto);
                return vm;
            }
            catch (Exception)
            {
                throw new Exception("Failed to fetch data from API. Please check the URL or your network connection.");
            }
        }
    }
}
