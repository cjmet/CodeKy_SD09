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

        public ProductLogic(IProductRepository productRepository, IOrderRepository orderRepository)
        {
            Console.WriteLine("ProductLogic: Constructor");
            _productRepo = productRepository;
            _orderRepo = orderRepository;
            Console.WriteLine(_productRepo.DbPath);
            ResetDatabase();
            throw new Exception("We should not be using this constructor any more, with Dependency Injection.");
        }
        
        public bool DataExists() => _productRepo.DataExists();

        public void ResetDatabase() => _productRepo.ResetDatabase();

        public void DebugDatabaseInit() => _productRepo.DebugDatabaseInit();
        
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
