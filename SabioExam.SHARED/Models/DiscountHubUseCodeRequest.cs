using System.ComponentModel.DataAnnotations;

namespace SabioExam.SHARED.Models
{
    public record DiscountHubUseCodeRequest
    {
        [MaxLength(8)]
        [MinLength(7)]
        public required string Code { get; set; }
    }
}
