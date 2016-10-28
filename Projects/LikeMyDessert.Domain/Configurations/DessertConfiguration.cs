using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using LikeMyDessert.Domain;

namespace LikeMyDessert.Domain.Configurations
{
    public class DessertConfiguration : EntityTypeConfiguration<Dessert>
    {
        public DessertConfiguration()
        {
            Map(map => map.MapInheritedProperties());
            Property(dessert => dessert.Description).IsRequired();
            HasRequired(dessert => dessert.Picture);
        }
    }
}
