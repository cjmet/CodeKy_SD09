﻿using System;
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
        }

        public string DbPath => _dbContext.DbPath;



        public IEnumerable<OrderEntity> GetAllOrders() => _dbContext.Orders.Include(o => o.Products).AsNoTracking().ToList();
        //public IEnumerable<OrderEntity> GetAllOrders() => _dbContext.Orders.ToList();

        public void AddOrder(OrderEntity order)
        {
            _dbContext.ChangeTracker.Clear();
            OrderEntity tmp = new OrderEntity();
            tmp.OrderDate = order.OrderDate;
            _dbContext.Orders.Add(tmp);
            foreach (var product in order.Products)
            {
                _dbContext.Products.Attach(product); // cjm
                tmp.Products.Add(product);             
            }
            _dbContext.SaveChanges();
            _dbContext.ChangeTracker.Clear();        // cjm 
        }

        public void UpdateOrder(OrderEntity order)
        {
            _dbContext.Orders.Update(order);
            _dbContext.SaveChanges();
        }

        public void DeleteOrder(int id)
        {
            _dbContext.Orders.Remove(GetOrderById(id));
            _dbContext.SaveChanges();
            _dbContext.ChangeTracker.Clear();
        }
        public void ResetDatabase()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
            //_dbContext.ChangeTracker.Clear();     // She's Dead , Jim.  Surely we don't need to explicitly clear the ChangeTracker after EnsureDeleted();
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
