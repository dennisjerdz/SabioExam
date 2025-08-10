using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;

using SabioExam.SHARED.Models;
using SabioExam.SHARED.Utilities.Constants;
using SabioExam.SERVER.Services;

namespace SabioExam.SERVER.Utilities.Classes
{
    public class DiscountHub : Hub
    {
        private readonly IDiscountService _discountService;

        public DiscountHub(IDiscountService discountService)
        {
            _discountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
        }

        public async Task SendMessage(string message)
        {
            string connectionId = Context.ConnectionId;
            await Clients.All.SendAsync(DiscountHubFunctions.CLIENT_MESSAGE_RECEIVE, connectionId, message);
        }

        public async Task GenerateDiscount(DiscountHubGenerateRequest request)
        {
            if (IsValid<DiscountHubGenerateRequest>(request))
            {
                var result = await _discountService.GenerateDiscountAsync(request.Count, request.Length);
                await Clients.Caller.SendAsync(DiscountHubFunctions.CLIENT_DISCOUNT_GENERATE_RESULT, new DiscountHubGenerateResponse { Result = true });
            }
            else
                await Clients.Caller.SendAsync(DiscountHubFunctions.CLIENT_DISCOUNT_GENERATE_RESULT, new DiscountHubGenerateResponse { Result = false });
        }

        public async Task UseDiscount(DiscountHubUseCodeRequest request)
        {
            if (IsValid<DiscountHubUseCodeRequest>(request))
            {
                var result = await _discountService.UseDiscountAsync(request.Code);
                await Clients.Caller.SendAsync(DiscountHubFunctions.CLIENT_DISCOUNT_USE_RESULT, new DiscountHubUseCodeResponse { Result = result });
            }
            else
                await Clients.Caller.SendAsync(DiscountHubFunctions.CLIENT_DISCOUNT_USE_RESULT, new DiscountHubUseCodeResponse { Result = DiscountHubUseCodeResultCodes.INVALID_CODE });
        }

        private bool IsValid<T>(T objectToValidate)
        {
            ValidationContext context = new ValidationContext(objectToValidate!);
            List<ValidationResult> results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(objectToValidate!, context, results, validateAllProperties: true);
            return isValid;
        }
    }
}
