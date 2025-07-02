using AutoMapper;
using WebAPI.DTOs;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Services
{
    public class ProfessionalServices : IProfessionalService
    {

        private readonly IMapper _mapper;
        private readonly ICityService _cityService;
        private readonly IUserService _userService;
        private readonly ICityProfessionalService _cityProfessionalService;
        private readonly ApiService _apiService;

        public ProfessionalServices(
            IMapper mapper,
            ICityService cityService,
            IUserService userService,
            ICityProfessionalService cityProfessional,
            ApiService apiService
        )
        {

            _mapper = mapper;
            _cityService = cityService;
            _userService = userService;
            _cityProfessionalService = cityProfessional;
            _apiService = apiService;
        }

        public ProfessionalDataVM GetSingleProfessional(int id)
        {
            var response = _apiService.Fetch<ProfessionalApiDataDto, ProfessionalDataVM>($"api/professional/{id}");
            if (response == null)
            {
                throw new Exception($"Professional with ID {id} not found.");
            }
            return response;
        }

        public bool CreateProfessional(CreateProfessionalVM professionalDto)
        {
            var response = _apiService.PostData<CreateProfessionalApiDataDto, CreateProfessionalVM>("api/professional", professionalDto);
            return response != null;
        }


        public List<ProfessionalDataVM> GetProfessionals(int pageSize, int page)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            int start = (page - 1) * pageSize;
            var url = $"api/professional?count={pageSize}&start={start}";
            var response = _apiService.Fetch<List<ProfessionalApiDataDto>, List<ProfessionalDataVM>>(url);
            if (response == null || !response.Any())
            {
                return new List<ProfessionalDataVM>();
            }
            return response;
        }


        public List<ProfessionalDataVM> Search(string? Name, string? cityName, int pageSize, int page)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            int start = (page - 1) * pageSize;
            var url = $"api/professional/search?count={pageSize}&start={start}";

            if (!string.IsNullOrEmpty(Name))
            {
                url += $"&name={Name}";
            }
            if (!string.IsNullOrEmpty(cityName))
            {
                url += $"&cityName={cityName}";
            }

            var response = _apiService.Fetch<List<ProfessionalApiDataDto>, List<ProfessionalDataVM>>(url);
            if (response == null || !response.Any())
            {
                return new List<ProfessionalDataVM>();
            }
            return response;
        }

        public bool UpdateProfessional(ProfessionalDataVM professionalDto)
        {
            var resposne = _apiService.PutData<ProfessionalApiDataDto, ProfessionalDataVM>($"api/professional", professionalDto);

            return resposne != null;
        }
        public bool DeleteProfessional(int id)
        {
            var response = _apiService.DeleteData($"api/professional/{id}");
            if (response)
            {
                throw new Exception($"Professional with ID {id} not found.");
            }
            return response;
        }
    }
}
