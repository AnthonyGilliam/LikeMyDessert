using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using LikeMyDessert.Domain;

namespace LikeMyDessert.Domain.Configurations
{
    public class PictureConfiguration : EntityTypeConfiguration<Picture>
    {
        public PictureConfiguration()
        {
            HasKey(pic => pic.ID);
            Property(pic => pic.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(pic => pic.TimeOfCreation).IsRequired();
            Property(pic => pic.Alt).IsRequired();
            Property(pic => pic.ImageType).IsRequired();
            Property(pic => pic.OrdinalIndex).IsRequired();
        }
    }
}
