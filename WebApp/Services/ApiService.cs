using AutoMapper;
using System.Text.Json;

namespace WebApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public ApiService(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _mapper = mapper;
        }

        public List<TVm> FetchDataList<TDto, TVm>(string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = _httpClient.Send(request);
                if (!response.IsSuccessStatusCode || response == null)
                {
                    return new List<TVm>();
                }
                var stream = response.Content.ReadAsStream();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var dtos = JsonSerializer.Deserialize<List<TDto>>(stream, options);
                //var dtos = JsonSerializer.Deserialize<List<TDto>>(stream);
                var vms = _mapper.Map<List<TVm>>(dtos);
                return vms;
            }
            catch
            {
                throw new Exception("Failed to fetch data from API. Please check the URL or your network connection.");
            }
        }

        internal TVm Fetch<TDto, TVm>(string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = _httpClient.Send(request);
                response.EnsureSuccessStatusCode();

                var stream = response.Content.ReadAsStream();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var dtos = JsonSerializer.Deserialize<TDto>(stream, options);

                var vm = _mapper.Map<TVm>(dtos);
                return vm;
            }
            catch (Exception)
            {
                throw new Exception("Failed to fetch data from API. Please check the URL or your network connection.");
            }
        }

        internal TVm PostData<TDto, TVm>(string url, TVm model)
        {
            var dto = _mapper.Map<TDto>(model);
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(JsonSerializer.Serialize(dto), System.Text.Encoding.UTF8, "application/json")
                };
                var response = _httpClient.Send(request);

                if (response.IsSuccessStatusCode)
                {
                    var stream = response.Content.ReadAsStream();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var resultDto = JsonSerializer.Deserialize<TVm>(stream, options);
                    return _mapper.Map<TVm>(resultDto);
                }
                else
                {
                    throw new Exception($"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to post data to API. {ex.Message}");
            }

        }

        internal bool DeleteData(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            try
            {
                var response = _httpClient.Send(request);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete data from API. {ex.Message}");
            }
        }

        internal TVm PutDataList<T1, TVm>(string url, T1 citiesIds)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(citiesIds), System.Text.Encoding.UTF8, "application/json")
            };

            try
            {
                var response = _httpClient.Send(request);
                if (response.IsSuccessStatusCode)
                {
                    var stream = response.Content.ReadAsStream();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var resultDto = JsonSerializer.Deserialize<TVm>(stream, options);
                    return _mapper.Map<TVm>(resultDto);
                }
                else
                {
                    throw new Exception($"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update data in API. {ex.Message}");
            }
        }

        internal TVm PutData<TDto, TVm>(string url, TVm model)
        {
            var dto = _mapper.Map<TDto>(model);
            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(dto), System.Text.Encoding.UTF8, "application/json")
            };
            try
            {
                var response = _httpClient.Send(request);
                if (response.IsSuccessStatusCode)
                {
                    var stream = response.Content.ReadAsStream();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var resultDto = JsonSerializer.Deserialize<TVm>(stream, options);
                    return _mapper.Map<TVm>(resultDto);
                }
                else
                {
                    throw new Exception($"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update data in API. {ex.Message}");
            }
        }
    }
}
