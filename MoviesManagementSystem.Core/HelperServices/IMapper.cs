using AutoMapper;
using MoviesManagementSystem.Core.Dtos;
using MoviesManagementSystem.Core.Models;

namespace MoviesManagementSystem.Core.HelperServices
{
    public class IMapper : Profile
    {
        public IMapper()
        {
            //To return CategoryDto
            CreateMap<Category,AddCategoryDto>();  
            
            CreateMap<AddCategoryDto,Category>();

            CreateMap<Movie,GetMovieDto>();
        }
    }
}
