using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ServiceTypeVM
    {
        public int IdserviceType { get; set; }
        [Required(ErrorMessage = "Service type name is required.")]
        [StringLength(100, ErrorMessage = "Service type name cannot be longer than 100 characters.")]
        [Display(Name = "Service type name")]
        public string ServiceTypeName { get; set; }
    }

    public class ServiceTypeIndexVm
    {
        public List<ServiceTypeVM> ServiceTypes { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
    }
}
