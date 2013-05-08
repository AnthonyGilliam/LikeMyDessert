using System;
using System.ComponentModel.DataAnnotations;

using LikeMyDessert.Web.ViewModels.Picture;

namespace LikeMyDessert.Web.ViewModels.HomePage
{
    public class DessertBoxViewModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public PictureViewModel Picture { get; set; }
    }
}
