using CatalogService.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace CatalogService
{
    public class CategoryContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "CategoryDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Items)
                .WithOne(e => e.Category)
                .HasForeignKey("CategoryId")
                .IsRequired();       
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
    }
}
