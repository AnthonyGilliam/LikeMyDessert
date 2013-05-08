using FluentNHibernate.Mapping;

using LikeMyDessert.Domain;

namespace LikeMyDessert.Repositories.Mappings.ClassMaps
{
	public class DessertMap : ClassMap<Dessert>
	{
        public DessertMap()
		{
			Id(m => m.ID);

			Map(m => m.Name).Length(100).Not.Nullable();
            Map(m => m.Description).Length(500).Not.Nullable();
			Map(m => m.Likes);
            Map(m => m.Dislikes);

            References(m => m.Picture).Not.Nullable();
		}
	}
}
