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

        private readonly StoreContext _dbContext;
        
        public ProductRepository(StoreContext DIContext)
        {
            //_dbContext = new ProductContext();
            _dbContext = DIContext;
            Console.WriteLine($"Product ContextId: {_dbContext.ContextId}");
        }

        public bool VerboseSQL { get => _dbContext.VerboseSQL; set => _dbContext.VerboseSQL = value; }
        public void ClearChangeTracker() { _dbContext.ChangeTracker.Clear(); }
        public bool DataExists() => _dbContext.Products.Count() > 0 || _dbContext.Orders.Count() > 0;



        // This version does not play nice with default controller and web pages.  Probably a config issue we have not learned yet.
        public IEnumerable<ProductEntity> GetAllProducts() => _dbContext.Products.Include(p => p.Orders).ToList();
        
        // rewritten version for the web controller that does not include linked orders.
        public IEnumerable<ProductEntity> GetAllProductsWeb() => _dbContext.Products.ToList();
        // Async Version for SD12
        public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync() => await _dbContext.Products.ToListAsync();



        public void AddProduct(ProductEntity product)
        {
            _dbContext.Products.Add(product);
        }

        // AddSync: This method is async only to allow special value generators, such as the one used by 'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo', to access the database asynchronously.For all other cases the non async method should be used.
        public async Task AddProductAsync(ProductEntity product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
        }



        public void UpdateProduct(ProductEntity product)
        {
            _dbContext.Products.Update(product);
        }

        // There is no UpdateAsync method in EF Core.
        // This is a workflow issue.
        // FindAsync, Retrieve, Update, SendBack, SaveChangesAsync      // cjm
        // ExecuteUpdateAsync ???
        public async Task UpdateProductAsync(ProductEntity product)
        {
            await _dbContext.Products.Where(item => item.Id == product.Id) 
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(item => item.Name, product.Name)
                    .SetProperty(item => item.Category, product.Category)
                    .SetProperty(item => item.Description, product.Description)
                    .SetProperty(item => item.Price, product.Price)
                    .SetProperty(item => item.Quantity, product.Quantity)
                );
        }



        public void DeleteProduct(int id)
        {
            _dbContext.Products.Remove(GetProductById(id));
        }
        // Same issue as UpdateAsync
        public async Task DeleteProductAsync(int id)
        {
            _dbContext.Products.Remove(GetProductById(id));
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteProductAsyncExec(int id)
        {
            await _dbContext.Products.Where(item => item.Id == id)
                .ExecuteDeleteAsync();
        }



        public void SaveChanges(Object? o = null) => _dbContext.SaveChanges();  
        public async Task SaveChangesAsync(Object? o = null) => await _dbContext.SaveChangesAsync();



        public void ResetDatabase()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
            _dbContext.ChangeTracker.Clear();
        }



        public IEnumerable<ProductEntity> GetAllProductsByCategory(string category)
        {
            category = category.ToLower();
            return GetAllProducts().Where(p => p.Category.ToLower().Contains(category)).ToList();
        }
        public async Task<IEnumerable<ProductEntity>> GetAllProductsByCategoryAsync(string category)
        {
            category = category.ToLower();
            return await _dbContext.Products.Where(p => p.Category.ToLower().Contains(category)).ToListAsync();
        }



        public IEnumerable<ProductEntity> GetAllProductsByName(string name)
        {
            name = name.ToLower();
            return GetAllProducts().Where(p => p.Name.ToLower().Contains(name)).ToList();
        }
        public async Task<IEnumerable<ProductEntity>> GetAllProductsByNameAsync(string name)
        {
            name = name.ToLower();
            return await _dbContext.Products.Where(p => p.Name.ToLower().Contains(name)).ToListAsync();
        }



        public IEnumerable<ProductEntity> GetOnlyInStockProducts() => GetAllProducts().Where(p => p.Quantity > 0).ToList();
        public async Task<IEnumerable<ProductEntity>> GetOnlyInStockProductsAsync() => await _dbContext.Products.Where(p => p.Quantity > 0).ToListAsync();



        public ProductEntity GetProductById(int id) => GetAllProducts().Where(p => p.Id == id).FirstOrDefault();
        public async Task<ProductEntity> GetProductByIdAsync(int id) => await _dbContext.Products.Where(p => p.Id == id).FirstOrDefaultAsync();



        public ProductEntity GetProductByName(string name)
        {
            name = name.ToLower();
            return GetAllProducts().Where(p => p.Name.ToLower().Contains(name)).FirstOrDefault();
        }
        public async Task<ProductEntity> GetProductByNameAsync(string name)
        {
            name = name.ToLower();
            return await _dbContext.Products.Where(p => p.Name.ToLower().Contains(name)).FirstOrDefaultAsync();
        }

    }
}
