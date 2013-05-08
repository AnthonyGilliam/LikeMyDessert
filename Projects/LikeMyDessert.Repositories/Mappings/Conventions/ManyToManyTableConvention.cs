using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;

using Global.Utilities.ExtensionMethods;

namespace LikeMyDessert.Repositories.Mappings.Conventions
{
	public class CustomManyToManyTableNameConvention : ManyToManyTableNameConvention
	{
		protected override string GetBiDirectionalTableName(IManyToManyCollectionInspector primaryCollection, IManyToManyCollectionInspector secondaryCollection)
		{
			return string.Format("tbl{0}{1}", primaryCollection.EntityType.Name.Pluralize(), secondaryCollection.EntityType.Name.Pluralize());
		}

		protected override string GetUniDirectionalTableName(IManyToManyCollectionInspector collection)
		{
			return string.Format("tbl{0}", collection.EntityType.Name.Pluralize());
		}
	}
}
