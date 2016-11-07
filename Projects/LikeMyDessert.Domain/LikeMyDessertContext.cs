using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LikeMyDessert.Domain
{
    public class LikeMyDessertContext : DbContext
    {
        public LikeMyDessertContext() : base("LikeMyDessert")
        {
        }

        public void Init()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<LikeMyDessertContext>());
            //Force database to initialize (Create/Migrate tables).
            Database.Initialize(true);
            //Log SQL statements in DEBUG mode
            Database.Log = s => Debug.WriteLine(s);
        }

        public virtual DbSet<Picture> Pictures { get; set; }
        public virtual DbSet<Dessert> Desserts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(this.GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
