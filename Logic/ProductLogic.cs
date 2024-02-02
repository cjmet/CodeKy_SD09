using CodeKY_SD01.Validators;
using FluentValidation;
using FluentValidation.Results;
using DataLibrary;


namespace CodeKY_SD01.Logic
{
    public class ProductLogic : IProductRepository
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

        public void AddProduct(DataLibrary.ProductEntity product)
        {
            ProductValidator validator = new ProductValidator();
            ValidationResult result = validator.Validate(product);
            if (GetProductByName(product.Name) != null)
            {
                result.Errors.Add(new ValidationFailure("Name", "Product with that name already exists", product.Name));
            }
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    string shortString = failure.AttemptedValue.ToString();
                    if (shortString.Length > 60)
                        shortString = shortString.Substring(0, 60);
                    Console.WriteLine($"Error [{failure.PropertyName} = {shortString}] \n\t {failure.ErrorMessage}");
                }
                Console.WriteLine();
                return;
            }
            _repository.Add(product);
            _repository.SaveChanges();
        }

        public void UpdateProduct(ProductEntity product)
        {
            _repository.Products.Update(product);
            _repository.SaveChanges();
        }
        public void DeleteProduct(int Id)
        {
            _repository.Products.Remove(GetProductById(Id));
            _repository.SaveChanges();
        }

        public IEnumerable<ProductEntity> GetAllProducts() =>
            _repository.Products.ToList();

        public IEnumerable<ProductEntity> GetOnlyInStockProducts() => _repository.Products.Where(p => p.Quantity > 0).ToList();

        public ProductEntity GetProductByName(string name)
        {
            name = name.ToLower();
            return GetAllProducts().Where(p => p.Name.ToLower() == name).FirstOrDefault();
        }

        public ProductEntity GetProductById(int Id)
        {
            return _repository.Products.Find(Id);
        }

        public List<ProductEntity> SearchProducts(string name)
        {
            name = name.ToLower();
            return GetAllProducts().Where(p => p.Name.ToLower().Contains(name)).ToList();

        }

        public IEnumerable<ProductEntity> GetAllProductsByName(string name)
        {
            name = name.ToLower();
            return _repository.Products.Where(p => p.Name.ToLower().Contains(name)).ToList();
        }

        public IEnumerable<ProductEntity> GetAllProductsByCategory(string category)
        {
            category = category.ToLower();
            return _repository.Products.Where(p => p.Category.ToLower().Contains(category)).ToList();
        }

        public bool VerboseSQL
        {
            get => _repository.VerboseSQL;
            set => _repository.VerboseSQL = value;
        }

    }
}
