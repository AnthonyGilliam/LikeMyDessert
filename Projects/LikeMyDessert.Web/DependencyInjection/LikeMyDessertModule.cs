using System;

using Ninject.Modules;

using Global.Utilities.Interfaces;
using Global.Utilities;
using HyperQueryNH.Core;
using LikeMyDessert.Repositories;
using LikeMyDessert.Repositories.Interfaces;
using LikeMyDessert.Services.Interfaces;
using LikeMyDessert.Services;
using LikeMyDessert.Web.ViewModels.HomePage;
using LikeMyDessert.Web.ViewModelManagers.Interfaces;
using LikeMyDessert.Web.ViewModelManagers;

namespace LikeMyDessert.Web.DependencyInjection
{
    public class LikeMyDessertModule : NinjectModule
    {
        public override void Load()
        {
            #region Utilities

			Bind<IUnitOfWork<Guid>>().ToMethod(i => new UnitOfWork<Guid>(PersistenceManager.CurrentObjectCache));
            Bind<IHttpHelper>().To<HttpHelper>();

            #endregion Utilities

            #region Repositories

            #region Live Data
        	Bind<IPictureRepository>().To<PictureRepository>();
            Bind<IDessertRepository>().To<DessertRepository>();
            #endregion Live Data

            #region Mock Data
            #endregion Mock Data

            #endregion Repositories

            #region Services

            #region Live Data
        	Bind<IPictureService>().To<PictureService>();
        	Bind<IDessertService>().To<DessertService>();
        	#endregion Live Data

        	#region Mock Data

        	#endregion Mock Data

        	#endregion Services

            #region ViewModelManagers

            #region Live Data
			Bind<IHomePageViewModelManager>().To<HomePageViewModelManager>();
            Bind<IDessertViewModelManager>().To<DessertViewModelManager>();
            #endregion Live Data

            #region Mock Data

            #endregion Mock Data

            #endregion ViewModelManagers
        }
    }
}
