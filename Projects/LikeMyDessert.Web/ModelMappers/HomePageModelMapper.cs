using System.Configuration;
using System.IO;
using AutoMapper;

using LikeMyDessert.Domain;
using LikeMyDessert.Web.ViewModels.Picture;
using LikeMyDessert.Web.ViewModels.HomePage;
using LikeMyDessert.Web.ViewModels.Dessert;

namespace LikeMyDessert.Web.ModelMappers
{
	public static class HomePageModelMapper
	{
        static HomePageModelMapper()
        {
            CreateMappings();
        }
        
        public static void Init()
		{
		}

		private static void CreateMappings()
		{
		    Mapper.CreateMap<Picture, PictureViewModel>()
		        .ForMember(dest => dest.Url, opt => opt.MapFrom(GetPhotoUrl));
            Mapper.CreateMap<TempPictureViewModel, Picture>()
                .ForMember(dest => dest.Alt, opt => opt.MapFrom(tempPic => tempPic.Name));
		    Mapper.CreateMap<PictureViewModel, Picture>();
		    Mapper.CreateMap<Dessert, DessertBoxViewModel>();
            Mapper.CreateMap<DessertBoxViewModel, Dessert>();
		}

		private static string GetPhotoUrl(Picture pic)
		{
			return ConfigurationManager.AppSettings["PhotoDirectory"] + pic.ID + "." + pic.ImageType;
		}
	}
}