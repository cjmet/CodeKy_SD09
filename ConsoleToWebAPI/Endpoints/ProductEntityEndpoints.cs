using Microsoft.EntityFrameworkCore;
using DataLibrary;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ConsoleToWebAPI.Endpoints;

public static class ProductEntityEndpoints
{
    public static void MapProductEntityEndpoints (this IEndpointRouteBuilder routes)
    {
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

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, ProductEntity productEntity, StoreContext db) =>
        {
            var affected = await db.Products
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, productEntity.Id)
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

        group.MapPost("/", async (ProductEntity productEntity, StoreContext db) =>
        {
            db.Products.Add(productEntity);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/ProductEntity/{productEntity.Id}",productEntity);
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
