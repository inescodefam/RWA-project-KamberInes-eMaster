namespace WebApp.Models
{
    public class CityIndexVM
    {
        public string NewCityName { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public List<CityVM> Cities { get; set; } = new List<CityVM>();
        public string SearchTerm { get; internal set; }
    }


    public class CityVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
