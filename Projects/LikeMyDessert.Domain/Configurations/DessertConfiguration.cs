using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace LikeMyDessert.Domain.Configurations
{
    public class DessertConfiguration : EntityTypeConfiguration<Dessert>
    {
        public DessertConfiguration()
        {
            HasKey(dessert => dessert.ID);
            Property(dessert => dessert.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(dessert => dessert.TimeOfCreation).IsRequired();
            Property(dessert => dessert.Name).IsRequired();
            Property(dessert => dessert.Description).IsRequired();
            HasRequired(dessert => dessert.Picture);
        }
    }
}
