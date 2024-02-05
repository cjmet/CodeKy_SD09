using CodeKY_SD01.Logic;
using DataLibrary;
using Microsoft.Extensions.DependencyInjection;
using AngelHornetLibrary;
using AngelHornetLibrary.CLI;
using System.Net.WebSockets;
using static CodeKY_SD01.CliLogic;
using System.Diagnostics;

namespace CodeKY_SD01
{
    internal class Program
    {

        static IServiceProvider CreateServiceCollection()
        {
            return new ServiceCollection()
                // Use AddScoped or AddSingleton for the ProdcutContext to ensure the same instance is used for both Interfaces.
                // Using AddTransient means the IProductRepository and IOrderRepository will be different.
                // This results in different instances of the ProductContext being used.
                // which means different contexts for products and orders depending on which repository loaded them.
                .AddScoped<ProductContext>()
                .AddScoped<IProductLogic, ProductLogic>()
                .AddScoped<IProductRepository, ProductRepository>()
                .AddScoped<IOrderRepository, ProductRepository>()
                .BuildServiceProvider();
        }

        static void Main(string[] args)
        {

            var services = CreateServiceCollection();
            var productLogic = services.GetService<IProductRepository>();
            var orderLogic = services.GetService<IOrderRepository>();
            Debug.WriteLine($"Database Path: {productLogic.DbPath}");

            if (!productLogic.DataExists())
            {
                productLogic.DebugDatabaseInit();
                for (int i = 5; i > 0; i--)
                {
                    Console.Write($"\rStarting in {i} seconds...");
                    Task.Delay(1000).Wait();
                }
                Console.WriteLine();
            }



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
                { CliSwitch(productLogic, orderLogic, 90); });
            utilityMenu.AddItem("Verbose", () =>
                { CliSwitch(productLogic, orderLogic, 91); });
            utilityMenu.AddItem("SeedDb", () =>
                { CliSwitch(productLogic, orderLogic, 94); });
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
    }
}