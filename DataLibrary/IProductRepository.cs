namespace DataLibrary
{
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
        public List<OrderEntity> Orders { get; set; } = new();
    }


    public interface IProductRepository
    {
        public string DbPath { get; }
        public bool VerboseSQL { get; set; }
        public void ResetDatabase();        // Special for DebugDatabaseInit
        public bool DataExists();            // Special for DebugDatabaseInit
        public void DebugDatabaseInit();    // Special


        public void AddProduct(ProductEntity product);
        public void DeleteProduct(int id);
        public void UpdateProduct(ProductEntity product);
        // public void SaveChanges();       // A separate method for saving changes is probably the best practice
        public ProductEntity GetProductById(int id);
        public ProductEntity GetProductByName(string name);
        public IEnumerable<ProductEntity> GetAllProducts();
        public IEnumerable<ProductEntity> GetAllProductsByCategory(string category);
        public IEnumerable<ProductEntity> GetAllProductsByName(string name);
        public IEnumerable<ProductEntity> GetOnlyInStockProducts();
    }
 
}
