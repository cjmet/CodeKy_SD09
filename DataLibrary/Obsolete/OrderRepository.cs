


namespace DataLibrary
{

    public class OrderRepository : IOrderRepository
    {
        private readonly ProductContext _context = new ProductContext();

        public string DbPath { get => _context.DbPath; }
        
        public bool VerboseSQL
        {
            get => _context.VerboseSQL;
            set => _context.VerboseSQL = value;
        }

        public void AddOrder(OrderEntity order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void DeleteOrder(int id)
        {
            var order = _context.Orders.Find(id);
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }

        public void UpdateOrder(OrderEntity order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void AddProductToOrder(int orderId, int productId)
        {
            var order = _context.Orders.Find(orderId);
            var product = _context.Products.Find(productId);
            order.Products.Add(product);
            product.Orders.Add(order);  
            _context.SaveChanges();
        }

        public void RemoveProductFromOrder(int orderId, int productId)
        {
            var order = _context.Orders.Find(orderId);
            var product = _context.Products.Find(productId);
            order.Products.Remove(product);
            product.Orders.Remove(order);
            _context.SaveChanges();
        }

        public IEnumerable<OrderEntity> GetAllOrders()
        {
            return _context.Orders.ToList();
        }

        public OrderEntity GetOrderById(int id)
        {
            var order =  _context.Orders.Find(id);
            _context.Entry(order).Collection(o => o.Products).Load();
            return order;
        }
    }
}

