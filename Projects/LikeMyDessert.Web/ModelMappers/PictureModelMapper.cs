using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

using LikeMyDessert.Domain;
using LikeMyDessert.Web.ViewModels.Picture;

namespace LikeMyDessert.Web.ModelMappers
{
    public static class PictureModelMapper
    {
        static PictureModelMapper()
        {
            CreateMappings();
        }

        public static void Init()
        {
        
        }

        private static void CreateMappings()
        {
            if (!ModelMapperHelper.DoesMapExist<Picture, PictureViewModel>())
            {
                AutoMapper.Mapper.CreateMap<Picture, PictureViewModel>()
                    .ForMember(dest => dest.Url, opt => opt.MapFrom(GetPhotoUrl));
            }
        }

		private static string GetPhotoUrl(Picture pic)
		{
			return ConfigurationManager.AppSettings["PhotoDirectory"] + pic.ID + "." + pic.ImageType;
		}

    }
}