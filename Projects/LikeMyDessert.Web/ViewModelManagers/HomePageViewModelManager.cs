using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LikeMyDessert.Domain;
using LikeMyDessert.Services.Interfaces;
using LikeMyDessert.Web.ModelMappers;
using LikeMyDessert.Web.ViewModels.Picture;
using LikeMyDessert.Web.ViewModels.HomePage;
using LikeMyDessert.Web.ViewModelManagers.Interfaces;

namespace LikeMyDessert.Web.ViewModelManagers
{
	public class HomePageViewModelManager : IHomePageViewModelManager
	{
		private readonly IPictureService _pictureService;
	    private readonly IDessertService _dessertService;

	    public HomePageViewModelManager(IPictureService pictureService, IDessertService dessertService)
		{
			_pictureService = pictureService;
		    _dessertService = dessertService;
		    HomePageModelMapper.Init();
		}

		public HomePageViewModel Get()
		{
			IList<Picture> pictures = _pictureService.GetFirstPictures(HomePageViewModel.TOP_SLIDE_PIC_COUNT);
			IList<Dessert> desserts = _dessertService.GetRatedDesserts(false);
			IList<PictureViewModel> topSlidePictureViewModels = AutoMapper.Mapper.Map<IList<Picture>, IList<PictureViewModel>>(pictures);
			IList<DessertBoxViewModel> dessertViewModels = AutoMapper.Mapper.Map<IList<Dessert>, IList<DessertBoxViewModel>>(desserts);

		    var viewModel = new HomePageViewModel
		                        {
		                            TopSlidePictures = topSlidePictureViewModels,
                                    Desserts = dessertViewModels
		                        };

		    return viewModel;
		}

        public IList<DessertBoxViewModel> GetDesserts()
        {
            var desserts = _dessertService.GetRatedDesserts(false);

            return AutoMapper.Mapper.Map<IList<Dessert>, IList<DessertBoxViewModel>>(desserts);
        }
    }
}
