using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;



namespace DataLibrary
{
    public class ProductRepository : IProductRepository
    {
        
        private readonly ProductContext _dbContext;
                public ProductRepository()
        {
            _dbContext = new ProductContext();
        }



        public string DbPath => _dbContext.DbPath;

        public bool VerboseSQL { get => _dbContext.VerboseSQL; set => _dbContext.VerboseSQL = value; }
        public bool DataExists() => _dbContext.Products.Count() > 0 || _dbContext.Orders.Count() > 0;

        public void AddProduct(ProductEntity product)
        {
            foreach (var order in product.Orders)
                _dbContext.Orders.Attach(order);
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            _dbContext.Products.Remove(GetProductById(id));
            _dbContext.SaveChanges();
        }
   
        
        public IEnumerable<ProductEntity> GetAllProducts() => _dbContext.Products.ToList();


        public IEnumerable<ProductEntity> GetAllProductsByCategory(string category)
        {
            category = category.ToLower();
            return _dbContext.Products.Where(p => p.Category.ToLower().Contains(category)).Include(p => p.Orders).ToList();
        }

        public IEnumerable<ProductEntity> GetAllProductsByName(string name)
        {
            name = name.ToLower();
            return _dbContext.Products.Where(p => p.Name.ToLower().Contains(name)).Include(p => p.Orders).ToList();
        }

        public IEnumerable<ProductEntity> GetOnlyInStockProducts() => _dbContext.Products.Where(p => p.Quantity > 0).ToList();

        public ProductEntity GetProductById(int id) => _dbContext.Products.Where(p => p.Id == id).Include(p => p.Orders).FirstOrDefault();

        public ProductEntity GetProductByName(string name)
        {
            name = name.ToLower();
            return GetAllProducts().Where(p => p.Name.ToLower().Contains(name)).FirstOrDefault();
        }

        public void ResetDatabase()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }

        public void UpdateProduct(ProductEntity product)
        {
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();
        }
    }
}
