using AutoMapper;
using Bookify.Web.Core.ViewModels;
using Bookify.Web.Core.ViewModels.Category;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Bookify.Web.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category
            CreateMap<Category, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

            CreateMap<CategoryFormViewModel, Category>()
                .ReverseMap();

            // Author
            CreateMap<Author, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));
            // Book
            CreateMap<BookFormVM, Book>()
                .ReverseMap()
                .ForMember(dest => dest.Categories, opt => opt.Ignore());
            CreateMap<Book, BookViewModel>();
        }
    }
}
