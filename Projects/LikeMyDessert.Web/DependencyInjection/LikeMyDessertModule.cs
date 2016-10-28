using System;
using Ninject.Modules;
using Global.Utilities.Interfaces;
using Global.Utilities;
using HyperQueryEF.Core;
using LikeMyDessert.Domain;
using LikeMyDessert.Repositories.Interfaces;
using LikeMyDessert.Repositories;
using LikeMyDessert.Services.Interfaces;
using LikeMyDessert.Services;
using LikeMyDessert.Web.ViewModelManagers.Interfaces;
using LikeMyDessert.Web.ViewModelManagers;

namespace LikeMyDessert.Web.DependencyInjection
{
    public class LikeMyDessertModule : NinjectModule
    {
        public override void Load()
        {
            #region Utilities

			Bind<LikeMyDessertContext>().ToSelf();
			Bind<IUnitOfWork>().To<PersistenceManager<LikeMyDessertContext>>();
            Bind<IHttpHelper>().To<HttpHelper>();

            #endregion Utilities

            #region Repositories

        	Bind<IPictureRepository>().To<PictureRepository>();
            Bind<IDessertRepository>().To<DessertRepository>();

            #endregion Repositories

            #region Services

        	Bind<IPictureService>().To<PictureService>();
        	Bind<IDessertService>().To<DessertService>();

        	#endregion Services

            #region ViewModelManagers

			Bind<IHomePageViewModelManager>().To<HomePageViewModelManager>();
            Bind<IPictureViewModelManager>().To<PictureViewModelManager>();
            Bind<IDessertViewModelManager>().To<DessertViewModelManager>();

            #endregion ViewModelManagers
        }
    }
}
