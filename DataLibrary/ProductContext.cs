using Microsoft.EntityFrameworkCore;

namespace DataLibrary
{
    public class ProductContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public string DbPath { get; private set; }

        public ProductContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "product.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
    }





}
