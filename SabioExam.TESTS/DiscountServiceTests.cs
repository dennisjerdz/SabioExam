using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SabioExam.DATA;
using SabioExam.DATA.Entities;
using SabioExam.SERVER.Services;
using Microsoft.EntityFrameworkCore.InMemory;
using SabioExam.SHARED.Utilities.Constants;

namespace SabioExam.TESTS
{
    [TestClass]
    public sealed class DiscountServiceTests
    {
        private IDiscountService _discountService;
        private DbContextOptions<DataContext> _options;
        private ILogger<DiscountService> _logger;

        [TestInitialize]
        public void TestMethod1()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;
            
            using (var context = new DataContext(_options))
            {
                if (!context.DiscountCodes.Any())
                {
                    context.DiscountCodes.AddRange(new List<DiscountCode>
                    {
                        new DiscountCode { Code = "777777A", CreatedAt = DateTime.UtcNow },
                        new DiscountCode { Code = "777777B", CreatedAt = DateTime.UtcNow },
                        new DiscountCode { Code = "8888888A", CreatedAt = DateTime.UtcNow },
                        new DiscountCode { Code = "8888888B", CreatedAt = DateTime.UtcNow },
                        new DiscountCode { Code = "8888888C", CreatedAt = DateTime.UtcNow }
                    });
                    context.SaveChanges();
                }
            }

            var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
            _logger = loggerFactory.CreateLogger<DiscountService>();

            var contextProvider = new DataContext(_options);
            _discountService = new DiscountService(contextProvider, _logger);
        }

        [TestMethod]
        [DataRow((byte)7,2)]
        [DataRow((byte)8,3)]
        public async Task GetDiscountCodes_ShouldReturnSuccess(byte length, int count)
        {
            // Act
            var result = await _discountService.GetDiscountCodesAsync(length);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<DiscountCode>));
            Assert.AreEqual(count, result.Count());
        }

        [TestMethod]
        [DataRow((ushort)1000, (byte)7, true)]
        [DataRow((ushort)1999, (byte)8, true)]
        [DataRow((ushort)2000, (byte)8, true)]
        [DataRow((ushort)1000, (byte)6, false)]
        [DataRow((ushort)999, (byte)7, false)]
        [DataRow((ushort)1, (byte)7, false)]
        [DataRow((ushort)0, (byte)0, false)]
        public async Task GenerateDiscountCodes_ShouldReturnSuccess(ushort count, byte length, bool expectedResult)
        {
            // Act
            var result = await _discountService.GenerateDiscountAsync(count, length);

            // Assert
            Assert.IsTrue(result == expectedResult);
        }

        [TestMethod]
        [DataRow("777777A", (byte)DiscountHubUseCodeResultCodes.SUCCESS)]
        [DataRow("777777B", (byte)DiscountHubUseCodeResultCodes.SUCCESS)]
        [DataRow("8888888A", (byte)DiscountHubUseCodeResultCodes.SUCCESS)]
        [DataRow("88888AAA", (byte)DiscountHubUseCodeResultCodes.INVALID_CODE)]
        [DataRow("0", (byte)DiscountHubUseCodeResultCodes.INVALID_CODE)]
        [DataRow("", (byte)DiscountHubUseCodeResultCodes.INVALID_CODE)]
        public async Task UseDiscountCodes_ShouldReturnSuccess(string useCode, byte expectedResult)
        {
            // Act
            var result = await _discountService.UseDiscountAsync(useCode);

            // Assert
            Assert.IsTrue(result == expectedResult);
        }
    }
}
