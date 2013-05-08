using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using LikeMyDessert.Web.ViewModels.Picture;
using LikeMyDessert.Web.ViewModels.HomePage;

namespace LikeMyDessert.Web.ViewModels.HomePage
{
	public class HomePageViewModel
	{
		public HomePageViewModel()
		{
            TopSlidePictures = new List<PictureViewModel>();
            Desserts = new List<DessertBoxViewModel>();
		}

	    public const int TOP_SLIDE_PIC_COUNT = 5;

		public IList<PictureViewModel> TopSlidePictures { get; set; }

        public IList<DessertBoxViewModel> Desserts { get; set; }
	}
}
