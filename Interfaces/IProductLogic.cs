using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeKY_SD01.Products;

namespace CodeKY_SD01.Interfaces
{
    internal interface IProductLogic
    {
        public void AddProduct(Product product);
        public List<Product> GetAllProducts();
        public DogLeash GetDogLeashByName(string name);
        public CatFood GetCatFoodByName(string name);
        public List<Product> SearchProducts(string name);
        public List<Product> GetOnlyInStockProducts();
        public decimal GetTotalPriceOfInventory();
        public List<string> GetOnlyInStockProductsByName();
        public void DebugDatabaseInit();
        public Product GetTestProduct();
        public T GetProductByName<T>(string name) where T : Product;

	}
}
