using Microsoft.EntityFrameworkCore;

namespace DataLibrary
{
    public interface IProductRepositoryAsync
    {
        public Task<ProductEntity> GetProductByIdAsync(int id);
        public Task<ProductEntity> GetProductByNameAsync(string name);

        public Task AddProductAsync(ProductEntity product);
        public Task UpdateProductAsync(ProductEntity product);
        public Task SaveChangesAsync(Object o = null);     
        public Task DeleteProductAsync(int id);
		public Task DeleteProductAsyncExec(int id);
        
        public Task<IEnumerable<ProductEntity>> GetAllProductsAsync();
        public Task<IEnumerable<ProductEntity>> GetOnlyInStockProductsAsync();
		public Task<IEnumerable<ProductEntity>> GetAllProductsByNameAsync(string name);
		public Task<IEnumerable<ProductEntity>> GetAllProductsByCategoryAsync(string category);
    }
}
