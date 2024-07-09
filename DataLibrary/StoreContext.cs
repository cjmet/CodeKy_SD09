using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DataLibrary
{
    public class StoreContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public string DbPath { get; private set; }
        public bool VerboseSQL { get; set; } = false;

        public void ResetDatabase()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        private void ConsoleLog(string logMessage)
        {
            if (VerboseSQL)
            {
                Console.WriteLine(logMessage);
                Console.WriteLine("");
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IEnumerable<string> cats = ["DbLoggerCategory.Database.Command.Name"];

            optionsBuilder
                .UseSqlite($"Data Source={DbPath}")
                .EnableSensitiveDataLogging()
                .LogTo(ConsoleLog,
                    new[] { DbLoggerCategory.Database.Command.Name },
                    LogLevel.Information,
                    DbContextLoggerOptions.None
                    );
        }

        public StoreContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "product.db");
            Console.WriteLine($"Store   ContextId: {this.ContextId}");
        }

        //modelBuilder.Entity<ProductEntity>()    // cjm - This did not actually help - SD10 240208.00
        //    .HasMany(a => a.Orders)             // I wanted many to many, but also allow duplicate products in an order
        //    .WithMany(a => a.Products)
        //    .UsingEntity<OrderProduct>(a => a.Property(e => e.Id).ValueGeneratedOnAdd());


    }

}
