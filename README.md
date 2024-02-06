![Angel Hornet Logo](https://github.com/cjmet/CodeKy_SD09/blob/main/Angel%20Hornet%20Logo.png)
# CodeKY_SD10
Code Kentucky Software Development Module 10


### **To Do**
* #### In Scope
  * Part 10b add Unit Testing
* #### Out of Scope
  * Tracking Errors
  * MenuCli.cs
    * TryParse, ExactMatch, and ContainsMatch
  * ProductFind and OrderFind 
    * TryParse, ExactMatch, and ContainsMatch
  * Clean up Legacy CliLogic.cs.  This will be a major task.   
<p>&nbsp;</p>

### **Known Issues**
* Tracking Errors in more complex usage.  
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

 
