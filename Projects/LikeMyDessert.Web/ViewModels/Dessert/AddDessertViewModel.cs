using System.ComponentModel.DataAnnotations;

using LikeMyDessert.Web.ViewModels.Picture;

namespace LikeMyDessert.Web.ViewModels.Dessert
{
    public class AddDessertViewModel
    {
        public AddDessertViewModel()
        {
            Name = string.Empty;
            Description = string.Empty;
            TempPicture = new TempPictureViewModel();
        }
        [Required, StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        [Required, StringLength(100, MinimumLength = 5)]
        public string Description { get; set; }
        public TempPictureViewModel TempPicture { get; set; }
    }
}