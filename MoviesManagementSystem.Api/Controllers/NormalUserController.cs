using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesManagementSystem.Core.Dots;
using MoviesManagementSystem.Core.Dtos;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.EF.Context;
using MoviesManagementSystem.EF.Models;
using Stripe.Checkout;

namespace MoviesManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NormalUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthRepository _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private List<string> _AllowedExtensions = new List<string> { ".jpg", ".png" };
        private long _MaxAllowedSize = 10485760;
        private static string s_wasmClientURL = string.Empty;
        public NormalUserController(UserManager<ApplicationUser> userManager, IAuthRepository authService, IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork, ApplicationDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _authService = authService;
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
            _context = context;
            _configuration = configuration;
        }

        [HttpPut("Addlike/{MovieId}")]
        public async Task<IActionResult> Addlike(int MovieId)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(MovieId);
            if (movie == null) return NotFound();

            movie.Likes++;
            _unitOfWork.Movies.Update(movie);
            _unitOfWork.Complete();

            return Ok(movie);
        } 
        [HttpPost("MakeReview/{MovieId}")]
        public async Task<IActionResult> MakeReview([FromForm]AddReviewDto Dto, int MovieId)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(MovieId);
            if (movie == null) return NotFound();

            var Review = await _unitOfWork.MovieReviews.FindAsync(a => a.UserId == Dto.UserId);
            if (Review != null) return BadRequest("You Already Review this Movie ... ");

            var Rewiew = new MovieReview
            {
                MovieId = MovieId,
                Content = Dto.Content,
                Rating = Dto.Rating,
                UserId = Dto.UserId,
            };

            await _unitOfWork.MovieReviews.AddAsync(Rewiew);
            _unitOfWork.Complete();

            return Ok(Rewiew);
        } 
        [HttpGet("GetMovieReviews/{MovieId}")]
        public async Task<IActionResult> GetMovieReviews(int MovieId)
        {
            var MovieReviews = await _context.MovieReviews
                .Where(a => a.MovieId == MovieId)
                .Include(B=>B.Movie)
                .Include(B=>B.User)
                .Select(a => new MovieReviewDto
                {
                    MovieId=a.MovieId,
                    Content=a.Content,
                    Rating = a.Rating,
                    UserId = a.UserId, 
                    Movie=a.Movie,
                    User=a.User
                }).ToListAsync();

            return Ok(MovieReviews);
        }

        [HttpPost("CreatePayment")]
        public async Task<IActionResult> CreatePayment([FromServices] IServiceProvider sp, string UserId,int MovieId)
        {
            var User = await _userManager.FindByIdAsync(UserId);
            if (User == null) return BadRequest();

            var Movie = await _unitOfWork.Movies.GetByIdAsync(MovieId);
            if (Movie == null) return NotFound();

            var CheckPayment = await _unitOfWork.Payments.FindAsync(b=>b.UserId== UserId & b.MovieId== MovieId);
            if (CheckPayment != null) return BadRequest("You allready paid");

            else
            {

                // Build the URL to which the customer will be redirected after paying.
                var server = sp.GetRequiredService<IServer>();

                var serverAddressesFeature = server.Features.Get<IServerAddressesFeature>();

                string? thisApiUrl = null;

                if (serverAddressesFeature is not null)
                {
                    thisApiUrl = serverAddressesFeature.Addresses.FirstOrDefault();
                }

                if (thisApiUrl is not null)
                {
                    var SessionUrl = await CheckOut(thisApiUrl, UserId , MovieId);

                    return Ok(SessionUrl);
                }
                else
                {
                    return StatusCode(500);
                }
            }
        }

        [NonAction]
        public async Task<string> CheckOut(string thisApiUrl, string UserId, int MovieId)
        {

            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{thisApiUrl}api/NormalUser/success?sessionId={{CHECKOUT_SESSION_ID}}&UserId={UserId}&MovieId={MovieId}", // Customer paid.
                CancelUrl = $"{thisApiUrl}api/NormalUser/failed?sessionId={{CHECKOUT_SESSION_ID}}", // Checkout cancelled.
                PaymentMethodTypes = new List<string> {"card"},
                LineItems =
                        [ new SessionLineItemOptions
                            {
                                Price = "price_1PMpjiHo3w5F4xJqCPwyE1l0",
                                Quantity = 1,
                              },
                         ],

                Mode = "payment",
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session.Url;
        }

        [HttpGet("success")]
        public async Task<ActionResult> CheckoutSuccessAsync(string sessionId, string UserId, int MovieId)
        {
            var CheckPayment = await _unitOfWork.Payments.FindAsync(b => b.UserId == UserId & b.MovieId == MovieId);
            if (CheckPayment != null) return BadRequest("You allready paid");

            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);

            var payment = new Payment
            {
                Date = DateTime.Now,
                MovieId = MovieId,
                UserId = UserId,
                Amount = 500,
            };

            await _unitOfWork.Payments.AddAsync(payment);
            _unitOfWork.Complete();

            return Ok("Payment Done");
        }

        [HttpGet("failed")]
        public async Task<ActionResult> failed(string sessionId)
        {
            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);

            return Ok("Payment Canceled");
        }
    }
}