using Microsoft.EntityFrameworkCore;
using DataLibrary;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ConsoleToWebAPI.Endpoints;

public static class OrderEntityEndpoints
{
    public static void MapOrderEntityEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/OrderEntity").WithTags(nameof(OrderEntity));

        group.MapGet("/", async (StoreContext db) =>
        {
            return await db.Orders.ToListAsync();
        })
        .WithName("GetAllOrderEntities")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<OrderEntity>, NotFound>> (int id, StoreContext db) =>
        {
            return await db.Orders.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is OrderEntity model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetOrderEntityById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, OrderEntity orderEntity, StoreContext db) =>
        {
            var affected = await db.Orders
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, orderEntity.Id)
                    .SetProperty(m => m.OrderDate, orderEntity.OrderDate)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateOrderEntity")
        .WithOpenApi();

        group.MapPost("/", async (OrderEntity orderEntity, StoreContext db) =>
        {
            db.Orders.Add(orderEntity);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/OrderEntity/{orderEntity.Id}", orderEntity);
        })
        .WithName("CreateOrderEntity")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, StoreContext db) =>
        {
            var affected = await db.Orders
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteOrderEntity")
        .WithOpenApi();
    }
}
