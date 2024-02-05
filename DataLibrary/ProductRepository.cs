using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;



namespace DataLibrary
{
    public class ProductRepository : IProductLogic
    {
        
        private readonly ProductContext _dbContext;
                public ProductRepository()
        {
            _dbContext = new ProductContext();
        }



        public string DbPath => _dbContext.DbPath;

        public bool VerboseSQL { get => _dbContext.VerboseSQL; set => _dbContext.VerboseSQL = value; }
        public bool DataExists() => _dbContext.Products.Count() > 0 || _dbContext.Orders.Count() > 0;

        public void AddOrder(OrderEntity order)
        {
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
        }

        public void AddProduct(ProductEntity product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
        }

        public void AddProductToOrder(int orderId, int productId)
        {
            throw new NotImplementedException();
        }

        public void DebugDatabaseInit()
        {
            Console.WriteLine("ProductRepository: Debug Database Init");
            if (DataExists())
            {
                Console.WriteLine("Database Already Contains Data.");
                return;
            }
            else
            {
                Console.WriteLine("Adding Test Products.");
                AddProduct(new ProductEntity("Kitten Chow", "Catfood", "A Delicious Bag of Kitten Chow", 9.87m, 65));
                AddProduct(new ProductEntity("Kitten Chow", "Catfood", "A Delicious Bag of Kitten Chow", 9.87m, 65));
                AddProduct(new ProductEntity("Kittendines", "Catfood", "A Delicious Bag of Sardines just for Kittens", 8.87m, 55));
                AddProduct(new ProductEntity("Void's Vittles for Kittens", "Catfood", "An Empty Bag of Kitten Food", 6.66m, 1));
                AddProduct(new ProductEntity("Kitten Kuts", "Catfood", "A Delicious Bag of Choped Steak for Kittens", 19.87m, 5));
                AddProduct(new ProductEntity("Bad Boy Bumble Bees", "Catfood", "A Delicious Bag of Dried Bumble Bees.  The Purrfect Snack for your one eyed Pirate Cats", 29.87m, 5));
                AddProduct(new ProductEntity("Puppy Chow", "Dogfood", "A Delicious Bag of Puppy Chow", 9.87m, 65));



                Console.WriteLine("Adding Test Orders.");
                var product1 = GetProductByName("Kitten Chow");
                var product2 = GetProductByName("Kittendines");
                if (product1 != null && product2 != null)
                    AddOrder(new OrderEntity() { OrderDate = DateTime.Now, Products = { product1, product2 } });

                product1 = GetProductByName("Void");
                product2 = GetProductByName("Kuts");
                if (product1 != null && product2 != null)
                    AddOrder(new OrderEntity() { OrderDate = DateTime.Now, Products = { product1, product2 } });

                product1 = GetProductByName("Bees");
                product2 = GetProductByName("Puppy");
                if (product1 != null && product2 != null)
                    AddOrder(new OrderEntity() { OrderDate = DateTime.Now, Products = { product1, product2 } });

                return;
            }
        }

        public void DeleteOrder(int id)
        {
           _dbContext.Orders.Remove(GetOrderById(id));
            _dbContext.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            _dbContext.Products.Remove(GetProductById(id));
            _dbContext.SaveChanges();
        }

        public IEnumerable<OrderEntity> GetAllOrders() => _dbContext.Orders.Include(o => o.Products).ToList();
        
        
        public IEnumerable<ProductEntity> GetAllProducts() => _dbContext.Products.ToList();


        public IEnumerable<ProductEntity> GetAllProductsByCategory(string category)
        {
            category = category.ToLower();
            return _dbContext.Products.Where(p => p.Category.ToLower().Contains(category)).Include(p => p.Orders).ToList();
        }

        public IEnumerable<ProductEntity> GetAllProductsByName(string name)
        {
            name = name.ToLower();
            return _dbContext.Products.Where(p => p.Name.ToLower().Contains(name)).Include(p => p.Orders).ToList();
        }

        public IEnumerable<ProductEntity> GetOnlyInStockProducts() => _dbContext.Products.Where(p => p.Quantity > 0).ToList();

        public OrderEntity GetOrderById(int id) => _dbContext.Orders.Where(o => o.Id == id).Include(o => o.Products).FirstOrDefault();

        public ProductEntity GetProductById(int id) => _dbContext.Products.Where(p => p.Id == id).Include(p => p.Orders).FirstOrDefault();

        public ProductEntity GetProductByName(string name)
        {
            name = name.ToLower();
            return GetAllProducts().Where(p => p.Name.ToLower().Contains(name)).FirstOrDefault();
        }

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
    }
}
