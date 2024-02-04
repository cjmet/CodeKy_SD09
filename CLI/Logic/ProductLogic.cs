using CodeKY_SD01.Validators;
using DataLibrary;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;


namespace CodeKY_SD01.Logic
{
    public class ProductLogic : IProductRepository, IOrderRepository
    {
        private readonly ProductContext _repository;
        public string DbPath { get => _repository.DbPath; }
        public ProductLogic()
        {
            _repository = new ProductContext();

        }
        public ProductLogic(ProductContext productRepository)
        {
            this._repository = productRepository;
            if (_repository.Products.Count() > 0 || _repository.Orders.Count() > 0) Seeded = true;
            else
            {
                _repository.Database.EnsureDeleted();
                _repository.Database.EnsureCreated();
            }
        }
        public bool Seeded { get; set; } = false;


        public void DebugDatabaseInit()
        {

            if (Seeded)
            {
                Console.WriteLine("Database Data:");
                return;
            }
            else
            {
                Seeded = true;
                bool Quiet = true;
                Console.WriteLine("Adding Test Products.");
                AddProduct(new ProductEntity("Kitten Chow", "Catfood", "A Delicious Bag of Kitten Chow", 9.87m, 65), Quiet);
                AddProduct(new ProductEntity("Kitten Chow", "Catfood", "A Delicious Bag of Kitten Chow", 9.87m, 65), Quiet);
                AddProduct(new ProductEntity("Kittendines", "Catfood", "A Delicious Bag of Sardines just for Kittens", 8.87m, 55), Quiet);
                AddProduct(new ProductEntity("Void's Vittles for Kittens", "Catfood", "An Empty Bag of Kitten Food", 6.66m, 1), Quiet);
                AddProduct(new ProductEntity("Kitten Kuts", "Catfood", "A Delicious Bag of Choped Steak for Kittens", 19.87m, 5), Quiet);
                AddProduct(new ProductEntity("Bad Boy Bumble Bees", "Catfood", "A Delicious Bag of Dried Bumble Bees.  The Purrfect Snack for your one eyed Pirate Cats", 29.87m, 5), Quiet);
                AddProduct(new ProductEntity("Puppy Chow", "Dogfood", "A Delicious Bag of Puppy Chow", 9.87m, 65), Quiet);



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
            return;
        }



        public void AddProduct(ProductEntity product) => AddProduct(product, false);
        public void AddProduct(ProductEntity product, bool Quiet = false)
        {
            ProductValidator validator = new ProductValidator();
            ValidationResult result = validator.Validate(product);
            if (GetProductByName(product.Name) != null)
            {
                result.Errors.Add(new ValidationFailure("Name", "Product with that name already exists", product.Name));
            }
            if (!result.IsValid)
            {
                if (!Quiet)
                {
                    foreach (var failure in result.Errors)
                    {
                        string shortString = failure.AttemptedValue.ToString();
                        if (shortString.Length > 60)
                            shortString = shortString.Substring(0, 60);
                        Console.WriteLine($"Error [{failure.PropertyName} = {shortString}] \n\t {failure.ErrorMessage}");
                    }
                    Console.WriteLine();
                }
                return;
            }
            _repository.Add(product);
            _repository.SaveChanges();
        }

        public void AddOrder(OrderEntity order)
        {
            //_repository.Products.UpdateRange(order.Products);
            _repository.Orders.Add(order);
            _repository.SaveChanges();
        }

        public void UpdateProduct(ProductEntity product)
        {
            _repository.Products.Update(product);
            _repository.SaveChanges();
        }
        public void DeleteProduct(int Id)
        {
            //_repository.Orders.UpdateRange(GetProductById(Id).Orders);
            _repository.Products.Remove(GetProductById(Id));
            _repository.SaveChanges();
        }

        public IEnumerable<ProductEntity> GetAllProducts() =>
            _repository.Products.Include(p => p.Orders).ToList();

        public IEnumerable<ProductEntity> GetOnlyInStockProducts() => _repository.Products.Where(p => p.Quantity > 0).ToList();

        public ProductEntity GetProductByName(string name)
        {
            name = name.ToLower();
            return GetAllProducts().Where(p => p.Name.ToLower().Contains(name)).FirstOrDefault();
        }

        public ProductEntity GetProductById(int Id)
        {
            return _repository.Products.Where(p => p.Id == Id).Include(p => p.Orders).FirstOrDefault();
        }

        public IEnumerable<ProductEntity> GetAllProductsByName(string name)
        {
            name = name.ToLower();
            return _repository.Products.Where(p => p.Name.ToLower().Contains(name)).Include(p => p.Orders).ToList();
        }

        public IEnumerable<ProductEntity> GetAllProductsByCategory(string category)
        {
            category = category.ToLower();
            return _repository.Products.Where(p => p.Category.ToLower().Contains(category)).Include(p => p.Orders).ToList();
        }

        public void AddProductToOrder(int orderId, int productId)
        {
            OrderEntity order = GetOrderById(orderId);

            order.Products.Add(GetProductById(productId));
        }

        public void RemoveProductFromOrder(int orderId, int productId)
        {
            OrderEntity order = GetOrderById(orderId);
            order.Products.Remove(GetProductById(productId));
        }

        public void DeleteOrder(int id)
        {
            //_repository.Products.UpdateRange(GetOrderById(id).Products);
            _repository.Orders.Remove(GetOrderById(id));
            _repository.SaveChanges();
        }

        public void UpdateOrder(OrderEntity order)
        {
            _repository.Orders.Update(order);
        }

        public OrderEntity GetOrderById(int id)
        {
            return _repository.Orders.Where(o => o.Id == id).Include(o => o.Products).FirstOrDefault();
        }

        public IEnumerable<OrderEntity> GetAllOrders()
        {
            return _repository.Orders.Include(o => o.Products).ToList();
        }

        public bool VerboseSQL
        {
            get => _repository.VerboseSQL;
            set => _repository.VerboseSQL = value;
        }

    }
}
