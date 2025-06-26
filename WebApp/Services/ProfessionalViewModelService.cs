using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.BL.DTOs;
using Shared.BL.Services;
using WebApp.Models;

namespace WebApp.Services
{
    public class ProfessionalViewModelService : IProfessionalViewModelService
    {
        private readonly ApiFetchService _apiFetchService;
        private readonly IMapper _mapper;
        private readonly ICityProfessionalService _cityProfessionalService;

        public ProfessionalViewModelService(ApiFetchService apiFetchService, IMapper mapper, IHttpClientFactory httpClient, ICityProfessionalService cityProfessional)
        {
            _apiFetchService = apiFetchService;
            _mapper = mapper;
            _cityProfessionalService = cityProfessional;
        }


        public async Task<ProfessionalIndexVM> GetProfessionalIndexVM(
            List<ProfessionalVM> professionals,
             int professionalCount,
            int userCount = 1000,
            int cityCount = 1000)
        {

            var users = await _apiFetchService.FetchDataList<UserDto, UserVM>(
                $"api/user?count={userCount}&start=0");

            var cities = await _apiFetchService.FetchDataList<CityDto, CityVM>(
                $"api/city?count={cityCount}&start=0");

            users = users ?? new List<UserVM>();
            cities = cities ?? new List<CityVM>();

            var userDict = users.ToDictionary(u => u.Iduser, u => u.Username);
            var cityDict = cities.ToDictionary(c => c.Idcity, c => c.Name);
            var citiesProfessional = await _cityProfessionalService.GetCityProfessionalsAsync();
            var professionalCityDict = citiesProfessional
                .GroupBy(cp => cp.ProfessionalId)
                .ToDictionary(g => g.Key, g => g.Select(cp => cp.CityId).ToList());

            foreach (var p in professionals)
            {
                p.Cities = professionalCityDict.TryGetValue(p.IdProfessional, out var cityIds)
                    ? cityIds.Select(cityId => new CityVM
                    {
                        Idcity = cityId,
                        Name = cityDict.TryGetValue(cityId, out var cityName) ? cityName : "Unknown"
                    }).ToList()
                    : new List<CityVM>();
                p.UserName = p.UserId.HasValue && userDict.TryGetValue(p.UserId.Value, out var userName)
                    ? userName : "Unknown";

                p.CityNames = professionalCityDict.TryGetValue(p.IdProfessional, out var cityIdForNames)
                     ? cityIdForNames
                         .Where(cityId => cityId.HasValue && cityDict.ContainsKey(cityId.Value))
                         .Select(cityId => cityDict[cityId.Value])
                         .ToList()
                     : new List<string>();
            }

            return new ProfessionalIndexVM
            {
                Professionals = professionals,
                Users = users.Select(u => new SelectListItem
                {
                    Value = u.Iduser.ToString(),
                    Text = $"{u.Username} ({u.FirstName} {u.LastName})"
                }).ToList(),
                Cities = cities.Select(c => new SelectListItem
                {
                    Value = c.Idcity.ToString(),
                    Text = c.Name
                }).ToList()
            };
        }
    }
}
