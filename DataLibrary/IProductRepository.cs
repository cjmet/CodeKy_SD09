namespace DataLibrary
{
    public interface IProductRepository
    {
        public string DbPath { get; }
        public void AddProduct(ProductEntity product);
        public void DeleteProduct(int id);
        public void UpdateProduct(ProductEntity product);
        // public void SaveChanges();
        public ProductEntity GetProductById(int id);
        public ProductEntity GetProductByName(string name);
        public IEnumerable<ProductEntity> GetAllProducts();
        public IEnumerable<ProductEntity> GetAllProductsByCategory(string category);
        public IEnumerable<ProductEntity> GetAllProductsByName(string name);
        public IEnumerable<ProductEntity> GetOnlyInStockProducts();
        public bool VerboseSQL { get; set; }
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
