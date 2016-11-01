using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LikeMyDessert.Web.ViewModelManagers.Interfaces;
using LikeMyDessert.Web.ViewModels.Picture;

namespace LikeMyDessert.Web.Controllers
{
    public class PictureController : Controller
    {
        private IPictureViewModelManager _pictureViewModelManager;

        public PictureController(IPictureViewModelManager pictureViewModelManager)
        {
            _pictureViewModelManager = pictureViewModelManager;
        }

        [HttpPost]
        public ActionResult GetNextTopSlidePicture(IEnumerable<Guid> referencePictureIDs)
        {
            var viewModel = _pictureViewModelManager.GetNextTopSlidePicture(referencePictureIDs);

            return View("_Picture", viewModel ?? new PictureViewModel());
        }
    }
}