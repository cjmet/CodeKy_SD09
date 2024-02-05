logo:
![Angel Hornet Logo]("https://github.com/cjmet/CodeKy_SD09/blob/main/Angel%20Hornet%20Logo.png")
# CodeKY_SD10
Code Kentucky Software Development Module 10

## To Do
* MenuCli.cs
  * TryParse, ExactMatch, and ContainsMatch
* Convert to IProductOrderRepository combined single interface and test
* Part 10b add Unit Testing

## Known Issues
* ... 

## Pet Shop Improved
* Improved Menu System
* Additional Menu Options
* *

## Notes
#### Use AddScoped or AddSingleton for the ProdcutContext
 * Use AddScoped or AddSingleton for the ProdcutContext to ensure the same instance is used for both Interfaces.  Using AddTransient means the IProductRepository and IOrderRepository will be different.  This results in different instances of the ProductContext being used, which means different contexts for products and orders depending on which repository loaded them.
 * You may alternatively want to create and use a combined single interface.  Aka: IProductOrder
 #### Use a Stack() when List.Reverse() fails
 * List.Remove() will remove the first instance of the object in the list.  If you want to remove the last instance, you can use a List.Reverse().  If List.Reverse() fails, you can use Stack() with List.Remove or Stack.Pop() instead.
 #### General Notes
 * set **Both** Solution Dependencies and Project Dependencies.  It does not always add these automatically.
 * Make sure an item exists before trying to add it to an order.  Double Check this in things like the database seeding.
 * static bool IsSeeded has some strange behavior.  Forcing the logic with IF fixed this issue.
 

 