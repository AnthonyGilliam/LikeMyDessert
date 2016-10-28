using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using LikeMyDessert.Domain;

namespace LikeMyDessert.Domain.Configurations
{
    public class PictureConfiguration : EntityTypeConfiguration<Picture>
    {
        public PictureConfiguration()
        {
            Map(map => map.MapInheritedProperties());
            Property(pic => pic.Alt).IsRequired();
            Property(pic => pic.ImageType).IsRequired();
            Property(pic => pic.OrdinalIndex).IsRequired();
        }
    }
}
