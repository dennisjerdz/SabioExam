using SabioExam.DATA.Entities;

namespace SabioExam.SERVER.Services
{
    public interface IDiscountService
    {
        public Task<IEnumerable<DiscountCode>> GetDiscountCodesAsync(byte length);
        public Task<bool> GenerateDiscountAsync(ushort count, byte length);
        public Task<byte> UseDiscountAsync(string discountCode);
    }
}
