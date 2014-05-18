using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LikeMyDessert.Web.ViewModelManagers.Interfaces;

namespace LikeMyDessert.Web.Controllers
{
    public class PictureController : Controller
    {
        private IPictureViewModelManager _pictureViewModelManager;

        public PictureController(IPictureViewModelManager pictureViewModelManager)
        {
            _pictureViewModelManager = pictureViewModelManager;
        }

        public ActionResult GetNextTopSlidePicture(Guid referencePictureID)
        {
            var viewModel = _pictureViewModelManager.GetNextTopSlidePicture(referencePictureID);

            return View(viewModel);
        }
    }
}