using DataLibrary;
using ConsoleToWebAPI.Endpoints;
namespace ConsoleToWebAPI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddDbContext<StoreContext>();
            // Add Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Add Swagger
                app.UseSwagger();
                app.UseSwaggerUI();

            }
            app.UseRouting();
            app.UseEndpoints(endpoints =>
                {
                    //    endpoints.MapGet("/", async context =>
                    //    {
                    //        await context.Response.WriteAsync("Hello From ASP.NET Core Web API");
                    //    });
                    //endpoints.MapGet("/Resource2", async context =>
                    //{
                    //    await context.Response.WriteAsync("Hello From ASP.NET Core Web API - Resource2");
                    //});

                    endpoints.MapControllers();
                    endpoints.MapOrderEntityEndpoints();
                    endpoints.MapProductEntityEndpoints();

                    // Set Default App Starting Page to Swagger
                    //endpoints.MapGet("/swagger", async context =>
                    //{
                    //    context.Response.Redirect("/swagger/index.html");
                    //});

                    // ================================
                    // Set Default App Starting Page to: 
                    //      /Swagger
                    //      /swagger/v1/swagger.json
                    //      /api/OrderEntity
                    //      /api/ProductEntity
                    //      /api/test
                    //      /api/ProductEntities
                    //      /api/IProductRepository/GetAllProducts
                    //      /api/IProductRepository/GetAllProductsWeb
                    //      /api/IProdcutRepository/GetAllProductsWebAsync
                    // 
                    endpoints.MapGet("/", async context =>
                    {
                        context.Response.Redirect("/Swagger");
                    });


                });
        }
    }
}
