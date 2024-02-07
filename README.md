![Angel Hornet Logo](https://github.com/cjmet/CodeKy_SD09/blob/main/Angel%20Hornet%20Logo.png)
# CodeKY_SD10
### Code Kentucky Software Development Module 10
#### This project is for Educational purposes.
The goal is to create a simple CLI (and later API and Web) application using Entity Framework Core.  The application will have a simple menu system and will allow the user to add, update?, and delete products and orders.  I will be documenting noteable issues and solutions, both good and bad, as we go along.  This is a learning experience.

Link: https://github.com/cjmet/CodeKy_SD09.git
<p>&nbsp;</p>

* ### Questions
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

### **To Do**
* * #### In Scope
  * SD11 Part 1

* #### Out of Scope
  * Tracking Errors
    * Add ClearTracking where needed in both Repositories
  * Fix the Utilities Menu
  * Fill out the rest of the menu options
  * MenuCli.cs
    * TryParse, ExactMatch, and ContainsMatch
  * ProductFind and OrderFind 
    * TryParse, ExactMatch, and ContainsMatch
  * Clean up Legacy CliLogic.cs.  This will be a major task.   
<p>&nbsp;</p>

### **Known Issues**
  * Additional Tracking Errors - Add ClearTracking where needed in both Repositories 
<p>&nbsp;</p>

### **Fixed Issues**
  * Tracking Errors in more complex usage.  
    * Ex: 1,6,3.  Ex: 2,6,4; 2,6,1.
<p>&nbsp;</p>

### **Pet Shop Improved**
Out of Scope improvements to the Pet Shop project.  
* Improved Menu System
* Additional Menu Options
* Product ContainsMatch
* Products Summary and Details
* Order Summary and Details
* Utilities Menu
<p>&nbsp;</p>

### **General Notes**
  * Currently using ClearTracking after Add, Update, Delete operations. Is there a better workflow alternative?
  * Use the References tag to double check all references to the DbContext (StoreContext).  Make sure there are no duplicates.
  * use Dependency Injection in both the ProductRepository and OrderRepository
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
  * use MSTest! ... Not NUnit Testing.  
  * set **Both** Solution Dependencies and Project Dependencies.  It does not always add these automatically.
  * Make sure an item exists before trying to add it to an order.  Double Check this in things like the database seeding.
  * static bool IsSeeded has some strange behavior.  Forcing the logic with IF fixed this issue.
<p>&nbsp;</p>

### **More Notes**
#### Remove .AddSingleton<ProductContext>()
 * Remove AddSingleton<ProductContext>(), legacy service.  It may have been causing some conflicts.
#### Use AddScoped or AddSingleton for the ProdcutContext
 * Use AddScoped or AddSingleton for the ProdcutContext to ensure the same instance is used for both Interfaces.  Using AddTransient means the IProductRepository and IOrderRepository will be different.  This results in different instances of the ProductContext being used, which means different contexts for products and orders depending on which repository loaded them.
 * You may alternatively want to create and use a combined single interface.  Aka: IProductOrder
 #### Use a Stack() when List.Reverse() fails
 * List.Remove() will remove the first instance of the object in the list.  If you want to remove the last instance, you can use a List.Reverse().  If List.Reverse() fails, you can use Stack() with List.Remove or Stack.Pop() instead.

 
