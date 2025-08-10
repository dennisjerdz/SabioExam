using System.ComponentModel.DataAnnotations;

namespace SabioExam.DATA.Entities
{
    public class DiscountCode
    {
        [Key]
        [MaxLength(8), MinLength(7)]
        public required string Code { get; set; }
        public uint MaxUsage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public List<Transaction>? Transactions { get; set; }
    }
}
