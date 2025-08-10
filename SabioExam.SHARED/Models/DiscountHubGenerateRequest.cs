using System.ComponentModel.DataAnnotations;

namespace SabioExam.SHARED.Models
{
    public class DiscountHubGenerateRequest
    {
        [Range(1000,2000)]
        public ushort Count { get; set; }
        [Range(7,8)]
        public byte Length { get; set; }
    }
}
