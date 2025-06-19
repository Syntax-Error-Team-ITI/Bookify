using AutoMapper;
using Bookify.Web.Core.ViewModels;
using Bookify.Web.Core.ViewModels.Author;
using Bookify.Web.Core.ViewModels.BookCopies;
using Bookify.Web.Core.ViewModels.Category;
using Bookify.Web.Core.ViewModels.Subscription;
using Bookify.Web.Core.ViewModels.User;
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

            CreateMap<AuthorFormViewModel, Author>()
               .ReverseMap();

            // Author
            CreateMap<Author, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

            // Book
            CreateMap<BookFormVM, Book>()
                .ReverseMap()
                .ForMember(dest => dest.Categories, opt => opt.Ignore());
            CreateMap<Book, BookViewModel>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author!.Name))
                .ForMember(dest => dest.Categories, opt => opt.Ignore());
            CreateMap<Book, BookListVM>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author!.Name));
            CreateMap<BookCopy, BookCopyViewModel>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book!.Title))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Book!.Id))
                .ForMember(dest => dest.BookThumbnailUrl, opt => opt.MapFrom(src => src.Book!.ImageThumbnailUrl));

            // Subscriper 
            CreateMap<Subscriber, SelectListItem>()
               .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.FirstName +" "+ src.LastName));
            //Governorate
            CreateMap<Governorate, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));
            //Area
            CreateMap<Area, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));
                
             // Subscriber
            CreateMap<SubscriberFormVM, Subscriber>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.ImageThumbnailUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Area, opt => opt.Ignore());

            CreateMap<Subscriber, SubscriberFormVM>();

            // Subscription
            CreateMap<Subscription, SubscriptionVM>();

            CreateMap<Subscriber, SubscriberVM>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.Governorate, opt => opt.MapFrom(src => src.Area.Governorate.Name))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area.Name));
            

            // User
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
            CreateMap<ApplicationUser, UserAddViewModel>().ReverseMap();
            CreateMap<UserAddViewModel, ApplicationUser>().ReverseMap();


            // Rentals
            CreateMap<Rental, RentalViewModel>();
            CreateMap<RentalCopy, RentalCopyViewModel>();



        }
    }
}
