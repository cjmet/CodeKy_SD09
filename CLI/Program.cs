using DataLibrary;
using Microsoft.Extensions.DependencyInjection;

using AngelHornetLibrary;
using AngelHornetLibrary.CLI;
using static AngelHornetLibrary.CLI.CliMenu;
using static AngelHornetLibrary.CLI.CliSystem;

using static CodeKY_SD01.CliLogic;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System.Diagnostics.Metrics;



namespace CodeKY_SD01
{
    internal class Program
    {
        static IServiceProvider CreateServiceCollection()
        {
            // When using direct ProductLogic without Dependency Injection, Use AddScoped.
            // Use AddScoped or AddSingleton for the ProdcutContext to ensure the same instance is used for both Interfaces.
            // Using AddTransient means the IProductRepository and IOrderRepository will be different.
            // This results in different instances of the ProductContext being used.
            // which means different contexts for products and orders depending on which repository loaded them.

            //  When using Dependency Injection,  
            return new ServiceCollection()
                //.AddTransient<IProductLogic, ProductOrderRepository>()    // - Single Combined Interface.  This code branch abandoned.
                //.AddTransient<IProductLogic, ProductLogic>()              // - Product Logic. Direct Version?  Legacy Code.  This code branch abandoned.
                .AddTransient<IProductRepository, ProductRepository>()      // - Product Logic <-> (Product Repository, Order Repository)
                .AddTransient<IOrderRepository, OrderRepository>()          // - Product Logic <-> (Product Repository, Order Repository)
                .AddDbContext<StoreContext>()                               // - Thank you Ernesto Ramos - Need to add the product context to the service collection.
                                                                            // - May need to be Scoped or Singleton. 
                                                                            // - .AddDbContext<ProductContext>(ServiceLifetime.Singleton)
                .BuildServiceProvider();
        }


