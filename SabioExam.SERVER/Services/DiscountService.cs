using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

using SabioExam.DATA;
using SabioExam.DATA.Entities;
using SabioExam.SHARED.Utilities.Constants;

namespace SabioExam.SERVER.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(DataContext dataContext, ILogger<DiscountService> logger)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        async Task<IEnumerable<DiscountCode>> IDiscountService.GetDiscountCodesAsync(byte length)
        {
            return await _dataContext.DiscountCodes
                .Where(d=>d.Code.Length == length)
                .Take(1000)
                .ToListAsync();
        }

        async Task<bool> IDiscountService.GenerateDiscountAsync(ushort count, byte length)
        {
            if (count < 1000 || count > 2000 || 
                length < 7 || length > 8)
                return false;

            Guid guid = Guid.NewGuid();
            var stopwatch = Stopwatch.StartNew();
            var generatedCodes = new List<string>();
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var task = Task.Run(async () =>
            {
                while (generatedCodes.Count < count)
                {
                    try
                    {
                        var discountCode = new DiscountCode
                        {
                            Code = GenerateRandomCode(length),
                            CreatedAt = DateTime.UtcNow
                        };

                        _dataContext.DiscountCodes.Add(discountCode);
                        await _dataContext.SaveChangesAsync();
                        generatedCodes.Add(discountCode.Code);

                        Console.WriteLine($"ThreadId {Environment.CurrentManagedThreadId} Guid {guid} - Generated discount code: {discountCode.Code}");
                    }
                    catch (DbUpdateException)
                    {
                        continue;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred while generating discount codes.");
                        _dataContext.DiscountCodes.RemoveRange(_dataContext.DiscountCodes.Where(dc => generatedCodes.Contains(dc.Code)));
                        await _dataContext.SaveChangesAsync();
                        cts.Cancel();
                        break;
                    }
                }
            }, token);

            await task;
            stopwatch.Stop();
            generatedCodes.Clear();
            Console.WriteLine($"ThreadId {Environment.CurrentManagedThreadId} Guid {guid} - Time Elapsed {stopwatch.Elapsed}");
            
            return !cts.IsCancellationRequested && task.IsCompletedSuccessfully;
        }

        async Task<byte> IDiscountService.UseDiscountAsync(string discountCode)
        {
            if (String.IsNullOrWhiteSpace(discountCode))
                return DiscountHubUseCodeResultCodes.INVALID_CODE;

            try
            {
                var code = await _dataContext.DiscountCodes.FindAsync(discountCode);
                if (code != null)
                {
                    _dataContext.Transactions.Add(new Transaction
                    {
                        Code = code.Code,
                        CreatedAt = DateTime.UtcNow
                    });
                    await _dataContext.SaveChangesAsync();
                    return DiscountHubUseCodeResultCodes.SUCCESS;
                }
                else
                    return DiscountHubUseCodeResultCodes.INVALID_CODE;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while using discount codes.");
                return DiscountHubUseCodeResultCodes.ERROR;
            }
        }

        private string GenerateRandomCode(byte length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
