using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace БазаДанныхМДК1102.NewFolder1
{
    public partial class Model_DB : DbContext
    {
        public Model_DB()
            : base("name=Model_DB")
        {
        }

        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<MaterialType> MaterialType { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }
        public virtual DbSet<ProductMaterial> ProductMaterial { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>()
                .HasMany(e => e.ProductMaterial)
                .WithRequired(e => e.Material)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductMaterial)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);
        }
    }
}