        //public static int IdToUpdate { get; set; } = -1;
        //public static ProductEntity productEntityToUpdate { get; set; } = null;
        static void Main(string[] args)
        {

            var services = CreateServiceCollection();

            //{
            //    var dbContext = new StoreContext();
            //    dbContext.Database.EnsureCreated();
            //    ProductEntity product = new ProductEntity();
            //    product.Name = "Test Product";
            //    product.Category = "Test Category";
            //    product.Description = "Test Description";
            //    product.Price = 1.23m;
            //    product.Quantity = 45;
            //    dbContext.Products.Add(product);
            //    dbContext.SaveChanges();
            //    dbContext.Dispose();
            //    Console.WriteLine("Database Created and Seeded.");
            //}


            var productLogic = services.GetService<IProductRepository>();       // - Product Logic <-> (Product Repository, Order Repository)
            var orderLogic = services.GetService<IOrderRepository>();           // - Product Logic <-> (Product Repository, Order Repository)

            DatabaseInitandTest(productLogic, orderLogic);

            // ###################################################################################################
            // MenuCli System - Work in Progress

            CliMenu mainMenu = new CliMenu();
            CliMenu productMenu = new CliMenu();
            CliMenu orderMenu = new CliMenu();
            CliMenu utilityMenu = new CliMenu();
            CliMenu productUpdate = new CliMenu();
            CliMenu orderUpdate = new CliMenu();

            void logo()
            {
                Console.Clear();
                Console.WriteLine($"\n{AngelHornet.Logo()}\n\n");
                Console.WriteLine("Welcome to our Pet Shop!               SD10");
                Console.WriteLine("------------------------");
            }


            // MAIN MENU
            //mainMenu.AddOnEntry(logo);    // Both these syntaxes work.  But lets use the lambda delegate version for consistency.
            mainMenu.AddOnEntry(() => { logo(); });
            mainMenu.AddItem("Products", () => { productMenu.Loop(); });
            mainMenu.AddItem("Orders", () => { orderMenu.Loop(); });
            mainMenu.AddItem("Utility", () => { utilityMenu.Loop(); });
            mainMenu.AddItem(["Quit", "Exit"], () => { mainMenu.Exit(); });
            mainMenu.AddDefault(mainMenu.GetEntryAction());
            

            // PRODUCT MENU
            //public static int productToUpdate { get; set; } 
            ProductEntity productEntityToUpdate = null;
            productMenu.AddItem("List", () =>
                { CliSwitch(productLogic, orderLogic, 13); });
            productMenu.AddItem("Detail", () =>
                { CliSwitch(productLogic, orderLogic, 12); });
            productMenu.AddItem("InStock", () =>
                { CliSwitch(productLogic, orderLogic, 14); });
            productMenu.AddItem("Add", () =>
                { CliSwitch(productLogic, orderLogic, 11); });
            productMenu.AddItem("Update", () =>
            {
                productEntityToUpdate = SelectProduct(productLogic);
                if (productEntityToUpdate != null)
                    productUpdate.Loop();
                productMenu.GetAction(0).Invoke();
            });
            productMenu.AddItem("Delete", () =>
                { CliSwitch(productLogic, orderLogic, 15); });
            productMenu.AddItem(["Back", "Quit", "Exit"], () => { productMenu.Exit(); });
            productMenu.AddDefault(0);
            productMenu.AddOnEntry(0);
            productMenu.AddOnExit(mainMenu.GetEntryAction());


            // ORDER MENU
            // ==========
            OrderEntity orderEntityToUpdate = null;
            orderMenu.AddItem("List", () =>
                { CliSwitch(productLogic, orderLogic, 23); });
            orderMenu.AddItem("Detail", () =>
                { CliSwitch(productLogic, orderLogic, 24); });
            orderMenu.AddItem("", () =>
            {
                orderMenu.ErrorMsg = "Update is not implemented yet.";
                orderMenu.GetAction(0).Invoke();
            });
            orderMenu.AddItem("Add", () =>
                { CliSwitch(productLogic, orderLogic, 21); });
            orderMenu.AddItem("Update", () =>
            {
                orderEntityToUpdate = SelectOrder(orderLogic);
                if (orderEntityToUpdate != null)
                    orderUpdate.Loop();
                orderMenu.GetAction(0).Invoke();
            });
            orderMenu.AddItem("Delete", () =>
                { CliSwitch(productLogic, orderLogic, 25); });
            orderMenu.AddItem(["Back", "Quit", "Exit"], () => { orderMenu.Exit(); });
            orderMenu.AddDefault(0);
            orderMenu.AddOnEntry(0);
            orderMenu.AddOnExit(mainMenu.GetEntryAction());


            // UTILITY MENU
            // ============
            utilityMenu.AddDefault( () =>
            {
                CliSwitch(productLogic, orderLogic, 90);
            });
            utilityMenu.AddItem("Display", () =>
            {
                Console.Clear();
                PrintDivider(2);
                Console.WriteLine("Displaying Full Database:");
                ProgramInfo(productLogic, orderLogic);
                Console.WriteLine();
                PrintDivider();
                PrintProductList(productLogic,true);
                PrintDivider();
                PrintOrderList(orderLogic,true);
                PrintDivider();
            });
            utilityMenu.AddItem("Verbose", () =>
                { CliSwitch(productLogic, orderLogic, 91); });
            utilityMenu.AddItem("Seed/Reload", () =>
            {
                CliSwitch(productLogic, orderLogic, 94);
            });
            utilityMenu.AddItem("WipeProducts", () =>
                { CliSwitch(productLogic, orderLogic, 92); });
            utilityMenu.AddItem("WipeOrders", () =>
                { CliSwitch(productLogic, orderLogic, 93); });
            utilityMenu.AddItem("WipeDb", () =>
                { CliSwitch(productLogic, orderLogic, 95); });
            utilityMenu.AddItem(["Back", "Quit", "Exit"], () => { utilityMenu.Exit(); });
            utilityMenu.AddOnEntry(utilityMenu.GetDefaultAction());
            utilityMenu.AddOnExit(mainMenu.GetEntryAction());


            // PRODUCT UPDATE MENU
            // ===================
            //Name = name;
            //Category = category;
            //Description = description;
            //Price = price;
            //Quantity = quantity;
            productUpdate.MenuItemWidth = 18;
            productUpdate.Message = "Select the Product Field to Update";
            productUpdate.AddDefault(() =>
            {
                Console.Clear();
                PrintDivider();
                PrintProductItem(productEntityToUpdate);
                Console.WriteLine();
            });
            Action productReturnAction = productUpdate.GetDefaultAction();
            productUpdate.AddOnEntry(productUpdate.GetDefaultAction());
            productUpdate.AddItem("Name", () => {
                var tmp = CliGetString("Enter the New Name: ");
                if (tmp != null && tmp != "") productEntityToUpdate.Name = tmp;
                productReturnAction.Invoke();
            });
            productUpdate.AddItem("Category", () =>
            {
                var tmp = CliGetString("Enter the New Category: ");
                if (tmp != null && tmp != "") productEntityToUpdate.Category = tmp;
                productReturnAction.Invoke();
            });
            productUpdate.AddItem("Description", () =>
            {
                var tmp = CliGetString("Enter the New Description: ");
                if (tmp != null && tmp != "") productEntityToUpdate.Description = tmp;
                productReturnAction.Invoke();
            });
            productUpdate.AddItem("Price", () =>
            {
                decimal tmp;
                if(CliGetDecimal("Enter the New Price: ", out tmp)) 
                    productEntityToUpdate.Price = tmp;
                productReturnAction.Invoke();
            });
            productUpdate.AddItem("Quantity", () =>
            {
                int tmp;
                if (CliGetInt("Enter the New Quantity: ", out tmp));
                    productEntityToUpdate.Quantity = (int)tmp;
                productReturnAction.Invoke();
            });
            //productUpdate.AddItem("Save Changes", () =>
            //{
            //    productLogic.SaveChanges();
            //    productReturnAction.Invoke();
            //});
            productUpdate.AddItem("Save and Exit", () => 
            { 
                productLogic.SaveChanges();
                productUpdate.Exit(); 
            });
            productUpdate.AddOnExit(mainMenu.GetEntryAction());


            // ORDER UPDATE MENU
            // =================
            orderUpdate.MenuItemWidth = 18;
            orderUpdate.Message = "Select the Order Field to Update";
                orderUpdate.AddDefault(() =>
                {
                Console.Clear();
                PrintDivider();
                PrintOrderItem(orderEntityToUpdate,true);
                Console.WriteLine();
            });
            Action orderReturnAction = orderUpdate.GetDefaultAction();
            orderUpdate.AddOnEntry(orderUpdate.GetDefaultAction());
            orderUpdate.AddItem("Add Item", () =>
            {
                Console.Clear();
                PrintDivider();
                PrintProductList(productLogic);
                PrintDivider();
                PrintOrderItem(orderEntityToUpdate, true);

                var item = SelectProduct(productLogic);
                if (item != null)
                {
                    orderEntityToUpdate.Products.Add(item);
                }
                orderReturnAction.Invoke();
            });
            orderUpdate.AddItem("Remove Item", () =>
            {
                ProductEntity item = SelectProduct(productLogic);
                if (item != null)
                {
                    int index = orderEntityToUpdate.Products.LastIndexOf(item);
                    orderEntityToUpdate.Products.RemoveAt(index);
                }
                orderReturnAction.Invoke();
            });
            orderUpdate.AddItem("Save and Exit", () =>
            {
                //orderLogic.UpdateOrder(orderEntityToUpdate);          // cjm 
                //foreach (var item in orderEntityToUpdate.Products)
                //{
                //    productLogic.UpdateProduct(item);
                //}
                orderLogic.SaveChanges(orderEntityToUpdate);
                orderUpdate.Exit();
            });
            orderUpdate.AddOnExit(mainMenu.GetEntryAction());



            mainMenu.Loop();
            Environment.Exit(0);
            // /MenuCli System 
            // ########################################################################
        }


