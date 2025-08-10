namespace SabioExam.SHARED.Utilities.Constants
{
    public class DiscountHubFunctions
    {
        // Server-bound
        public const string SERVER_MESSAGE_SEND = "SendMessage";
        public const string SERVER_DISCOUNT_GENERATE = "GenerateDiscount";
        public const string SERVER_DISCOUNT_USE = "UseDiscount";

        // Client-bound
        public const string CLIENT_MESSAGE_RECEIVE = "ReceiveMessage";
        public const string CLIENT_DISCOUNT_GENERATE_RESULT = "GenerateDiscountResult";
        public const string CLIENT_DISCOUNT_USE_RESULT = "UseDiscountResult";
    }
}
