using CodeKY_SD01.Validators;
using DataLibrary;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;


namespace CodeKY_SD01.Logic
{
    public class ProductLogic : IProductLogic
    {
        private readonly IProductRepository _productRepo;
        private readonly IOrderRepository _orderRepo;
        public string DbPath { get => _productRepo.DbPath; }

        //public ProductLogic(IProductLogic productLogic)
        //{
        //    Console.WriteLine("ProductLogic: Constructor");
        //    _productRepo = productLogic;
        //    _orderRepo = productLogic;
        //    Console.WriteLine(_productRepo.DbPath);
        //    ResetDatabase();
        //}
        public ProductLogic(IProductRepository productRepository, IOrderRepository orderRepository)
        {
            Console.WriteLine("ProductLogic: Constructor");
            _productRepo = productRepository;
            _orderRepo = orderRepository;
            Console.WriteLine(_productRepo.DbPath);
            ResetDatabase();
        }
        
        public bool DataExists() => _productRepo.DataExists();

        public void ResetDatabase() => _productRepo.ResetDatabase();

        public void DebugDatabaseInit()
        {
            if (DataExists())
            {
                Console.WriteLine("Order Repository Already Contains Data.");
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
            _productRepo.AddProduct(product);
        }

        public void AddOrder(OrderEntity order) => _orderRepo.AddOrder(order);

        public void UpdateProduct(ProductEntity product) => _productRepo.UpdateProduct(product);

        public void DeleteProduct(int Id) => _productRepo.DeleteProduct(Id);

        public IEnumerable<ProductEntity> GetAllProducts() => _productRepo.GetAllProducts();

        public IEnumerable<ProductEntity> GetOnlyInStockProducts() => _productRepo.GetOnlyInStockProducts();

        public ProductEntity GetProductByName(string name) => _productRepo.GetProductByName(name);


        public ProductEntity GetProductById(int Id) => _productRepo.GetProductById(Id);

        public IEnumerable<ProductEntity> GetAllProductsByName(string name) => _productRepo.GetAllProductsByName(name);


        public IEnumerable<ProductEntity> GetAllProductsByCategory(string category) => _productRepo.GetAllProductsByCategory(category);

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

        public void DeleteOrder(int id) => _orderRepo.DeleteOrder(id);

        public void UpdateOrder(OrderEntity order) => _orderRepo.UpdateOrder(order);

        public OrderEntity GetOrderById(int id) => _orderRepo.GetOrderById(id);

        public IEnumerable<OrderEntity> GetAllOrders() => _orderRepo.GetAllOrders();

        public bool VerboseSQL
        {
            get => _productRepo.VerboseSQL;
            set => _productRepo.VerboseSQL = value;
        }

    }
}
