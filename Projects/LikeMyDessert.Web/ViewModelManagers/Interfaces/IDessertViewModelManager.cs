using System;
using System.Web;

using LikeMyDessert.Web.ViewModels.Dessert;
using LikeMyDessert.Web.ViewModels.HomePage;

namespace LikeMyDessert.Web.ViewModelManagers.Interfaces
{
    public interface IDessertViewModelManager
    {
        void Like(Guid _dessertID);
        void Dislike(Guid _dessertID);
        void Add(DessertBoxViewModel dessert);
        /// <summary>
        /// Store temporary add form picture in temp folder.
        /// </summary>
        /// <returns>Path to temp picture</returns>
        TempPictureViewModel UploadTempPicture(HttpPostedFileBase pictureFile);
    }
}
