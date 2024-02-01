using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CodeKY_SD01.Data
{
    public class ProductContext : DbContext
	{
		public DbSet<ProductEntity> Products { get; set; }
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

	public class ProductEntity
	{
        public ProductEntity() : this("void", null, null, 0, 0) { }
        public ProductEntity(String name) : this(name, null, null, 0, 0) { }
        public ProductEntity(String name, String category, String description, decimal price, int quantity)
        {
            Name = name;
            Category = category;
            Description = description;
            Price = price;
            Quantity = quantity;
        }
		
		public int Id { get; set; }
        public string Name { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }



}
