namespace DataLibrary
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public List<ProductEntity> Products { get; set; } = new List<ProductEntity>();
    }

}
