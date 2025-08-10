namespace SabioExam.SERVER.Utilities.Classes
{
    public class ParametersGenerateDiscount
    {
        public ParametersGenerateDiscount(short count, byte length)
        {
            Count = count;
            Length = length;
        }
        public short Count { get; set; }
        public byte Length { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
