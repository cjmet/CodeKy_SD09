using CodeKY_SD01.Logic;
using DataLibrary;
using Microsoft.Extensions.DependencyInjection;
using AngelHornetLibrary;
using AngelHornetLibrary.CLI;
using System.Net.WebSockets;
using static CodeKY_SD01.CliLogic;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;

namespace CodeKY_SD01
{
    internal class Program
    {

        static IServiceProvider CreateServiceCollection()
        {
            // Use AddScoped or AddSingleton for the ProdcutContext to ensure the same instance is used for both Interfaces.
            // Using AddTransient means the IProductRepository and IOrderRepository will be different.
            // This results in different instances of the ProductContext being used.
            // which means different contexts for products and orders depending on which repository loaded them.
            return new ServiceCollection()
                .AddTransient<IProductLogic, ProductOrderRepository>()
                //.AddTransient<IProductRepository, ProductRepository>()
                //.AddTransient<IOrderRepository, OrderRepository>()
                .BuildServiceProvider();
        }

        static void Main(string[] args)
        {

            var services = CreateServiceCollection();
            //var productLogic = services.GetService<IProductRepository>();
            //var orderLogic = services.GetService<IOrderRepository>();
            var productLogic = services.GetService<IProductLogic>();
            Debug.WriteLine($"Database Path: {productLogic.DbPath}");

            productLogic.ResetDatabase(); // This was for Testing Purposes.  It's not needed now.
            if (productLogic.DataExists())
            {
                Console.WriteLine("Order Repository Already Contains Data.");
                Console.WriteLine($"Products: {productLogic.GetAllProducts().Count()}     Orders: {productLogic.GetAllOrders().Count()}");
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
                    productLogic.AddOrder(order);
                }

                product1 = productLogic.GetProductByName("Kitten Chow");
                product2 = productLogic.GetProductByName("Kittendines");
                if (product1 != null && product2 != null)
                    productLogic.AddOrder(new OrderEntity() { OrderDate = DateTime.Now, Products = { product1, product2 } });

                product1 = productLogic.GetProductByName("Void");
                product2 = productLogic.GetProductByName("Kuts");
                if (product1 != null && product2 != null)
                    productLogic.AddOrder(new OrderEntity() { OrderDate = DateTime.Now, Products = { product1, product2 } });

                product1 = productLogic.GetProductByName("Bees");
                product2 = productLogic.GetProductByName("Puppy");
                if (product1 != null && product2 != null)
                    productLogic.AddOrder(new OrderEntity() { OrderDate = DateTime.Now, Products = { product1, product2 } });
            }
             
            PrintDivider();
            PrintProductList(productLogic,true);
            PrintDivider();
            PrintOrderList(productLogic, true);
            PrintDivider();

            //for (int i = 5; i > 0; i--)
            //{
            //    Console.Write($"\rStarting in {i} seconds...");
            //    Task.Delay(1000).Wait();
            //}
            //Console.WriteLine();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();



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


            //mainMenu.AddOnEntry(logo);                  // Both these syntaxes work.  But lets use the delegate version for consistency.
            mainMenu.AddOnEntry(() =>
            {
                logo();
            });   // Both these syntaxes work.
            mainMenu.AddItem("Products", () => { productMenu.Loop(); });
            mainMenu.AddItem("Orders", () => { orderMenu.Loop(); });
            mainMenu.AddItem("Utility", () => { utilityMenu.Loop(); });
            mainMenu.AddItem(["Quit", "Exit"], () => { mainMenu.Exit(); });
            mainMenu.AddDefault(mainMenu.GetEntryAction());


            productMenu.AddItem("List", () =>
                { CliSwitch(productLogic, productLogic, 13); });
            productMenu.AddItem("Detail", () =>
                { CliSwitch(productLogic, productLogic, 12); });
            productMenu.AddItem("InStock", () =>
                { CliSwitch(productLogic, productLogic, 14); });
            productMenu.AddItem("Add", () =>
                { CliSwitch(productLogic, productLogic, 11); });
            productMenu.AddItem("Update", () =>
                {
                    productMenu.ErrorMsg = "Update is not implemented yet.";
                    productMenu.GetAction(0).Invoke();
                });
            productMenu.AddItem("Delete", () =>
                { CliSwitch(productLogic, productLogic, 15); });
            productMenu.AddItem(["Back", "Quit", "Exit"], () => { productMenu.Exit(); });
            productMenu.AddDefault(0);
            productMenu.AddOnEntry(0);
            productMenu.AddOnExit(mainMenu.GetEntryAction());


            orderMenu.AddItem("List", () =>
               { CliSwitch(productLogic, productLogic, 23); });
            orderMenu.AddItem("Detail", () =>
               { CliSwitch(productLogic, productLogic, 24); });
            orderMenu.AddItem("", () =>
            {
                orderMenu.ErrorMsg = "Update is not implemented yet.";
                orderMenu.GetAction(0).Invoke();
            });
            orderMenu.AddItem("Add", () =>
               { CliSwitch(productLogic, productLogic, 21); });
            orderMenu.AddItem("Update", () =>
                {
                    orderMenu.ErrorMsg = "Update is not implemented yet.";
                    orderMenu.GetAction(0).Invoke();
                });
            orderMenu.AddItem("Delete", () =>
               { CliSwitch(productLogic, productLogic, 25); });
            orderMenu.AddItem(["Back", "Quit", "Exit"], () => { orderMenu.Exit(); });
            orderMenu.AddDefault(0);
            orderMenu.AddOnEntry(0);
            orderMenu.AddOnExit(mainMenu.GetEntryAction());


            utilityMenu.AddItem("Display", () =>
                { CliSwitch(productLogic, productLogic, 90); });
            utilityMenu.AddItem("Verbose", () =>
                { CliSwitch(productLogic, productLogic, 91); });
            utilityMenu.AddItem("SeedDb", () =>
                {
                    CliSwitch(productLogic, productLogic, 94); utilityMenu.ErrorMsg = "SeedDB will need to be Re-Implemented Differently.";
                    utilityMenu.GetAction(0).Invoke();
                });
            utilityMenu.AddItem("WipeProducts", () =>
                { CliSwitch(productLogic, productLogic, 92); });
            utilityMenu.AddItem("WipeOrders", () =>
                { CliSwitch(productLogic, productLogic, 93); });
            utilityMenu.AddItem("WipeDb", () =>
                { CliSwitch(productLogic, productLogic, 95); });
            utilityMenu.AddItem(["Back", "Quit", "Exit"], () => { utilityMenu.Exit(); });
            utilityMenu.AddDefault(0);
            utilityMenu.AddOnEntry(0);
            utilityMenu.AddOnExit(mainMenu.GetEntryAction());

            mainMenu.Loop();
            Environment.Exit(0);
            // /MenuCli System 
            // ########################################################################
        }
    }
}