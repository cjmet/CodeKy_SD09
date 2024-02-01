using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CodeKY_SD01.Data;
using CodeKY_SD01.Interfaces;
using System.Globalization;



namespace CodeKY_SD01.Data
{

    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context = new ProductContext();


        public void AddProduct(ProductEntity product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            _context.Products.Remove(GetProductById(id));
            _context.SaveChanges();

            String a = "a";
        }

        public void UpdateProduct(ProductEntity product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        // public void SaveChanges() => _context.SaveChanges();

        public ProductEntity GetProductById(int id) => _context.Products.Find(id);

        public IEnumerable<ProductEntity> GetAllProducts() => _context.Products.ToList();

        // StringComparison.OrdinalIgnoreCase does not work here
        public IEnumerable<ProductEntity> GetProductsByCategory(string category)
        {
            category = category.ToLower();
            return _context.Products.Where(p => p.Category.Contains(category)).ToList();
        }

        public IEnumerable<ProductEntity> GetProductsByName(string name)
        {
            name = name.ToLower();
            return _context.Products.Where(p => p.Name.ToLower().Contains(name)).ToList();
        }

        public IEnumerable<ProductEntity> GetOnlyInStockProducts() => _context.Products.Where(p => p.Quantity > 0).ToList();

        public void DebugDatabaseInit()
        {
            throw new NotImplementedException();
        }
    }
}

