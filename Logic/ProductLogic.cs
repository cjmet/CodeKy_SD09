using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using CodeKY_SD01.Interfaces;
using CodeKY_SD01.Validators;
using FluentValidation;
using FluentValidation.Results;
using CodeKY_SD01.Data;


namespace CodeKY_SD01.Logic
{
    public class ProductLogic : IProductRepository
    {
        private readonly IProductRepository _repository;

        public ProductLogic()
        {
            _repository = new ProductRepository();
        }
        public ProductLogic(IProductRepository productRepository)
        {
            this._repository = productRepository;
        }

        public void DebugDatabaseInit()
        {
            Console.WriteLine("Creating a Debug Database.");

            AddProduct(new ProductEntity("Kitten Chow", "Catfood", "A Delicious Bag of Kitten Chow", 9.87m, 65));
            AddProduct(new ProductEntity("Kittendines", "Catfood", "A Delicious Bag of Sardines just for Kittens", 8.87m, 55));
            AddProduct(new ProductEntity("Void's Vittles for Kittens", "Catfood", "An Empty Bag of Kitten Food", 6.66m, 1));
            AddProduct(new ProductEntity("Kitten Kuts", "Catfood", "A Delicious Bag of Choped Steak for Kittens", 19.87m, 5));
            AddProduct(new ProductEntity("Bad Boy Bumble Bees", "Catfood", "A Delicious Bag of Dried Bumble Bees.  The Purrfect Snack for your one eyed Pirate Cats", 29.87m, 5));
        }

        public void AddProduct(ProductEntity product)
        {
            ProductValidator validator = new ProductValidator();
            ValidationResult result = validator.Validate(product);
            if (!result.IsValid)
            {
                string s = JsonSerializer.Serialize(product, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true });
                result.Errors.Add(new ValidationFailure("product", s));
                throw new ValidationException(result.Errors);
            }
            _repository.AddProduct(product);
        }


        public IEnumerable<ProductEntity> GetAllProducts()
        {
            return _repository.GetAllProducts().ToList();
        }

        public IEnumerable<ProductEntity> GetOnlyInStockProducts() => _repository.GetOnlyInStockProducts();

        public decimal GetTotalPriceOfInventory()
        {
            //return _products.Values.ToList().InStock().Sum(item => item.Price * item.Quantity);
            return GetOnlyInStockProducts().Sum(item => item.Price * item.Quantity);
        }

        public List<string> GetOnlyInStockProductsByName()
        {
            return _repository.GetAllProducts().Where(p => p.Quantity > 0).ToList().Select(p => p.Name).ToList();
        }

        public ProductEntity GetProductByName(string name)
        {
            name = name.ToLower();
            return _repository.GetAllProducts().Where(p => p.Name.ToLower() == name).FirstOrDefault();
        }

        public ProductEntity GetProductById(int Id)
        {
            return _repository.GetProductById(Id);
        }

        public List<ProductEntity> SearchProducts(string name)
        {
            name = name.ToLower();
            return GetAllProducts().Where(p => p.Name.ToLower().Contains(name)).ToList();

        }

        public void UpdateProduct(ProductEntity product) => _repository.UpdateProduct(product);

        public void DeleteProduct(int Id) => _repository.DeleteProduct(Id);

        public IEnumerable<ProductEntity> GetProductsByName(string name) => _repository.GetProductsByName(name);

        public IEnumerable<ProductEntity> GetProductsByCategory(string category) => _repository.GetProductsByCategory(category);


    }
}
