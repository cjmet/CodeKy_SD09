using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;



namespace DataLibrary
{
    public class OrderRepository : IOrderRepository
    {
        public string OrderInterfaceFilename => "OrderRepository";
        public string OrderInterfaceFunctionName() => "OrderRepository";
        public string OrderDbPath => _dbContext.DbPath;

        private readonly StoreContext _dbContext;
        public OrderRepository(StoreContext DIContext)
        {
            //_dbContext = new ProductContext();
            _dbContext = DIContext;
            Console.WriteLine($"Order   ContextId: {_dbContext.ContextId}");
        }

        public string DbPath => _dbContext.DbPath;



        public IEnumerable<OrderEntity> GetAllOrders() => _dbContext.Orders.Include(o => o.Products).ToList();
        //public IEnumerable<OrderEntity> GetAllOrders() => _dbContext.Orders.ToList();

        public void AddOrder(OrderEntity order)
        {
            _dbContext.Orders.Add(order);
        }

        public void UpdateOrder(OrderEntity order)
        {
            _dbContext.Orders.Update(order);
        }

        public void DeleteOrder(int id)
        {
            _dbContext.Orders.Remove(GetOrderById(id));
        }

        public void SaveChanges(Object? o = null) => _dbContext.SaveChanges();  

        public void ResetDatabase()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }




        public OrderEntity GetOrderById(int id) => GetAllOrders().Where(o => o.Id == id).FirstOrDefault();

        public void RemoveProductFromOrder(int orderId, int productId)
        {
            throw new NotImplementedException();
        }
        
        public void AddProductToOrder(int orderId, int productId)
        {
            throw new NotImplementedException();
        }
    }
}
