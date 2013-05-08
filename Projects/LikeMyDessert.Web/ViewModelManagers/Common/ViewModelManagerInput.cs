using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DessertDemo_Web.Web.ViewModelManagers
{
	public class VMMInput<TViewModel>
		where TViewModel : class
	{
		public virtual TViewModel ViewModel { get; set; }

		public virtual IDictionary<string, object> AdditionalArguments { get; set; }

		public virtual Guid PersistenceId { get; set; }

		public virtual string Query { get; set; }
	}
}
