namespace DataLibrary
{

    public class OrderEntity
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public List<ProductEntity> Products { get; set; } = new();
    }


    public interface IOrderRepository
    {
        public void AddOrder(OrderEntity order);
        public void AddProductToOrder(int orderId, int productId);
        public void RemoveProductFromOrder(int orderId, int productId);
        public void DeleteOrder(int id);
        public void UpdateOrder(OrderEntity order);
        public OrderEntity GetOrderById(int id);
        public IEnumerable<OrderEntity> GetAllOrders();
    }

}
