using System;
using Ninject.Modules;

using Global.Utilities.Interfaces;
using Global.Utilities;
using HyperQueryNH.Core;
using LikeMyDessert.Repositories.Interfaces;
using LikeMyDessert.Repositories;
using LikeMyDessert.Services.Interfaces;
using LikeMyDessert.Services;
using LikeMyDessert.Web.ViewModelManagers.Interfaces;
using LikeMyDessert.Web.ViewModelManagers;
using Moq;
using LikeMyDessert.Web.ViewModels.Dessert;
using System.IO;
using System.Configuration;

namespace LikeMyDessert.Tests.DependencyInjection
{
    public class LikeMyDessertModule : NinjectModule
    {
        public const string LIVE_DATA_METADATA_KEY = "MockData";
        public const string MOCK_DATA_METADATA_KEY = "LiveData";

        public override void Load()
        {
            #region Utilities

            Bind<IUnitOfWork<Guid>>().ToMethod(i => new UnitOfWork<Guid>(PersistenceManager.CurrentObjectCache)).WithMetadata(LIVE_DATA_METADATA_KEY, true);
            Bind<IHttpHelper>().To<HttpHelper>().WithMetadata(LIVE_DATA_METADATA_KEY, true);

            #endregion Utilities

            #region Repositories

            #region Live Data
			Bind<IPictureRepository>().To<PictureRepository>().WithMetadata(LIVE_DATA_METADATA_KEY, true);
			Bind<IDessertRepository>().To<DessertRepository>().WithMetadata(LIVE_DATA_METADATA_KEY, true);
            #endregion Live Data

            #region Mock Data
            var TEMP_PIC_FILE_NAME = "Test Pic.jpg";
            var TEMP_PIC_FILE_PATH = Path.Combine(ConfigurationManager.AppSettings["TempDirectory"], TEMP_PIC_FILE_NAME);
            var TEMP_PIC_SESSION_KEY = "TempPicture";
            var tempPictureUrl = "/";
            File.Copy(@"C:\Projects\LikeMyDessert\Projects\LikeMyDessert.Tests\IntegrationTests\Content\Test Pic.jpg", TEMP_PIC_FILE_PATH, true);
            var tempPictureViewModel = new TempPictureViewModel 
            {
                Name = TEMP_PIC_FILE_NAME,
                ImageType = "jpg",
                SavedFIlePath = TEMP_PIC_FILE_PATH,
                Uri = tempPictureUrl
            };

            var httpHelperMock = new Mock<IHttpHelper>();
            httpHelperMock.Setup(hpr => hpr.GetFromAppSession<TempPictureViewModel>(TEMP_PIC_SESSION_KEY))
                .Returns(tempPictureViewModel);
            Bind<IHttpHelper>().ToConstant(httpHelperMock.Object).WithMetadata(MOCK_DATA_METADATA_KEY, true);
            #endregion Mock Data

            #endregion Repositories

			#region Services

			#region Live Data
            Bind<IPictureService>().To<PictureService>().WithMetadata(LIVE_DATA_METADATA_KEY, true);
            Bind<IDessertService>().To<DessertService>().WithMetadata(LIVE_DATA_METADATA_KEY, true);
			#endregion Live Data

			#region Mock Data

			#endregion Mock Data

			#endregion Services

			#region ViewModelManagers

			#region Live Data
			Bind<IHomePageViewModelManager>().To<HomePageViewModelManager>().WithMetadata(LIVE_DATA_METADATA_KEY, true);
			Bind<IDessertViewModelManager>().To<DessertViewModelManager>().WithMetadata(LIVE_DATA_METADATA_KEY, true);
			#endregion Live Data

			#region Mock Data

			#endregion Mock Data

			#endregion ViewModelManagers
        }
    }
}
