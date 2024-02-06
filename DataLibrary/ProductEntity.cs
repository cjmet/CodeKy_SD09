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
        public List<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
    }
 
}
