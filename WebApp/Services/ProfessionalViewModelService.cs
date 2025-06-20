using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.BL.DTOs;
using WebApp.Models;

namespace WebApp.Services
{
    public class ProfessionalViewModelService
    {
        private readonly ApiFetchService _apiFetchService;
        private readonly IMapper _mapper;

        public ProfessionalViewModelService(ApiFetchService apiFetchService, IMapper mapper)
        {
            _apiFetchService = apiFetchService;
            _mapper = mapper;
        }

        public async Task<ProfessionalIndexVM> GetProfessionalIndexVM(
            int professionalCount = 50,
            int professionalStart = 0,
            int userCount = 1000,
            int cityCount = 1000)
        {
            

            var professionals = await _apiFetchService.FetchDataList<ProfessionalDto, ProfessionalVM>(
                $"api/professional?count={professionalCount}&start={professionalStart}");

            var users = await _apiFetchService.FetchDataList<UserDto, UserVM>(
                $"api/user?count={userCount}&start=0");

            var cities = await _apiFetchService.FetchDataList<CityDto, CityVM>(
                $"api/city?count={cityCount}&start=0");

            professionals = professionals ?? new List<ProfessionalVM>();
            users = users ?? new List<UserVM>();
            cities = cities ?? new List<CityVM>();

            var userDict = users.ToDictionary(u => u.Iduser, u => u.Username);
            var cityDict = cities.ToDictionary(c => c.Idcity, c => c.Name);

            foreach (var p in professionals)
            {
                p.UserName = p.UserId.HasValue && userDict.TryGetValue(p.UserId.Value, out var userName)
                    ? userName : "Unknown";

                p.CityName = p.CityId.HasValue && cityDict.TryGetValue(p.CityId.Value, out var cityName)
                    ? cityName : "Unknown";
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
