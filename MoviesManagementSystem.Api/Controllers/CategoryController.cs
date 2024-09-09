using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.EF.Repositories;

namespace MoviesManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("GetAllCategoties")]
        public IActionResult GetAllMovie()
        {
            return Ok(_unitOfWork.Categories.GetAllAsync());
        } 
        [HttpGet("GetCategoty{CategotyId}")]
        public async Task<IActionResult> GetMovie(int MoviesId)
        {
            return Ok( await _unitOfWork.Categories.GetByIdAsync(MoviesId));
        }
    }
}
