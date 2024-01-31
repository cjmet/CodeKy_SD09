using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeKY_SD01.Products;
using Microsoft.EntityFrameworkCore;

namespace CodeKY_SD01
{
    public class ProductContext : DbContext
	{
		public DbSet<Product> Products { get; set; }
		public string DbPath { get; private set; }

		public ProductContext()
		{
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "product.db");
        }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite($"Data Source={DbPath}");
		}
	}
}
