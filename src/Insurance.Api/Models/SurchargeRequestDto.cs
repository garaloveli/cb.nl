using System.ComponentModel.DataAnnotations;

namespace Insurance.Api.Models
{
    public class SurchargeRequestDto
    {
        [Required]
        public int? ProductTypeId { get; set; }
        [Required]
        public float? SurchargeRate { get; set; }
    }
}
