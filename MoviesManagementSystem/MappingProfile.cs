using AutoMapper;
using MoviesManagementSystem.Core.Dots.Movie;
using MoviesManagementSystem.Core.Dots.RateDtos;
using MoviesManagementSystem.Core.Dots.ReviewDtos;
using MoviesManagementSystem.Core.Dots.UserDtos;
using MoviesManagementSystem.Core.Dots.WatchListDtos;
using MoviesManagementSystem.Core.Dtos;
using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.EF.Models;

namespace MoviesManagementSystem.Core.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Genre
            CreateMap<Genre, GenreDatailsDto>()
                .ForMember(dest => dest.MoviesNames, opt => opt.MapFrom(src => src.Movies.Select(m => m.Title).ToList()));
            CreateMap<AddGenreDto, Genre>()
                .ForMember(dest => dest.Movies, opt => opt.Ignore())
                .ReverseMap();

            // Rate
            CreateMap<Rate, RateDto>().ReverseMap();
            CreateMap<RateDetailsDto, Rate>().ReverseMap();

            // Movie
            CreateMap<Movie, MovieDetailsDto>()
                .ForMember(dest => dest.GenresName, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name).ToList()))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Rates != null && src.Rates.Count > 0 ? src.Rates.Average(r => r.Score) : 0))
                .ForMember(dest => dest.Poster, opt => opt.MapFrom(src => src.PosterUrl))
                .ForMember(dest => dest.Video, opt => opt.MapFrom(src => src.VideoUrl));

            CreateMap<CreateMovieDto, Movie>();

            // Review
            CreateMap<ReviewDto, Review>();
            CreateMap<Review, ReviewDetailsDto>();

            // WatchList
            CreateMap<WatchList, WatchListDetailsDto>();

            // User
            CreateMap<ApplicationUser, UserInfoDto>()
                .ForMember(dest => dest.RolesName, opt => opt.MapFrom(src => new List<string>()));
        }
    }
}
