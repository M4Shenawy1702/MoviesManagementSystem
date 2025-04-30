namespace MoviesManagementSystem.Core.IServices
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentSessionAsync(int movieId, string userId, IServiceProvider serviceProvider);
        Task<string> CheckoutSuccessAsync(string sessionId, int movieId, string userId);
        Task<string> CheckoutFailedAsync(string sessionId);
    }
}
