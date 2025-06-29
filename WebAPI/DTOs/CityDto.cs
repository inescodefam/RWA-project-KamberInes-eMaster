namespace Shared.BL.DTOs
{
    public class CityDto
    {
        public int? Idcity { get; set; }
        public string Name { get; set; }
    }

    public class CreateCityDto
    {
        public string Name { get; set; }
    }
}