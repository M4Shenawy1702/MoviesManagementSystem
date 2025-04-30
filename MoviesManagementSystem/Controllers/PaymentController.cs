using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.IServices;

namespace MoviesManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("sessions")]
        public async Task<IActionResult> CreatePaymentSessionAsync([FromBody] int movieId, string userId, [FromServices] IServiceProvider sp)
        {
            var paymentSessionUrl = await _paymentService.CreatePaymentSessionAsync(movieId, userId, sp);
            if (paymentSessionUrl == null)
                return StatusCode(500, "An error occurred while creating payment session.");

            return CreatedAtAction(nameof(CreatePaymentSessionAsync), new { url = paymentSessionUrl }, new { url = paymentSessionUrl });
        }

        [HttpGet("sessions/success")]
        public async Task<IActionResult> CheckoutSuccessAsync(string sessionId, int movieId, string userId)
        {
            var result = await _paymentService.CheckoutSuccessAsync(sessionId, movieId, userId);
            return Ok(result);
        }

        [HttpGet("sessions/failed")]
        public async Task<IActionResult> CheckoutFailedAsync(string sessionId)
        {
            var result = await _paymentService.CheckoutFailedAsync(sessionId);
            return BadRequest("Payment process failed.");
        }
    }
}
