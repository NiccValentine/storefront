namespace StoreFront.EF.Repository.Data
{
    using Microsoft.EntityFrameworkCore;
    using StoreFront.Common.Models;
    using StoreFront.Common;

    public class StoreFrontContext : DbContext
    {
        public DbSet<Product> Product { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<StoreProduct> StoreProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasKey(product => new { product.ProductId });

            modelBuilder.Entity<Store>()
                .HasKey(store => new { store.StoreId });

            modelBuilder.Entity<StoreProduct>()
                .HasKey(storeProduct => new { storeProduct.StoreId, storeProduct.ProductId });

            modelBuilder.Entity<Product>()
                .HasMany(product => product.StoreProduct)
                .WithOne(storeProduct => storeProduct.Product);

            modelBuilder.Entity<Store>()
                .HasMany(store => store.StoreProduct)
                .WithOne(storeProduct => storeProduct.Store);

            modelBuilder.Entity<StoreProduct>()
                .HasOne(storeProduct => storeProduct.Product)
                .WithMany(product => product.StoreProduct)
                .HasForeignKey(storeProduct => storeProduct.ProductId);

            modelBuilder.Entity<StoreProduct>()
                .HasOne(storeProduct => storeProduct.Store)
                .WithMany(store => store.StoreProduct)
                .HasForeignKey(storeProduct => storeProduct.StoreId);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Settings.ConnectionStringEF);
        }
    }
}
