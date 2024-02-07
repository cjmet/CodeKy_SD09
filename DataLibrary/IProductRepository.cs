namespace DataLibrary
{
    public interface IProductRepository
    {
        public string ProductInterfaceFilename { get => "IProductRepository"; }
        public string ProductInterfaceFunctionName() => "IProductRespository";
        public string ProductDbPath { get => "IProductRepository"; }
        public bool VerboseSQL { get; set; }
        public void ResetDatabase();        // Special for DebugDatabaseInit
        public bool DataExists();            // Special for DebugDatabaseInit


        public void AddProduct(ProductEntity product);
        public void DeleteProduct(int id);
        public void UpdateProduct(ProductEntity product);
        public void SaveChanges(Object o = null);       // A separate method for saving changes is probably the best practice
        public ProductEntity GetProductById(int id);
        public ProductEntity GetProductByName(string name);
        public IEnumerable<ProductEntity> GetAllProducts();
        public IEnumerable<ProductEntity> GetAllProductsByCategory(string category);
        public IEnumerable<ProductEntity> GetAllProductsByName(string name);
        public IEnumerable<ProductEntity> GetOnlyInStockProducts();
    }
 
}
