using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LikeMyDessert.Web.ViewModels.HomePage;
using LikeMyDessert.Web.ViewModelManagers.Interfaces;

namespace LikeMyDessert.Web.Controllers
{
	public class HomeController : Controller
	{
        private readonly IHomePageViewModelManager _viewModelManager;

        public HomeController(IHomePageViewModelManager viewModelManager)
        {
            _viewModelManager = viewModelManager;
        }

		public ActionResult Index()
		{
            HomePageViewModel viewModel = _viewModelManager.Get();

			return View(viewModel);
		}

		public ActionResult About()
		{
			return View();
		}

        public PartialViewResult UpdateDessertPane()
		{
            var desserts = _viewModelManager.GetDesserts();   
            return PartialView("_DessertPane", desserts);
		}
	}
}
