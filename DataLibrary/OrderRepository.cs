using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;



namespace DataLibrary
{
    public class OrderRepository : ProductRepository, IOrderRepository
    {
        
        private readonly ProductContext _dbContext;
        public OrderRepository()
        {
            _dbContext = new ProductContext();
        }

        public string DbPath => _dbContext.DbPath;

        public void AddOrder(OrderEntity order)
        {
            _dbContext.ChangeTracker.Clear();
            Console.WriteLine("Adding Order to Database");
            Console.WriteLine($"Tracking: {_dbContext.Orders.Entry(order).State}");
            Console.WriteLine($"Order ID: {order.Id}");
            Console.WriteLine($"Order Date: {order.OrderDate}");
            Console.WriteLine($"Order Products: {order.Products.Count}");
            Console.WriteLine();

            foreach (var product in order.Products)
            {
                Console.WriteLine($"Product ID: {product.Id}");
                Console.WriteLine($"Tracking: {_dbContext.Products.Entry(product).State}");
                Console.WriteLine($"Product Name: {product.Name}");
                Console.WriteLine();
            }

            _dbContext.ChangeTracker.Clear();
            //foreach (var product in order.Products)
            //    if (_dbContext.Products.Entry(product).State == EntityState.Detached)
            //        _dbContext.Products.Attach(product); // cjm  

            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();       // cjm
        }

        public void DeleteOrder(int id)
        {
           _dbContext.Orders.Remove(GetOrderById(id));
            _dbContext.SaveChanges();
        }

        public IEnumerable<OrderEntity> GetAllOrders() => _dbContext.Orders.Include(o => o.Products).ToList();

        public OrderEntity GetOrderById(int id) => _dbContext.Orders.Where(o => o.Id == id).Include(o => o.Products).FirstOrDefault();

        public void RemoveProductFromOrder(int orderId, int productId)
        {
            throw new NotImplementedException();
        }

        public void ResetDatabase()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }

        public void UpdateOrder(OrderEntity order)
        {
            _dbContext.Orders.Update(order);
            _dbContext.SaveChanges();
        }

        public void UpdateProduct(ProductEntity product)
        {
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();
        }

        void IOrderRepository.AddProductToOrder(int orderId, int productId)
        {
            throw new NotImplementedException();
        }
    }
}
