using Microsoft.EntityFrameworkCore;
using DataLibrary;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using System.Xml.Linq;
namespace ConsoleToWebAPI.Endpoints;

public static class ProductEntityEndpoints
{
    public static void MapProductEntityEndpoints(this IEndpointRouteBuilder routes)
    {

        // ================================
        // Custom Endpoints // cjm 
        var iProductRepositoryEndpoints = routes.MapGroup("/api/IProductRepository").WithTags(nameof(IProductRepository));

        iProductRepositoryEndpoints.MapGet("/GetAllProducts", (IProductRepository productRepository) =>
        {
            return productRepository.GetAllProducts();
        })
        .WithName("GetAllProducts")
        .WithOpenApi()
        .WithTags("Z","Depreciated") 
        .WithDescription("<h1>THIS WILL FAIL:</h1>" +
            "<h3>System.Text.Json.JsonException: A possible object cycle was detected. This can either <br>>" +
            "be due to a cycle or if the object depth is larger than the maximum allowed depth <br>" +
            "of 64. Consider using ReferenceHandler.Preserve on JsonSerializerOptions to support cycles. <br>" +
            "Path: $.Orders.Products.Orders.Products.Orders.Products.Orders. ... .Products.Orders.Id. </h3>");  
        // <h3> <font size=\"+1\"> 

        iProductRepositoryEndpoints.MapGet("/GetAllProductsWeb", (IProductRepository productRepository) =>
        {
            return productRepository.GetAllProductsWeb();
        })
        .WithName("GetAllProductsWeb")
        .WithOpenApi();

        iProductRepositoryEndpoints.MapGet("/GetAllProductsAsync", (IProductRepository productRepository) =>
        {
            return productRepository.GetAllProductsAsync();
        })
        .WithName("GetAllProductsWebAsync")
        .WithOpenApi();

        iProductRepositoryEndpoints.MapGet("/GetOnlyInStockProductsAsync", (IProductRepository productRepository) =>
        {
            return productRepository.GetOnlyInStockProductsAsync();
        })
        .WithName("GetOnlyInStockProductsAsync")
        .WithOpenApi();

        //iProductRepositoryEndpoints.MapGet("/tmpname/{id}", (IProductRepository productRepository, int id) =>
        //{
        //    return productRepository.tmpname();
        //})
        //.WithName("tmpname")
        //.WithOpenApi();

        iProductRepositoryEndpoints.MapGet("/GetAllProductsByNameAsync/{name}", (IProductRepository productRepository, string name) =>
        {
            return productRepository.GetAllProductsByNameAsync(name);
        })
        .WithName("GetAllProductsByNameAsync")
        .WithOpenApi();

        iProductRepositoryEndpoints.MapGet("/GetAllProductsByCategoryAsync/{category}", (IProductRepository productRepository, string category) =>
        {
            return productRepository.GetAllProductsByCategoryAsync(category);
        })
        .WithName("GetAllProductsByCategoryAsync")
        .WithOpenApi();

        iProductRepositoryEndpoints.MapGet("/GetProductByIdAsync/{id}", (IProductRepository productRepository, int id) =>
        {
            return productRepository.GetProductByIdAsync(id);
        })
            .WithName("GetProductByIdAsync")
            .WithOpenApi();

        iProductRepositoryEndpoints.MapGet("/GetProductByNameAsync/{name}", (IProductRepository productRepository, string name) =>
        {
            return productRepository.GetProductByNameAsync(name);
        })
            .WithName("GetProductByNameAsync")
            .WithOpenApi();

        // Post is Create, ... Put is Edit
        iProductRepositoryEndpoints.MapPost("/AddProductAsync/{id}", (IProductRepository productRepository, ProductEntity product) =>
        {
            return productRepository.AddProductAsync(product);
        })
            .WithName("AddProductAsync")
            .WithOpenApi();

        // Put is Edit
        iProductRepositoryEndpoints.MapPut("/UpdateProductAsync", (IProductRepository productRepository, ProductEntity product) =>
        {
            return productRepository.UpdateProductAsync(product);
        })
            .WithName("UpdateProductAsync")
            .WithOpenApi();

        iProductRepositoryEndpoints.MapPut("/SaveChangesAsync", (IProductRepository productRepository, object o) =>
        {
            return productRepository.SaveChangesAsync(o);
        })
            .WithName("SaveChangesAsync")
            .WithOpenApi()
            .WithTags("Z", "Depreciated")
            .WithDescription("<h1>This Method Does Not Appear To Be Needed?<h1>");

        iProductRepositoryEndpoints.MapDelete("/DeleteProductAsyncExec/{id}", (IProductRepository productRepository, int id) =>
        {
            return productRepository.DeleteProductAsyncExec(id);
        })
            .WithName("DeleteProductAsync")
            .WithOpenApi();

        
        



        // ================================
        // ProductEntity Endpoints
        var group = routes.MapGroup("/api/ProductEntity").WithTags(nameof(ProductEntity));

        group.MapGet("/", async (StoreContext db) =>
        {
            return await db.Products.ToListAsync();
        })
        .WithName("GetAllProductEntities")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<ProductEntity>, NotFound>> (int id, StoreContext db) =>
        {
            return await db.Products.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is ProductEntity model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetProductEntityById")
        .WithOpenApi();

        // Put is Edit?
        group.MapPut("/{id}", async Task<Results<Ok, NotFound>>(int id, ProductEntity productEntity, StoreContext db) =>
        {
            var affected = await db.Products
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Name, productEntity.Name)
                    .SetProperty(m => m.Category, productEntity.Category)
                    .SetProperty(m => m.Description, productEntity.Description)
                    .SetProperty(m => m.Price, productEntity.Price)
                    .SetProperty(m => m.Quantity, productEntity.Quantity)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateProductEntity")
        .WithOpenApi();

        // Post is Create?
        group.MapPost("/", async (ProductEntity productEntity, StoreContext db) =>
        {
            db.Products.Add(productEntity);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/ProductEntity/{productEntity.Id}", productEntity);
        })
        .WithName("CreateProductEntity")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, StoreContext db) =>
        {
            var affected = await db.Products
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteProductEntity")
        .WithOpenApi();
    }
}
