namespace WebAPI.DTOs
{
    public class CityApiDto
    {
        public int? Idcity { get; set; }
        public string Name { get; set; }
    }

    public class CreateCityApiDto
    {
        public string Name { get; set; }
    }
}