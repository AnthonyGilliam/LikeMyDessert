using System;
using System.Web;
using System.Web.Mvc;

using LikeMyDessert.Web.ViewModels.Picture;
using LikeMyDessert.Web.ViewModelManagers.Interfaces;
using LikeMyDessert.Web.ViewModels.HomePage;
using Global.Utilities.Interfaces;
using LikeMyDessert.Web.ViewModels.Dessert;

namespace LikeMyDessert.Web.Controllers
{
    public class DessertController : Controller
    {
        private IDessertViewModelManager _dessertViewModelManager;

        public DessertController(IDessertViewModelManager dessertViewModelManager)
        {
            _dessertViewModelManager = dessertViewModelManager;
        }

        [HttpPost]
        public void Like(Guid id)
        {
            _dessertViewModelManager.Like(id);
        }

        [HttpPost]
        public void Dislike(Guid id)
        {
            _dessertViewModelManager.Dislike(id);
        }

        [HttpPost]
        public PartialViewResult AddPictureChange()
        {
            HttpPostedFileBase picture = Request.Files[0];
                        
            var tempPic = _dessertViewModelManager.UploadTempPicture(picture);

            //HACK
            System.Threading.Thread.Sleep(3000);
            //TODO: Produce delay to observe cat-butt

            return PartialView("_TempPicture", tempPic);
        }

        [HttpPost]
        public PartialViewResult Add(DessertBoxViewModel dessert)
        {
            //TODO: Implement add dessert
            _dessertViewModelManager.Add(dessert);

            return PartialView("_AddSuccess");
        }
    }
}
