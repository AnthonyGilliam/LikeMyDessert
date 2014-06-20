using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LikeMyDessert.Domain;
using LikeMyDessert.Services.Interfaces;
using LikeMyDessert.Web.ViewModels.Picture;
using LikeMyDessert.Web.ViewModelManagers.Interfaces;
using LikeMyDessert.Web.ModelMappers;

namespace LikeMyDessert.Web.ViewModelManagers
{
    public class PictureViewModelManager : IPictureViewModelManager
    {
        IPictureService _pictureService;

        public PictureViewModelManager(IPictureService pictureService)
        {
            _pictureService = pictureService;
            PictureModelMapper.Init();
        }

        public PictureViewModel GetNextTopSlidePicture(IEnumerable<Guid> referencePictureIDs)
        {
            var randomPicture = _pictureService.GetNextRandomPicture(referencePictureIDs);

            var viewModel = AutoMapper.Mapper.Map<Picture, PictureViewModel>(randomPicture);

            return viewModel;
        }
    }
}