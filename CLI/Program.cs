using CodeKY_SD01.Logic;
using DataLibrary;
using Microsoft.Extensions.DependencyInjection;
using AngelHornetLibrary;
using AngelHornetLibrary.CLI;
using static CodeKY_SD01.CliLogic;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

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

        static void Main(string[] args)
        {

            var services = CreateServiceCollection();
            //var productLogic = services.GetService<IProductLogic>();          // cjm - Single Combined Interface.  This code branch abandoned.
            var productLogic = services.GetService<IProductRepository>();       // - Product Logic <-> (Product Repository, Order Repository)
            var orderLogic = services.GetService<IOrderRepository>();           // - Product Logic <-> (Product Repository, Order Repository)

            DatabaseInitandTest(productLogic, orderLogic);

            // ###################################################################################################
            // MenuCli System - Work in Progress

            MenuCli mainMenu = new MenuCli();
            MenuCli productMenu = new MenuCli();
            MenuCli orderMenu = new MenuCli();
            MenuCli utilityMenu = new MenuCli();

            void logo()
            {
                Console.Clear();
                Console.WriteLine($"\n{AngelHornet.Logo()}\n\n");
                Console.WriteLine("Welcome to our Pet Shop!");
                Console.WriteLine("------------------------");
            }


            //mainMenu.AddOnEntry(logo);    // Both these syntaxes work.  But lets use the lambda delegate version for consistency.
            mainMenu.AddOnEntry(() => { logo(); });
            mainMenu.AddItem("Products", () => { productMenu.Loop(); });
            mainMenu.AddItem("Orders", () => { orderMenu.Loop(); });
            mainMenu.AddItem("Utility", () => { utilityMenu.Loop(); });
            mainMenu.AddItem(["Quit", "Exit"], () => { mainMenu.Exit(); });
            mainMenu.AddDefault(mainMenu.GetEntryAction());


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
                productMenu.ErrorMsg = "Update is not implemented yet.";
                productMenu.GetAction(0).Invoke();
            });
            productMenu.AddItem("Delete", () =>
                { CliSwitch(productLogic, orderLogic, 15); });
            productMenu.AddItem(["Back", "Quit", "Exit"], () => { productMenu.Exit(); });
            productMenu.AddDefault(0);
            productMenu.AddOnEntry(0);
            productMenu.AddOnExit(mainMenu.GetEntryAction());


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
                orderMenu.ErrorMsg = "Update is not implemented yet.";
                orderMenu.GetAction(0).Invoke();
            });
            orderMenu.AddItem("Delete", () =>
                { CliSwitch(productLogic, orderLogic, 25); });
            orderMenu.AddItem(["Back", "Quit", "Exit"], () => { orderMenu.Exit(); });
            orderMenu.AddDefault(0);
            orderMenu.AddOnEntry(0);
            orderMenu.AddOnExit(mainMenu.GetEntryAction());


            utilityMenu.AddItem("Display", () =>
            {
                CliSwitch(productLogic, orderLogic, 90); 
            });
            utilityMenu.AddItem("Verbose", () =>
                { CliSwitch(productLogic, orderLogic, 91); });
            utilityMenu.AddItem("SeedDb", () =>
            {
                CliSwitch(productLogic, orderLogic, 94); utilityMenu.ErrorMsg = "SeedDB will need to be Re-Implemented Differently.";
                utilityMenu.GetAction(0).Invoke();
            });
            utilityMenu.AddItem("WipeProducts", () =>
                { CliSwitch(productLogic, orderLogic, 92); });
            utilityMenu.AddItem("WipeOrders", () =>
                { CliSwitch(productLogic, orderLogic, 93); });
            utilityMenu.AddItem("WipeDb", () =>
                { CliSwitch(productLogic, orderLogic, 95); });
            utilityMenu.AddItem(["Back", "Quit", "Exit"], () => { utilityMenu.Exit(); });
            utilityMenu.AddDefault(0);
            utilityMenu.AddOnEntry(0);
            utilityMenu.AddOnExit(mainMenu.GetEntryAction());

            mainMenu.Loop();
            Environment.Exit(0);
            // /MenuCli System 
            // ########################################################################
        }



        private static void DatabaseInitandTest(IProductRepository? productLogic, IOrderRepository? orderLogic)
        {
            //productLogic.ResetDatabase(); // This if for Testing Purposes.     // cjm 
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

                Console.WriteLine("Adding Test Orders.");
                var product1 = productLogic.GetProductByName("Puppy");
                var product2 = productLogic.GetProductByName("Kuts");
                if (product1 != null && product2 != null)
                {
                    OrderEntity order = new OrderEntity();
                    order.OrderDate = DateTime.Now;
                    if (order.Products == null) order.Products = new List<ProductEntity>();
                    order.Products.Add(product1);
                    order.Products.Add(product2);
                    orderLogic.AddOrder(order);
                }

                product1 = productLogic.GetProductByName("Kitten Chow");
                product2 = productLogic.GetProductByName("Kittendines");
                if (product1 != null && product2 != null)
                    orderLogic.AddOrder(new OrderEntity() { OrderDate = DateTime.Now, Products = { product1, product2 } });

                product1 = productLogic.GetProductByName("Void");
                product2 = productLogic.GetProductByName("Kuts");
                if (product1 != null && product2 != null)
                    orderLogic.AddOrder(new OrderEntity() { OrderDate = DateTime.Now, Products = { product1, product2 } });

                product1 = productLogic.GetProductByName("Bees");
                product2 = productLogic.GetProductByName("Puppy");
                if (product1 != null && product2 != null)
                    orderLogic.AddOrder(new OrderEntity() { OrderDate = DateTime.Now, Products = { product1, product2 } });

                PrintDivider();
                PrintProductList(productLogic, true);
                PrintDivider();
                PrintOrderList(orderLogic, true);
                PrintDivider();

                StartInSeconds(5);
            }
        }

        static void StartInSeconds (int seconds)
        {
            for (int i = seconds; i > 0; i--)
            {
                Console.Write($"\rStarting in {i} seconds...");
                Task.Delay(1000).Wait();
            }
            Console.WriteLine();
        }
    }
}