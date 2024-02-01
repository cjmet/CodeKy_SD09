

namespace DataLibrary
{

    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context = new ProductContext();

        public string DbPath { get => _context.DbPath; }

        public void AddProduct(ProductEntity product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            _context.Products.Remove(GetProductById(id));
            _context.SaveChanges();
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
        public IEnumerable<ProductEntity> GetAllProductsByCategory(string category)
        {
            category = category.ToLower();
            return _context.Products.Where(p => p.Category.Contains(category)).ToList();
        }

        public IEnumerable<ProductEntity> GetAllProductsByName(string name)
        {
            name = name.ToLower();
            return _context.Products.Where(p => p.Name.ToLower().Contains(name)).ToList();
        }

        public IEnumerable<ProductEntity> GetOnlyInStockProducts() => _context.Products.Where(p => p.Quantity > 0).ToList();

        ProductEntity IProductRepository.GetProductByName(string name)
            => _context.Products
            .Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();

    }
}

