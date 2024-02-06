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
        public string ProductInterfaceFilename => "ProductRepository";
        public string ProductInterfaceFunctionName() => "ProductRepository";
        public string ProductDbPath => _dbContext.DbPath;

        private readonly ProductContext _dbContext;
                public ProductRepository()
        {
            _dbContext = new ProductContext();
        }

        public bool VerboseSQL { get => _dbContext.VerboseSQL; set => _dbContext.VerboseSQL = value; }
        public bool DataExists() => _dbContext.Products.Count() > 0 || _dbContext.Orders.Count() > 0;

        
        
        public void AddProduct(ProductEntity product)
        {
            //foreach (var order in product.Orders)
            //    _dbContext.Orders.Attach(order);
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
        }

        public IEnumerable<ProductEntity> GetAllProducts() => _dbContext.Products.Include(p => p.Orders).AsNoTracking().ToList();
        //public IEnumerable<ProductEntity> GetAllProducts() => _dbContext.Products.ToList();



        public void DeleteProduct(int id)
        {
            _dbContext.Products.Remove(GetProductById(id));
            _dbContext.SaveChanges();
        }

        public IEnumerable<ProductEntity> GetAllProductsByCategory(string category)
        {
            category = category.ToLower();
            return GetAllProducts().Where(p => p.Category.ToLower().Contains(category)).ToList();
        }

        public IEnumerable<ProductEntity> GetAllProductsByName(string name)
        {
            name = name.ToLower();
            return GetAllProducts().Where(p => p.Name.ToLower().Contains(name)).ToList();
        }

        public IEnumerable<ProductEntity> GetOnlyInStockProducts() => GetAllProducts().Where(p => p.Quantity > 0).ToList();

        public ProductEntity GetProductById(int id) => GetAllProducts().Where(p => p.Id == id).FirstOrDefault();

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
