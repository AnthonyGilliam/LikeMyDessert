using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LikeMyDessert.Web.ViewModels.HomePage;

namespace LikeMyDessert.Web.ViewModelManagers.Interfaces
{
    public interface IHomePageViewModelManager
    {
        HomePageViewModel Get();
        IList<DessertBoxViewModel> GetDesserts();
    }
}
