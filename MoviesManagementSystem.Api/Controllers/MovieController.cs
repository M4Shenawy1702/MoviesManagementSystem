using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesManagementSystem.Core.Dtos;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.EF.Context;

namespace MoviesManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private List<string> _PhotoAllowedExtensions = new List<string> { ".jpg", ".png" };
        private List<string> _VideoAllowedExtensions = new List<string> { ".mp4" };
        private long _PhotoMaxAllowedSize = 10485760;


        public MovieController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;

        }
        [HttpGet("GetAllMovies")]
        public async Task<IActionResult> GetAllMovies()
        {
            var Movies = await _unitOfWork.Movies.FindAllAsync(new[] { "MovieReviews" });
            //var Data = _mapper.Map<IEnumerable<CategoryDatailsDto>>(Categories);
            return Ok(Movies);
        }

        [HttpPost("AddMovie")]
        public async Task<IActionResult> AddMovie([FromForm] AddMovieDto Dto)
        {
            if (Dto == null) return BadRequest();

            if (Dto.Title == null) return BadRequest();

            //var Movie = _mapper.Map<Movie>(Dto);
            var movie = new Movie
            {
                Title = Dto.Title,
                Description = Dto.Description,
                IsFree = Dto.IsFree,
                CategoryId = Dto.CategoryId
            };
            await _unitOfWork.Movies.AddAsync(movie);
            _unitOfWork.Complete();
            if (Dto.Poster is not null)
            {
                var extension = Path.GetExtension(Dto.Poster.FileName);
                if (!_PhotoAllowedExtensions.Contains(extension.ToLower()))
                    return BadRequest("only .jpg and .png img are allowed");
                if (Dto.Poster.Length > _PhotoMaxAllowedSize)
                    return BadRequest("Max Allowed Size is 10Mb");

                using var dataStream = new MemoryStream();
                await Dto.Poster.CopyToAsync(dataStream);

                movie.Poster = dataStream.ToArray();
            } 
            if (Dto.Video is not null)
            {
                var extension = Path.GetExtension(Dto.Video.FileName);
                var MovieName = $"{Dto.Title}-{movie.id}.mp4";

                if (!_VideoAllowedExtensions.Contains(extension.ToLower()))
                     return BadRequest("only .mp4 and .png img are allowed");

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/Movie/Viedo", MovieName);
                using var stream = System.IO.File.Create(path);
                Dto.Video.CopyTo(stream);

                movie.Video = MovieName;
            }

            _unitOfWork.Movies.Update(movie);
            _unitOfWork.Complete();

            return Ok(movie);
        }
        [HttpPut("EditCategory/{CategoryID}")]
        public async Task<IActionResult> EditCategory([FromForm] GetMovieDto Dto, int MovieID)
        {
            if (Dto == null) return BadRequest();

            if (Dto.Title == null) return BadRequest();

            //var category = _mapper.Map<Category>(Dto);
            var Movie = await _unitOfWork.Movies.GetByIdAsync(MovieID);
            if (Movie == null) return NotFound();

            Movie.Title = Dto.Title;
            Movie.Description = Dto.Description;
            Movie.IsFree = Dto.IsFree;
            Movie.Likes = Dto.Likes;

            _unitOfWork.Movies.Update(Movie);
            _unitOfWork.Complete();

            return Ok(Movie);
        }
        [HttpGet("GetMovie/{MovieId}")]
        public async Task<IActionResult> GetMovie(int MovieId)
        {
            var Movie = await _unitOfWork.Movies.FindAsync(B => B.id == MovieId, new[] { "MovieReviews" });
            var result = new GetMovieDto{
                AverageRating = Movie.AverageRating,
                CategoryId = Movie.CategoryId,
                Description = Movie.Description,
                IsFree = Movie.IsFree,
                Likes = Movie.Likes,
                id = MovieId,
                Poster=Movie.Poster,
                Title=Movie.Title,  
            }; 

            return Ok(result);
        }
        [HttpGet("GetMovieVideo/{UserId}&{MovieId}")]
        public async Task<IActionResult> GetMovieVideo(int MovieId, string UserId)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(MovieId);
            if (movie is null) return NotFound();
            
            var moviePath = $"{_webHostEnvironment.WebRootPath}/Movie/Viedo/{movie.Title}-{movie.id}.mp4";

            if (movie.IsFree != true)
            {
                var payment = await _unitOfWork.Payments.FindAsync(b => b.MovieId == MovieId && b.UserId == UserId);
                if (payment == null) return BadRequest("You Shoud Pay first");
                else return Ok(moviePath);
            }
            else return Ok(moviePath);
        }
        [HttpPut("DeleteMovie/{MovieID}")]
        public async Task<IActionResult> DeleteCategory(int MovieID)
        {
            var Movie = await _unitOfWork.Movies.GetByIdAsync(MovieID);
            if (Movie == null) return NotFound();

            _unitOfWork.Movies.Delete(Movie);
            _unitOfWork.Complete();

            return Ok(Movie);
        }
    }
}
