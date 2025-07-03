using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public bool CreateProfessional(ProfessionalBaseVm professionalVm) // ProfessionalVM
        {
            try
            {
                var response = _apiService.PostData<ProfessionalApiDto, ProfessionalBaseVm>("api/professional", professionalVm);

                var professionals = _apiService.FetchDataList<ProfessionalApiDataDto, ProfessionalDataVM>("api/professional?count=10000&start=0");

                // i dont like this but... no time for fixing it now

                var professional = professionals.FirstOrDefault(p => p.UserId == professionalVm.UserId);

                foreach (var cityId in professionalVm.CityIds)
                {
                    _cityProfessionalService.AddCityProfessional(new CityProfessionalVM
                    {
                        ProfessionalId = professional.IdProfessional,
                        CityId = cityId
                    });
                }

                return response != null;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public ProfessionalIndexVM GetProfessionals(int pageSize, int page)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            int start = (page - 1) * pageSize;
            var url = $"api/professional?count={pageSize}&start={start}";
            var response = _apiService.Fetch<List<ProfessionalApiDataDto>, List<ProfessionalDataVM>>(url);
            if (response == null || !response.Any())
            {
                return new ProfessionalIndexVM
                {
                    Professionals = new List<ProfessionalVM>(),
                    Users = new List<SelectListItem>(),
                    Cities = new List<SelectListItem>()
                };
            }
            ProfessionalIndexVM professionalIndexVM = MapProfessionalDataModelToIndexModel(response);

            return professionalIndexVM;
        }

        public ProfessionalIndexVM Search(string? Name, string? cityName, int pageSize, int page)
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
                return new ProfessionalIndexVM
                {
                    Professionals = new List<ProfessionalVM>(),
                    Users = new List<SelectListItem>(),
                    Cities = new List<SelectListItem>()
                };
            }
            ProfessionalIndexVM professionalIndexVM = MapProfessionalDataModelToIndexModel(response);


            return professionalIndexVM;
        }

        public bool UpdateProfessional(ProfessionalDataVM professionalDto) //ProfessisionalVM
        {
            var resposne = _apiService.PutData<ProfessionalApiDataDto, ProfessionalDataVM>($"api/professional", professionalDto);

            return resposne != null;
        }
        public bool DeleteProfessional(int id)
        {
            var response = _apiService.DeleteData($"api/professional/{id}");
            if (!response)
            {
                throw new Exception($"Professional with ID {id} not found.");
            }
            return response;
        }

        // ---------- private methods ----------------------------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------------------------------------------
        private ProfessionalIndexVM MapProfessionalDataModelToIndexModel(List<ProfessionalDataVM> professionalData)
        {
            List<ProfessionalVM> professionals = new List<ProfessionalVM>();
            List<UserVM> users = _userService.GetAllUsers();
            List<CityVM> cities = _cityService.GetAllCities();

            ProfessionalIndexVM model = new();

            foreach (var professional in professionalData)
            {
                List<CityVM> cityList = _cityProfessionalService.GetCitysByProfessional(professional.IdProfessional);
                List<int> cityIds = cityList.Select(c => c.Idcity).ToList();
                List<string> cityNames = cityList.Select(c => c.Name).ToList(); // i dont know why i did this.... but let it be separate for now: TODO!!

                professionals.Add(
                    new ProfessionalVM
                    {
                        IdProfessional = professional.IdProfessional,
                        UserId = professional.UserId,
                        UserName = professional.UserName,
                        CityIds = cityIds,
                        CityNames = cityNames
                    }
                    );

            }
            model = new ProfessionalIndexVM
            {
                Professionals = professionals,
                Users = users.Select(u => new SelectListItem
                {
                    Value = u.Iduser.ToString(),
                    Text = $"{u.FirstName} {u.LastName} ({u.Username})"
                }).ToList(),
                Cities = cities.Select(c => new SelectListItem
                {
                    Value = c.Idcity.ToString(),
                    Text = c.Name
                }).ToList()
            };

            return model;
        }
    }
}
