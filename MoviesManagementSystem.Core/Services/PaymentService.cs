using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using MoviesManagementSystem.Core.Errors;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.IServices;
using MoviesManagementSystem.Core.Models;
using Stripe.BillingPortal;
using Stripe.Checkout;
using System.Net;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;


namespace MoviesManagementSystem.Core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> CreatePaymentSessionAsync(int movieId, string userId, IServiceProvider serviceProvider)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(movieId);
            if (movie == null) throw new NotFoundException("Movie not found.");

            if (movie.IsFree)
                throw new ServiceException((int)HttpStatusCode.BadRequest, "The movie is already free.");

            var existingPayment = await _unitOfWork.Payments.FindAsync(p => p.MovieId == movieId && p.UserId == userId);
            if (existingPayment != null)
                throw new ServiceException((int)HttpStatusCode.BadRequest, "You have already bought this movie.");

            var server = serviceProvider.GetRequiredService<IServer>();
            var serverAddressesFeature = server.Features.Get<IServerAddressesFeature>();
            var thisApiUrl = serverAddressesFeature?.Addresses.FirstOrDefault();

            if (thisApiUrl == null)
                throw new NotFoundException("API URL not found.");

            var sessionUrl = await CreateStripeSessionAsync(thisApiUrl, movieId, userId);
            return sessionUrl;
        }

        private async Task<string> CreateStripeSessionAsync(string thisApiUrl, int movieId, string userId)
        {
            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{thisApiUrl}api/user/success?sessionId={{CHECKOUT_SESSION_ID}}&movieId={movieId}&userId={userId}",
                CancelUrl = $"{thisApiUrl}api/user/failed?sessionId={{CHECKOUT_SESSION_ID}}",
                PaymentMethodTypes = new List<string> { "card", "applepay" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = "price_1PMpjiHo3w5F4xJqCPwyE1l0",
                        Quantity = 1
                    }
                },
                Mode = "payment"
            };

            var sessionService = new SessionService();
            var session = await sessionService.CreateAsync(options);

            return session.Url;
        }

        public async Task<string> CheckoutSuccessAsync(string sessionId, int movieId, string userId)
        {
            var existingPayment = await _unitOfWork.Payments.FindAsync(p => p.MovieId == movieId && p.UserId == userId);
            if (existingPayment != null) throw new ServiceException((int)HttpStatusCode.BadRequest, "The movie is already Paid.");

            var movie = await _unitOfWork.Movies.GetByIdAsync(movieId);
            if (movie == null) throw new NotFoundException("Movie not found.");

            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);

            var payment = new Payment
            {
                Date = DateTime.Now,
                MovieId = movieId,
                UserId = userId,
            };

            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            return "Payment completed.";
        }

        public async Task<string> CheckoutFailedAsync(string sessionId)
        {
            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);

            return "Payment canceled.";
        }
    }
}
