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

        public string ProductInterfaceFilename => "ProductLogic";
        public string ProductInterfaceFunctionName()  => "ProductLogic";
        public string ProductDbPath { get => _productRepo.ProductDbPath; }
        public string OrderInterfaceFilename => "ProductLogic";
        public string OrderInterfaceFunctionName() => "ProductLogic";
        public string OrderDbPath { get => _orderRepo.OrderDbPath; }
        public bool VerboseSQL
        {
            get => _productRepo.VerboseSQL;
            set => _productRepo.VerboseSQL = value;
        }


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
            _productRepo = productRepository;
            _orderRepo = orderRepository;
        }

        public bool DataExists() => _productRepo.DataExists();

        public void ResetDatabase() => _productRepo.ResetDatabase();

        public void AddProduct(ProductEntity product) => AddProduct(product, false);
        
        public void AddProduct(ProductEntity product, bool Quiet = false)
        {
            _productRepo.AddProduct(product);

            //ProductValidator validator = new ProductValidator();
            //ValidationResult result = validator.Validate(product);
            //if (GetProductByName(product.Name) != null)
            //{
            //    result.Errors.Add(new ValidationFailure("Name", "Product with that name already exists", product.Name));
            //}
            //if (!result.IsValid)
            //{
            //    if (!Quiet)
            //    {
            //        foreach (var failure in result.Errors)
            //        {
            //            string shortString = failure.AttemptedValue.ToString();
            //            if (shortString.Length > 60)
            //                shortString = shortString.Substring(0, 60);
            //            Console.WriteLine($"Error [{failure.PropertyName} = {shortString}] \n\t {failure.ErrorMessage}");
            //        }
            //        Console.WriteLine();
            //    }
            //    return;
            //}
            //_productRepo.AddProduct(product);
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
            _orderRepo.AddProductToOrder(orderId, productId);

            //OrderEntity order = GetOrderById(orderId);
            //order.Products.Add(GetProductById(productId));
        }

        public void RemoveProductFromOrder(int orderId, int productId)
        {
            _orderRepo.RemoveProductFromOrder(orderId, productId);

            //OrderEntity order = GetOrderById(orderId);
            //order.Products.Remove(GetProductById(productId));
        }

        public void DeleteOrder(int id) => _orderRepo.DeleteOrder(id);

        public void UpdateOrder(OrderEntity order) => _orderRepo.UpdateOrder(order);

        public OrderEntity GetOrderById(int id) => _orderRepo.GetOrderById(id);

        public IEnumerable<OrderEntity> GetAllOrders() => _orderRepo.GetAllOrders();

        void IProductRepository.SaveChanges(Object? o = null)
        {
            throw new NotImplementedException();
        }

        void IOrderRepository.SaveChanges(Object? o = null)
        {
            throw new NotImplementedException();
        }

        IEnumerable<ProductEntity> IProductRepository.GetAllProductsWeb()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<ProductEntity>> IProductRepository.GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        Task IProductRepositoryAsync.AddProductAsync(ProductEntity product)
        {
            throw new NotImplementedException();
        }

        Task IProductRepositoryAsync.DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task IProductRepositoryAsync.DeleteProductAsyncExec(int id)
        {
            throw new NotImplementedException();
        }

        Task IProductRepositoryAsync.UpdateProductAsync(ProductEntity product)
        {
            throw new NotImplementedException();
        }

        Task IProductRepositoryAsync.SaveChangesAsync(object o)
        {
            throw new NotImplementedException();
        }

        public Task<ProductEntity> GetProductByIdAsync(int id) => _productRepo.GetProductByIdAsync(id);

        Task<ProductEntity> IProductRepositoryAsync.GetProductByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<ProductEntity>> IProductRepositoryAsync.GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<ProductEntity>> IProductRepositoryAsync.GetAllProductsByCategoryAsync(string category)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<ProductEntity>> IProductRepositoryAsync.GetAllProductsByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<ProductEntity>> IProductRepositoryAsync.GetOnlyInStockProductsAsync()
        {
            throw new NotImplementedException();
        }

        void IProductRepository.ClearChangeTracker()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductEntity> GetAllProductsWeb => _productRepo.GetAllProductsWeb();
    }
}
