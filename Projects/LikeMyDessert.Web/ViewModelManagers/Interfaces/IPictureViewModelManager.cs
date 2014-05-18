using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LikeMyDessert.Web.ViewModels.Picture;

namespace LikeMyDessert.Web.ViewModelManagers.Interfaces
{
    public interface IPictureViewModelManager
    {
        PictureViewModel GetNextTopSlidePicture(Guid referencePictureID);
    }
}
