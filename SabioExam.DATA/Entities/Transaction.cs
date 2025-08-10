using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SabioExam.DATA.Entities
{
    public class Transaction
    {
        [Key]
        public long TransactionId { get; set; }

        [ForeignKey("DiscountCode")]
        public required string Code { get; set; }

        public DiscountCode? DiscountCode { get; }

        public DateTime CreatedAt { get; set; }
    }
}
