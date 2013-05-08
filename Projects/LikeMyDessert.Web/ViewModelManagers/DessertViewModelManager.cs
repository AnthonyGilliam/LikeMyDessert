using System;
using System.Linq;
using System.Configuration;
using System.IO;
using System.Web;

using Global.Utilities.Interfaces;
using LikeMyDessert.Domain;
using LikeMyDessert.Services.Interfaces;
using LikeMyDessert.Web.ViewModels.Picture;
using LikeMyDessert.Web.ViewModels.Dessert;
using LikeMyDessert.Web.ViewModels.HomePage;
using LikeMyDessert.Web.ModelMappers;
using LikeMyDessert.Web.ViewModelManagers.Interfaces;

namespace LikeMyDessert.Web.ViewModelManagers
{
    public class DessertViewModelManager : IDessertViewModelManager
    {
        private readonly IHttpHelper _httpHelper;
        private readonly IPictureService _pictureService;
        private readonly IDessertService _dessertService;
        private const string TEMP_PIC_SESSION_KEY = "TempPicture";

        public DessertViewModelManager(IHttpHelper httpHelper, IPictureService pictureService, IDessertService dessertService)
        {
            _httpHelper = httpHelper;
            _pictureService = pictureService;
            _dessertService = dessertService;
            HomePageModelMapper.Init();
        }

        public void Like(Guid _dessertID)
        {
            _dessertService.LikeDessert(_dessertID);
        }

        public void Dislike(Guid _dessertID)
        {
            _dessertService.DislikeDessert(_dessertID);
        }

        public TempPictureViewModel UploadTempPicture(HttpPostedFileBase pictureFile)
        {
            //Check to see if the user already uploaded a picture:
            if (_httpHelper.ExistsInSession(TEMP_PIC_SESSION_KEY))
            {
                var oldPicViewModel = _httpHelper.GetFromAppSession<TempPictureViewModel>(TEMP_PIC_SESSION_KEY);
                if (oldPicViewModel != null && !string.IsNullOrWhiteSpace(oldPicViewModel.SavedFIlePath))
                {
                    File.Delete(oldPicViewModel.SavedFIlePath);
                }
            }

            var picName = pictureFile.FileName;
            var tempRelativePath = ConfigurationManager.AppSettings["TempDirectory"];
            var tempDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempRelativePath);
            var tempUri = "/" + Path.Combine(tempRelativePath, picName).Replace("\\", "/");
            var savedFilePath = Path.Combine(tempDirectory, picName);
            pictureFile.SaveAs(savedFilePath);

            var tempPic = new TempPictureViewModel
                              {
                                  Name = picName,
                                  Uri = tempUri,
                                  SavedFIlePath = savedFilePath,
                                  ImageType = "jpg"
                              };

            _httpHelper.AddToAppSession(TEMP_PIC_SESSION_KEY ,tempPic);

            return tempPic;
        }

        public void Add(DessertBoxViewModel dessertViewModel)
        {
            var pic = SaveTempPictureAsPhoto();

            var dessert = AutoMapper.Mapper.Map<DessertBoxViewModel, Dessert>(dessertViewModel);
            dessert.Picture = pic;
            _dessertService.Save(dessert);
        }

        private Picture SaveTempPictureAsPhoto()
        {
            var tempPic = _httpHelper.GetFromAppSession<TempPictureViewModel>(TEMP_PIC_SESSION_KEY);
            var pic = AutoMapper.Mapper.Map<TempPictureViewModel, Picture>(tempPic);

            _pictureService.Save(pic);
            var photoRelativePath = (ConfigurationManager.AppSettings["PhotoDirectory"] + pic.ID + "." + pic.ImageType).Replace("/", "\\");
            var photoPath = AppDomain.CurrentDomain.BaseDirectory + photoRelativePath;

            File.Copy(tempPic.SavedFIlePath, photoPath, true);
            File.Delete(tempPic.SavedFIlePath);

            return pic;
        }
    }
}