![Angel Hornet Logo](https://github.com/cjmet/CodeKy_SD09/blob/main/Angel%20Hornet%20Logo.png)
# CodeKY_SD11
### Code Kentucky Software Development Module 11
#### This project is for Educational purposes.
The goal is to create a simple CLI (and later API and Web) application using Entity Framework Core.  The application will have a simple menu system and will allow the user to add, update?, and delete products and orders.  I will be documenting noteable issues and solutions, both good and bad, as we go along.  This is a learning experience.

**See Also:** https://github.com/cjmet/AngelHornetLibrary
<p>&nbsp;</p>

### Code Kentucky
Code Kentucky is a software development bootcamp in Louisville, Kentucky.  The course is designed to teach the fundamentals of software development and to prepare students for a career in the field.  The course is taught by experienced professionals in the field.

https://codekentucky.org/

https://codelouisville.org/

https://code-you.org/
<p>&nbsp;</p>

### Current Questions for Class
  * Change EVERY method to Async does not appear to be supported? nor intended? 
    * UpdateAsync: There is no UpdateAsync method in EF Core. This is a workflow issue.  (FindAsync, Retrieve, Update, SendBack, SaveChangesAsync)  
    * AddAsync: This method is async only to allow special value generators, such as the one used by 'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo', to access the database asynchronously. For all other cases the non async method should be used.*
    * Maybe DbContext.ExecuteUpdateAsync() and DbContext.ExecuteDeleteAsync?
      * This is inconsistent.
      * Add(); SaveChangesAsync();
      * Update(); SaveChangesAsync();
        * _ExecuteUpdateAsync();_ . . .  With completely different syntax and usage
      * SaveChangesAsync(); . . . Doesn't even appear to be needed, _**Yet**_, in the WebAPI project.
      * Remove(); SaveChangesAsync();
        * _ExecuteDeleteAsync();_ . . . With completely different syntax and usage
```
        public async Task UpdateProductAsync(ProductEntity product)
        {
            await _dbContext.Products.Where(item => item.Id == product.Id) 
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(item => item.Id, product.Id)
                    .SetProperty(item => item.Name, product.Name)
                    .SetProperty(item => item.Category, product.Category)
                    .SetProperty(item => item.Description, product.Description)
                    .SetProperty(item => item.Price, product.Price)
                    .SetProperty(item => item.Quantity, product.Quantity)
                    .SetProperty(item => item.Orders, product.Orders)
                );
        }
```
  * Error: response status is 500, Response body, Download System.Text.Json.JsonException: A possible object cycle was detected. 
    * I fixed this temporarily by creating a GetAllProductsWeb that doesn't include linked orders
```
    Error: response status is 500

    Response body
    Download
    System.Text.Json.JsonException: A possible object cycle was detected. This can either
        be due to a cycle or if the object depth is larger than the maximum allowed depth
        of 64. Consider using ReferenceHandler.Preserve on JsonSerializerOptions to 
        support cycles. 
    Path: $.Orders.Products.Orders.Products.Orders.Products.Orders. ... .Products.Orders.Id.
```

```
        // This version does not play nice with default controller and web pages.  Probably a config issue we have not learned yet.
        public IEnumerable<ProductEntity> GetAllProducts() => _dbContext.Products.Include(p => p.Orders).ToList();
        
        // rewritten version for the web controller that does not include linked orders.
        public IEnumerable<ProductEntity> GetAllProductsWeb() => _dbContext.Products.ToList();
```
  * MapGet( ... ).WithName("GetAllProductEntities").WithOpenApi();
    * Need more Info on What is WithName() and how do we use it?
    * Need more info on WithOpenApi() and how do we use it?
  * How do I add a Qty to the OrderProduct Merge Table?  Is this a good idea?
  * Struct OrderLineItem { Id, Product, LinePrice, LineQty, LineCost } ?
  * **Workflow**: Is there a better workflow? 
    * Currently using ClearTracking after Add, Update, Delete operations.  
    * start with a null entity and check productList is null then load?
  * **MenuCLI.cs**: Is there a better strategy or structure for the MenuCli.cs?
      * public void AddItem(List<String> commands, Action actionOnSelect)
 ```
            mainMenu.AddOnEntry(() => { logo(); });
            mainMenu.AddItem("Products", () => { productMenu.Loop(); });
            mainMenu.AddItem("Orders", () => { orderMenu.Loop(); });
            mainMenu.AddItem("Utility", () => { utilityMenu.Loop(); });
            mainMenu.AddItem("Oops!", () =>
            {
                mainMenu.ErrorMsg = "No! No! No! Not THAT button!";
                mainMenu.GetAction(0).Invoke();
            });
            mainMenu.AddItem(["Quit", "Exit"], () => { mainMenu.Exit(); });
            mainMenu.AddDefault(mainMenu.GetEntryAction());
            mainMenu.AddOnExit(() => { Goodbye(); });
 ```
 <p>&nbsp;</p>

### **Requirements**
* SD12 Part 2

### **Known Issues**
* ... 
<p>&nbsp;</p>

### **Hope to Do - Out of Scope**
* Full Concurrency Support for CLI and WebAPI
  * Will need to either DbContext.ChangeTracker.Clear between transactions, or otherwise detatch/reattach the context between transaction the say way ASP.Net runs disconnected. Right now I have to purposefully clear the context to force load the concurrently changed data.  Some of the CLI product logic may also have to be changed as a result.
* Test On-Demand Change Tracking  (This will probably be a requirement for the ASP.Net versions)
  * Set attached data to null. If null load.  If empty it's loaded but empty.  Test change tracking functionality.
* MenuCli.cs
    * TryParse, ExactMatch, and ContainsMatch
* ProductFind and OrderFind 
  * TryParse, ExactMatch, and ContainsMatch
* Clean up Legacy CliLogic.cs.  This will be a major task.  (And will probably never be done.)
<p>&nbsp;</p>

### **Additional Pet Shop Features**
Out of Scope improvements to the Pet Shop project.  
* Improved Menu System
* Additional Menu Options
* Product ContainsMatch
* Products Summary and Details
* Order Summary and Details
* Utilities Menu
<p>&nbsp;</p>

### **General Notes and Blog**
* Remember to set various Objects to Public when editing them.
* SD12 - Completed
  * Added to WebAPI.  I added this separately so that I would not lose CLI functionality.
  * Added Async Product Methods
  * Added IProductRepositoryAsync interface and changed IProductRepository to Inherit it.
    * In hindsight it might have been a better idea to keep them separated. Or to separate them in the future.
  * Added Partial Concurrency Support for running the CLI and WebAPI simultaneously
  * Updated ProductLogic.cs, Created and Passed Async Unit Test.
* Tested using Full Change Tracking and without dbclearcontext. It's working as expected.
* Fixed multiple DbContext Errors and Issues - Thanks Ernesto
* Currently using ClearTracking after Add, Update, Delete operations. Is there a better workflow alternative?
* Use the References tag to double check all references to the DbContext (StoreContext).  Make sure there are no duplicates.
* **use Dependency Injection in both the ProductRepository and OrderRepository**
    * See the more detailed note below
* use MSTest! ... Not NUnit Testing.  
* set **Both** Solution Dependencies and Project Dependencies.  It does not always add these automatically.
* Make sure an item exists before trying to add it to an order.  Double Check this in things like the database seeding.
* static bool IsSeeded has some strange behavior.  Forcing the logic with IF fixed this issue.
<p>&nbsp;</p>

### **More Notes**
#### Multiple DBContexts and Dependency Injection
* **use Dependency Injection in both the ProductRepository and OrderRepository**
```
     public OrderRepository(StoreContext DIContext)
     {
        //_dbContext = new ProductContext();
        _dbContext = DIContext;
     }
```
      * Setup Dependency Injection in startup (program.cs in this case)
```
	{                                                       // Thank you, Ernesto Ramos - Need to add the product context to the service collection.
	return new ServiceCollection()                          // When using Dependency Injection,  
	.AddTransient<IProductRepository, ProductRepository>()  // Product Logic <-> (Product Repository, Order Repository)
	.AddTransient<IOrderRepository, OrderRepository>()      // Product Logic <-> (Product Repository, Order Repository)
	.AddDbContext<StoreContext>()                           // May need to be Scoped or Singleton. But this appears to be working.
	.BuildServiceProvider();                                // .AddDbContext<ProductContext>(ServiceLifetime.Singleton)
	} 
```
* I kept thinking 'we have two contexts competing'.  I didn't know why, or how, or what even defines a context, or different context.  But one context was pulling products while the other pulled orders.  
  * How do we coordinate this?  There has to be a way?  And this probably will still be relevant in the future.
* The answer was, ![Doh!](https://github.com/cjmet/CodeKy_SD09/blob/main/doh.gif) 'WE **DO** HAVE TWO CONTEXTS!!!',   And it should only be **ONE** context, Dependency Injected!
  * "_This error is caused by multiple instances of the DbContext being used.  This can be caused by using the same DbContext in multiple repositories.  The solution is to use Dependency Injection to ensure the same instance of the DbContext is used in both repositories.  This can be done by adding the DbContext to the ServiceCollection in the Startup.cs file.  The DbContext can be added as a Singleton or Scoped service.  The DbContext can then be injected into the repositories.  This will ensure the same instance of the DbContext is used in both repositories._"
* So now lets revert all the effected code to simpler code.  Load everything, and let the change tracker deal with it.  Then see if that works.  Load Everything isn't going to be a good idea at some point, but it should be a simple test.
  * Remove AsNoTracking.  
    * In this use case, we are eager loading and tracking everything as soon as possible.  We will only have to explicitly Add, Update, or Remove specific objects as needed. However, everything will be in-memory and tracked, so that's probably not the best idea.  Revise this workflow later for more efficient use.
  * Remove _dbContext.ChangeTracker.Clear();
  * We have to be tracking BOTH the Container and the Objects we are working with.
```
        // In this use case, we are already tracking the products, so we can just add them to the container.
        order = new OrderEntity();                  // New Container
        orderLogic.AddOrder(order);                 // Add Container to get it tracking.
        product1 = productLogic.                    // Get Item - We are already tracking it.
            GetProductByName("Kitten Chow");
        // If Needed Add Tracking Code Here.        // If Not Tracked, add it to tracking.
        product2 = productLogic.                    // Get Item - We are already tracking it.
            GetProductByName("Kittendines");
        // If Needed Add Tracking Code Here.        // If Not Tracked, add it to tracking.
        order.Products = new List<ProductEntity>    // Add Tracked Items to Container
            { product1, product2 };                 // EF Core automatically tracks all changes.
        orderLogic.SaveChanges(order);              // Save Container and Items
```
  * Separate Add, Update, Delete from SaveChanges.
* Then lets try an intermediate approach.  Set the attached data null so we can test if it's loaded.  Null vs Empty.  If null then load as needed.  If empty it's loaded but empty.  
#### Remove .AddSingleton<ProductContext>()
 * Remove AddSingleton<ProductContext>(), legacy service.  It may have been causing some conflicts.
#### Use AddScoped or AddSingleton for the ProdcutContext
 * Use AddScoped or AddSingleton for the ProdcutContext to ensure the same instance is used for both Interfaces.  Using AddTransient means the IProductRepository and IOrderRepository will be different.  This results in different instances of the ProductContext being used, which means different contexts for products and orders depending on which repository loaded them.
 * You may alternatively want to create and use a combined single interface.  Aka: IProductOrder
 #### Use a Stack() when List.Reverse() fails
 * List.Remove() will remove the first instance of the object in the list.  If you want to remove the last instance, you can use a List.Reverse().  If List.Reverse() fails, you can use Stack() with List.Remove or Stack.Pop() instead.
<p>&nbsp;</p>

###### Ascii Art and Angel Hornet Logo are (c) All Rights Reserved.  Everything else is GPL 3.0.
 