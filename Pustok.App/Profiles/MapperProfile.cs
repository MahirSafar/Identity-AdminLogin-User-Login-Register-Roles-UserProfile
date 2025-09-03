using AutoMapper;
using Pustok.App.Models;
using Pustok.App.ViewModels;

namespace Pustok.App.Profiles
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Book, BookTestVM>()
                .ForMember(dest => dest.BookImageUrls, opt => opt.MapFrom(src => src.BookImages.Select(bi=>bi.ImageUrl)))
                .ForMember(dest => dest.BookTagNames, opt => opt.MapFrom(src => src.BookTags.Select(bt=>bt.Tag.Name)));
        }
    }
}
