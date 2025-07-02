using WebAPI.DTOs;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Services
{
    public class CityProfessionalService : ICityProfessionalService
    {
        private readonly ApiService _apiFetchService;

        public CityProfessionalService(ApiService api)
        {
            _apiFetchService = api;
        }

        public IEnumerable<CityProfessionalDataVM> GetCityProfessionals(int pageSize, int page)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            int start = (page - 1) * pageSize;
            var url = $"api/city-professional?count={pageSize}&start={start}";
            var response = _apiFetchService.FetchDataList<CityProfessionalApiDto, CityProfessionalDataVM>(url);
            return response;
        }

        public List<ProfessionalDataVM> GetProfessionalsByCity(int city)
        {
            var url = $"api/city-professional/city/{city}";
            var response = _apiFetchService.FetchDataList<ProfessionalApiDataDto, ProfessionalDataVM>(url);
            return response;
        }

        public List<CityVM> GetCitysByProfessional(int professionalId)
        {
            var url = $"api/city-professional/professional/{professionalId}";
            var response = _apiFetchService.FetchDataList<CityApiDto, CityVM>(url);
            return response;
        }

        public List<CityProfessionalDataVM> GetProfessionalData(
            List<ProfessionalVM> professionals,
             int professionalCount,
            int pageSize, int page)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            int start = (page - 1) * pageSize;

            var cityProfessionals = _apiFetchService.FetchDataList<CityProfessionalDataApiDto, CityProfessionalDataVM>($"api/city-professional?count={pageSize}&start={start}");

            return cityProfessionals;
        }

        public CityProfessionalVM AddCityProfessional(CityProfessionalVM model)
        {
            var url = "api/city-professional";
            var response = _apiFetchService.PostData<CityProfessionalApiDto, CityProfessionalVM>(url, model);
            if (response == null)
            {
                throw new Exception("City professional could not be created.");
            }
            return response;
        }

        public List<CityProfessionalDataVM> UpdateCitiesByProfessional(int professionalId, List<int> citiesIds)
        {
            var url = $"api/city-professional/professional/{professionalId}";
            var response = _apiFetchService.PutDataList<List<int>, List<CityProfessionalDataVM>>(url, citiesIds);
            if (response == null)
            {
                throw new Exception("Cities for the professional could not be updated.");
            }
            return response;
        }
        public List<CityProfessionalDataVM> UpdateProfessionalsByCity(int cityId, List<int> professionalsIds)
        {
            var url = $"api/city-professional/city/{cityId}";
            var response = _apiFetchService.PutDataList<List<int>, List<CityProfessionalDataVM>>(url, professionalsIds);
            if (response == null)
            {
                throw new Exception("Professionals for the city could not be updated.");
            }
            return response;
        }

        public CityProfessionalDataVM UpdateCityProfessional(int id, CityProfessionalDataVM model)
        {

            var url = $"api/city-professional";
            var response = _apiFetchService.PutData<CityProfessionalApiDto, CityProfessionalDataVM>(url, model);
            if (response == null)
            {
                throw new Exception("City professional could not be updated.");
            }
            return response;
        }

        public bool DeleteCityProfessional(int idProfessionalCity)
        {
            var url = $"api/city-professional/{idProfessionalCity}";
            var response = _apiFetchService.DeleteData(url);
            if (!response)
            {
                throw new Exception("City professional could not be deleted.");
            }
            return response;
        }

        public bool DeleteCitiesForProfessional(int professionalId)
        {
            var url = $"api/city-professional/{professionalId}";
            var response = _apiFetchService.DeleteData(url);
            if (!response)
            {
                throw new Exception("Cities for the professional could not be deleted.");
            }
            return response;
        }


        public bool DeleteProfessionalsForCity(int cityId)
        {
            var url = $"api/city-professional/city/{cityId}";
            var response = _apiFetchService.DeleteData(url);
            if (!response)
            {
                throw new Exception("Professionals for the city could not be deleted.");
            }
            return response;
        }


    }
}
