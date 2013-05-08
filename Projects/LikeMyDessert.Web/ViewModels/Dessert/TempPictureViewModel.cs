using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LikeMyDessert.Web.ViewModels.Picture;

namespace LikeMyDessert.Web.ViewModels.Dessert
{
    public class TempPictureViewModel
    {
        public TempPictureViewModel()
        {
            Name = string.Empty;
            SavedFIlePath = string.Empty;
            ImageType = string.Empty;
        }

        public string Name { get; set; }
        public string Uri { get; set; }
        public string SavedFIlePath { get; set; }
        public string ImageType { get; set; }
    }
}