        public static OrderEntity SelectOrder(IOrderRepository orderLogic)
        {
            int id;
            Console.WriteLine("Enter the Order Id");
            string input = Console.ReadLine();
            input = input.Trim().ToLower();

            if (int.TryParse(input, out id))
                return orderLogic.GetOrderById(id);
            else
                return null;
        }


        public static ProductEntity SelectProduct(IProductRepository productLogic)
        {
            int id;
            Console.WriteLine("Enter the Product Id or Name");
            string input = Console.ReadLine();
            input = input.Trim().ToLower();

            if (int.TryParse(input, out id))
                return productLogic.GetProductById(id);
            else
                return productLogic.GetProductByName(input);
        }


        public static void DatabaseInitandTest(IProductRepository? productLogic, IOrderRepository? orderLogic)
        {
            productLogic.ClearChangeTracker();
            // productLogic.ResetDatabase(); // This if for Testing Purposes.     
            ProgramInfo(productLogic, orderLogic);

            if (productLogic.DataExists())
            {
                //StartInSeconds(1);
            }
            else
            {
                Console.WriteLine("Adding Test Products.");
                productLogic.AddProduct(new ProductEntity("Kitten Chow", "Catfood", "A Delicious Bag of Kitten Chow", 9.87m, 65));
                productLogic.AddProduct(new ProductEntity("Kittendines", "Catfood", "A Delicious Bag of Sardines just for Kittens", 8.87m, 55));
                productLogic.AddProduct(new ProductEntity("Void's Vittles for Kittens", "Catfood", "An Empty Bag of Kitten Food", 6.66m, 0));
                productLogic.AddProduct(new ProductEntity("Kitten Kuts", "Catfood", "A Delicious Bag of Choped Steak for Kittens", 19.87m, 5));
                productLogic.AddProduct(new ProductEntity("Bad Boy Bumble Bees", "Catfood", "A Delicious Bag of Dried Bumble Bees.  The Purrfect Snack for your one eyed Pirate Cats", 29.87m, 5));
                productLogic.AddProduct(new ProductEntity("Puppy Chow", "Dogfood", "A Delicious Bag of Puppy Chow", 9.87m, 65));
                productLogic.AddProduct(new ProductEntity("PuppyChovies", "Dogfood", "A Delicious Bag of Anchovies just for Puppies", 8.87m, 55));
                productLogic.SaveChanges();


                Console.WriteLine("Adding Test Orders.");
                DateTime RandomDate()
                {
                    Random rand = new Random();
                    return DateTime.Now - TimeSpan.FromDays(rand.Next(30))
                    - TimeSpan.FromHours(rand.Next(24))
                    - TimeSpan.FromMinutes(rand.Next(60))
                    - TimeSpan.FromSeconds(rand.Next(30));
                }
                Random rand = new Random();
                OrderEntity? order;
                ProductEntity? product1;
                ProductEntity? product2;
                // 1234567 10 234567 20 234567 30 234567 40 234567 50 234567 60 234567 70 234567 80 //
                // In this use case, we are already tracking the products, so we can just add them to the container.
                order = new OrderEntity();                  // New Container
                orderLogic.AddOrder(order);                 // Add Container to Tracking
                order.OrderDate = RandomDate();             // Set Date
                product1 = productLogic.                    // Get Item - We are already tracking it.
                    GetProductByName("Kitten Chow");
                // If Needed Add Tracking Code Here.        // If Not Tracked, add it to tracking.
                product2 = productLogic.                    // Get Item - We are already tracking it.
                    GetProductByName("Kittendines");
                // If Needed Add Tracking Code Here.        // If Not Tracked, add it to tracking.
                order.Products = new List<ProductEntity>    // Add Tracked Items to Container
                    { product1, product2 };                 // EF Core automatically tracks all changes.
                orderLogic.SaveChanges(order);              // Save Container and Items

                // Order 2
                order = new OrderEntity();
                order.OrderDate = RandomDate();
                orderLogic.AddOrder(order);
                product1 = productLogic.GetProductByName("Puppy");
                product2 = productLogic.GetProductByName("Kuts");
                order.Products = new List<ProductEntity> { product1, product2 };
                orderLogic.SaveChanges(order);

                // Order 3
                order = new OrderEntity();
                orderLogic.AddOrder(order);
                order.OrderDate = RandomDate();
                product1 = productLogic.GetProductByName("Void");
                product2 = productLogic.GetProductByName("Kuts");
                order.Products = new List<ProductEntity> { product1, product2 };
                orderLogic.SaveChanges(order);

                // Order 4
                order = new OrderEntity();
                orderLogic.AddOrder(order);
                order.OrderDate = RandomDate();
                product1 = productLogic.GetProductByName("Bees");
                product2 = productLogic.GetProductByName("Puppy");
                order.Products = new List<ProductEntity> { product1, product2 };
                orderLogic.SaveChanges(order);

                PrintDivider();
                PrintProductList(productLogic, true);
                PrintDivider();
                PrintOrderList(orderLogic, true);
                PrintDivider();

                CliSleep(3, "Starting in ", CliSleepDisplay.Counter);
                //Console.WriteLine("Press <Enter> to continue:"); Console.ReadLine();
            }
        }
    }
}