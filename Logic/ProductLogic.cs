using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using CodeKY_SD01.Extensions;
using CodeKY_SD01.Interfaces;
using CodeKY_SD01.Products;
using CodeKY_SD01.Validators;
using FluentValidation;
using FluentValidation.Results;

namespace CodeKY_SD01.Logic
{
    public class ProductLogic : IProductLogic
    {
        // private List<Product> _products = new List<Product>();
        private Dictionary<string, Product> _products = new Dictionary<string, Product>();
        private Dictionary<string, CatFood> _catFoods = new Dictionary<string, CatFood>();
        private Dictionary<string, DogLeash> _dogLeashs = new Dictionary<string, DogLeash>();

        public void DebugDatabaseInit()
        {
            Console.WriteLine("Creating a Debug Database.");

            AddProduct(new CatFood("Kitten Chow", "A Delicious Bag of Kitten Chow", 9.87m, 65, 4.32, true));
            AddProduct(new CatFood("Kittendines", "A Delicious Bag of Sardines just for Kittens", 8.87m, 55, 3.32, true));
            AddProduct(new CatFood("Void's Vittles for Kittens", "An Empty Bag of Kitten Food", 6.66m, 1, 0.01, true));
            AddProduct(new CatFood("Kitten Kuts", "A Delicious Bag of Choped Steak for Kittens", 19.87m, 15, 2.32, true));
            AddProduct(new CatFood("Bad Boy Bumble Bees", "A Delicious Bag of Dried Bumble Bees.  The Purrfect Snack for your one eyed Pirate Cats", 29.87m, 5, 1.32, false));


            Console.WriteLine();
        }

		public Product GetTestProduct()
		{
			Console.WriteLine("Creating a Test Product");
    		return (Product) new CatFood("Soylent Green for Kittens", "Saving the World one Kitten at a Time.", 6.67m, 96, 7.66, true);
		}

		public void AddProduct(Product product)
        {
			ProductValidator validator = new ProductValidator();
            ValidationResult result = validator.Validate(product);
            if (!result.IsValid)
            {
                string s = JsonSerializer.Serialize(product, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true });
                result.Errors.Add(new ValidationFailure("product", s));
                throw new ValidationException(result.Errors);
            }

			_products.Add(product.Name.ToLower(), product);

            if (product is DogLeash)
            {
                Console.WriteLine("Adding a Dog Leash");
                _dogLeashs.Add(product.Name.ToLower(), product as DogLeash);
            }
            else if (product is CatFood)
            {
                Console.WriteLine("Adding a Cat Food");
                _catFoods.Add(product.Name.ToLower(), product as CatFood);
            }
            else
            {
                Console.WriteLine("Adding a Generic Product");
            }
        }

        public List<Product> GetAllProducts()
        {
            List<Product> _list = new List<Product>();
            foreach (var item in _products) { _list.Add(item.Value); }
            return _list;
        }

        /// <summary>
        /// Get a generic List of only In-Stock Products 
        /// </summary>
        /// <returns>A generic List of only In-Stock Products</returns>
        public List<Product> GetOnlyInStockProducts()
        {
            //var query =
            //	from item in _products
            //	where item.Value.Quantity > 0
            //	select item.Value;
            //var results = query.ToList();
            //return results;

            return _products.Values.ToList().InStock();
        }

        public decimal GetTotalPriceOfInventory()
        {
            //return _products.Values.ToList().InStock().Sum(item => item.Price * item.Quantity);
            return GetOnlyInStockProducts().Sum(item => item.Price * item.Quantity);
        }

        public List<string> GetOnlyInStockProductsByName()
        {
            var query =
                from item in _products
                where item.Value.Quantity > 0
                select item.Value.Name;
            var results = query.ToList();
            return results;
        }

        // Return the dogleash or null
        public DogLeash GetDogLeashByName(string name)
        {
            name = name.ToLower();
            return _dogLeashs.ContainsKey(name) ? _dogLeashs[name] : null;
        }

		public T GetProductByName <T> (string name) where T : Product
		{
			name = name.ToLower();
                        
            return (_products.ContainsKey(name) ? _products[name] : null) as T;
		}

		// Return the catfood or null
		public CatFood GetCatFoodByName(string name)
        {
            name = name.ToLower();
            return _catFoods.ContainsKey(name) ? _catFoods[name] : null;
        }


        public List<Product> SearchProducts(string name)
        {
            name = name.ToLower();
            List<Product> _list = new List<Product>();
            foreach (var item in _products)
            {
                if (item.Key.Contains(name)) { _list.Add(item.Value); }
            }
            return _list;
        }

    }
}
