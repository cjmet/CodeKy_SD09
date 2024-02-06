namespace DataLibrary
{
    public interface IOrderRepository
    {
        public string OrderInterfaceFilename { get => "IOrderRepository"; }
        public string OrderInterfaceFunctionName() => "IOrderRespository";
        public string OrderDbPath { get => "IOrderRespository"; }
        public void AddOrder(OrderEntity order);
        public void AddProductToOrder(int orderId, int productId);
        public void RemoveProductFromOrder(int orderId, int productId);
        public void DeleteOrder(int id);
        public void UpdateOrder(OrderEntity order);
        public OrderEntity GetOrderById(int id);
        public IEnumerable<OrderEntity> GetAllOrders();
    }

}
