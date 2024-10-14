using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesManagementSystem.Core.Dtos;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.EF.Context;
using System.Security.Policy;

namespace MoviesManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private List<string> _AllowedExtensions = new List<string> { ".jpg", ".png" };
        private long _MaxAllowedSize = 10485760;
        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("GetAllCategoties")]
        public async Task<IActionResult> GetAllCategoties()
        {
           var category = await _unitOfWork.Categories.FindAllAsync(new[] { "Movies" });
            return Ok(category);
        }
        [HttpGet("GetCategory{CategotyId}")]
        public async Task<IActionResult> GetCategory(int CategotyId)
        {
            var category = await _unitOfWork.Categories.FindAsync(b=>b.Id== CategotyId, new[] { "Movies"});

            return Ok(category);
        }
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromForm] AddCategoryDto Dto)
        {
            if (Dto == null) return BadRequest();

            if (Dto.Name == null) return BadRequest();

            //var category = _mapper.Map<Category>(Dto);
            var category = new Category
            {
                Name = Dto.Name,
                Description = Dto.Description,
            };

            var Category = await _unitOfWork.Categories.AddAsync(category);
            _unitOfWork.Complete();

            return Ok(Category);
        }
        [HttpPut("EditCategory/{CategoryID}")]
        public async Task<IActionResult> EditCategory([FromForm] AddCategoryDto Dto, int CategoryID)
        {
            if (Dto == null) return BadRequest();

            if (Dto.Name == null) return BadRequest();

            //var category = _mapper.Map<Category>(Dto);
            var category = await _unitOfWork.Categories.GetByIdAsync(CategoryID);
            if (category == null) return NotFound();

            category.Name = Dto.Name;
            category.Description = Dto.Description;

            _unitOfWork.Categories.Update(category);
            _unitOfWork.Complete();

            return Ok(category);
        }
        [HttpPut("DeleteCategory/{CategoryID}")]
        public async Task<IActionResult> DeleteCategory(int CategoryID)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(CategoryID);
            if (category == null) return NotFound();

            _unitOfWork.Categories.Delete(category);
            _unitOfWork.Complete();

            return Ok(category);
        }
    }
}
