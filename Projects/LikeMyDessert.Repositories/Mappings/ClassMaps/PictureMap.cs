using FluentNHibernate.Mapping;

using LikeMyDessert.Domain;

namespace LikeMyDessert.Repositories.Mappings.ClassMaps
{
	public class PictureMap : ClassMap<Picture>
	{
		public PictureMap()
		{
		    Id(m => m.ID);

			Map(m => m.Alt).Length(100);
			Map(m => m.ImageType).Not.Nullable();
            Map(m => m.OrdinalIndex);
		}
	}
}
