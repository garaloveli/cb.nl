using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Insurance.Api.Models
{
    public class InsuranceRequestDto
    {
        [Required]
        public int? ProductId { get; set; }
    }
}
