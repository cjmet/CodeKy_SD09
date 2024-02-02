using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DataLibrary
{
    public class ProductContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public string DbPath { get; private set; }
        public bool VerboseSQL { get; set; } = false;

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

        public ProductContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "product.db");
        }

    }





}
