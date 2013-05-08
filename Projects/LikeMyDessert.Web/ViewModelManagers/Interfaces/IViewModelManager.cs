using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DessertDemo_Web.Web.ViewModelManagers;

namespace DessertDemo_Web.Web.ViewModelManagers.Interfaces
{
	public interface IViewModelManager<TViewModel>
		where TViewModel: class
	{
		TViewModel Get();
	}
}
