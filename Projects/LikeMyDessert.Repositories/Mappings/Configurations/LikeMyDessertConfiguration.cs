using System;
using System.Collections.Generic;
using System.Text;

using FluentNHibernate.Automapping;
using LikeMyDessert.Domain;

namespace LikeMyDessert.Repositories.Mappings.Configurations
{
	public class LikeMyDessertConfiguration : DefaultAutomappingConfiguration
	{
		public override bool ShouldMap(Type type)
		{
			return type.IsSubclassOf(typeof(PersistentObject));
		}

		public override bool IsId(FluentNHibernate.Member member)
		{
			return member.Name == "ID";
		}
	}
}
