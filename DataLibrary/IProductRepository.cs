namespace DataLibrary
{
    public interface IProductRepository
    {
        public string DbPath { get; }
        public bool VerboseSQL { get; set; }
        public bool Seeded { get; set; }
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
        public void DebugDatabaseInit();
    }

    public interface IOrderRepository
    {
        public string DbPath { get; }
        public bool VerboseSQL { get; set; }
        public void AddOrder(OrderEntity order);
        public void AddProductToOrder(int orderId, int productId);
        public void RemoveProductFromOrder(int orderId, int productId);
        public void DeleteOrder(int id);
        public void UpdateOrder(OrderEntity order);
        public OrderEntity GetOrderById(int id);
        public IEnumerable<OrderEntity> GetAllOrders();
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
        public List<OrderEntity> Orders { get; set; } = new();
    }

    public class OrderEntity
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public List<ProductEntity> Products { get; set; } = new();
    }
}
