using CodeKY_SD01.Logic;
using DataLibrary;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;




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
                .AddTransient<IProductRepository, ProductLogic>()
                .AddTransient<IOrderRepository, ProductLogic>()
                .BuildServiceProvider();
        }

        static void Main(string[] args)
        {

            var services = CreateServiceCollection();
            var productLogic = services.GetService<IProductRepository>();
            var orderLogic = services.GetService<IOrderRepository>();
            Console.WriteLine($"Database Path: {productLogic.DbPath}");
            Console.WriteLine($"Contains Data: {productLogic.Seeded}");
            Console.WriteLine("\n\n");

            string userInput;
            Console.WriteLine("Welcome to our Pet Shop!");
            Console.WriteLine("------------------------");

            bool _exitProgram = false;
            string _lastInput = "false";

            MenuAddRow("-----------------");
            MenuAddRow(0, "Product Commands:");
            MenuAddRow(0, "11", "Add", "add a product.");
            MenuAddRow(0, "12", "Detail", "detail search product(s)");
            MenuAddRow(0, "13", "List", "list all products.");
            MenuAddRow(0, "14", "InStock", "list in-stock products.");
            MenuAddRow(0, "15", "Delete", "delete product by id.");

            MenuAddRow(1, "Orders Commands:");
            MenuAddRow(1, "21", "Add", "add an order.");
            MenuAddRow(1, "22", "Find", "find order by id.");
            MenuAddRow(1, "23", "List", "list all orders.");
            MenuAddRow(1, "24", "Detail", "detail list of all orders.");
            MenuAddRow(1, "25", "Delete", "delete order by id.");

            MenuAddRow();

            MenuAddRow("Test Commands:");
            MenuAddRow(0, "90", "Verbose", "Toggle VerboseSQL Mode.");
            MenuAddRow(1, "");
            MenuAddRow(0, "92", "Wipe", "delete all Products.");
            MenuAddRow(1, "93", "Wipe", "delete all Orders.");
            MenuAddRow(0, "94", "SeedDb", "Seed and/or List All Data.");
            MenuAddRow(1, "95", "WipeDb", "Wipe Databases.");

            MenuAddRow();

            MenuAddRow("Type 'exit' to quit.");

            do
            {
                //Console.WriteLine();
                //               ("1234567890123456789012345678901234567..|..34567890123456789012345678901234567");
                //               ("-----------------------------------------------------------------------------");

                //Console.WriteLine("Product Commands:                         Orders Commands:");
                //Console.Write($"{"11: Add      add a product.",-42}");
                //Console.Write($"{"21: Add      add an order."}\n");

                //Console.Write($"{"12: Find     find product by name or id.",-42}");
                //Console.Write($"{"22: Find     find order by id."}\n");

                //Console.Write($"{"13: List     list all products.",-42}");
                //Console.Write($"{"23: List     list all orders."}\n");

                //Console.Write($"{"14: InStock  list in-stock products.",-42}");
                //Console.Write($"{"24:          unimplemented."}\n");

                //Console.Write($"{"15: Delete   delete product by id.",-42}");
                //Console.Write($"{"25: Delete   delete order by id."}\n");

                //Console.WriteLine();

                //Console.WriteLine($"{"Test Commands:",-42}");
                //Console.Write($"{"90: Verbose  Toggle VerboseSQL Mode.",-42}");
                //Console.Write($"{"91:          Unimplemented."}\n");
                //Console.Write($"{"92: SeedDb   Add Test Data.",-42}");
                //Console.Write($"{"93: WipeDb   Wipe Databases."}\n");


                //Console.WriteLine();
                //Console.WriteLine("Type 'exit' to quit.");

                Console.WriteLine();
                MenuDisplay();

                userInput = Console.ReadLine();
                userInput = userInput.Trim();
                userInput = userInput.ToLower();

                Console.Clear();
                //Console.WriteLine($"User Input: {userInput}");

                switch (userInput)
                {
                    case "11":
                        Console.WriteLine("Adding a new product.");
                        {
                            ProductEntity product = new ProductEntity();
                            Console.WriteLine("Enter the Product Name:");
                            product.Name = Console.ReadLine();
                            Console.WriteLine("Enter the Product Category:");
                            product.Category = Console.ReadLine();
                            Console.WriteLine("Enter the Product Description:");
                            product.Description = Console.ReadLine();
                            Console.WriteLine("Enter the Product Price:");
                            product.Price = decimal.TryParse(Console.ReadLine(), out decimal price) ? price : 0;
                            Console.WriteLine("Enter the Product Quantity:");
                            product.Quantity = int.TryParse(Console.ReadLine(), out int quantity) ? quantity : 0;
                            Console.WriteLine();

                            // Results
                            Console.Clear();
                            productLogic.AddProduct(product);
                            if (product.Id > 0)
                                Console.WriteLine("Product Added.");
                            else
                                Console.WriteLine("Product Not Added.");
                            PrintDivider();
                            PrintProductList(productLogic);
                            //Console.WriteLine();
                        }
                        break;
                    case "12":
                        {
                            Console.Clear();
                            PrintDivider();
                            PrintProductList(productLogic);
                            Console.WriteLine();

                            Console.WriteLine("Enter the product id, name, or keyword, or <enter> for all products:");
                            string userInput2 = Console.ReadLine();
                            Console.WriteLine();
                            userInput2 = userInput2.Trim();

                            List<ProductEntity> products = new List<ProductEntity>();
                            int productId = 0;
                            ProductEntity product = null;
                            if (int.TryParse(userInput2, out productId))
                                product = productLogic.GetProductById(productId);
                            else
                                products =
                                    productLogic.GetAllProductsByName(userInput2).ToList();
                            if (product != null) products.Add(product);

                            //Results
                            Console.Clear();
                            Console.WriteLine($"Searching for \"{userInput2}\":");
                            PrintDivider();
                            if (products != null && products.Count > 0)
                            {
                                foreach (var item in products)
                                {
                                    if (item != null && item.Id != null)
                                        productLogic.GetProductById(item.Id); // force load of orders
                                    PrintProductItem(item);
                                    PrintProductDetails(item);
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Product '{userInput2}' was not found.");
                            }
                            break;
                        }
                    case "13":
                        {
                            Console.Clear();
                            Console.WriteLine("The Following is a list of all Item:");
                            PrintDivider();
                            PrintProductList(productLogic);
                            break;
                        }
                    case "14":
                        {
                            Console.Clear();
                            var list = productLogic.GetOnlyInStockProducts().ToList();
                            Console.WriteLine("The Following is a list of all In-Stock Items.");
                            PrintDivider();
                            if (list != null && list.Count > 0)
                            {
                                foreach (var item in list)
                                {
                                    PrintProductItem(item);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Item List is Empty");
                            }
                            break;
                        }

                    case "15":
                        {
                            PrintDivider();
                            PrintProductList(productLogic);
                            Console.WriteLine("\nEnter the product id you wish to delete.");
                            string userInput2 = Console.ReadLine();
                            userInput2 = userInput2.Trim();
                            Console.Clear();
                            if (int.TryParse(userInput2, out int id) && productLogic.GetProductById(id) != null)
                            {
                                productLogic.DeleteProduct(id);
                                Console.WriteLine($"Product with id {id} has been deleted.");
                            }
                            else
                            {
                                Console.WriteLine($"Product with id [{userInput2}] was not found.");
                            }
                            PrintDivider();
                            PrintProductList(productLogic);
                            break;
                        }
                        case "92":
                        {
                            var products = productLogic.GetAllProducts().ToList();
                            foreach (var item in products)
                            {
                                productLogic.DeleteProduct(item.Id);
                            }
                            PrintDivider();
                            PrintProductList(productLogic);
                            PrintDivider();
                            PrintOrderList(orderLogic);
                            PrintDivider();
                            break;
                        }

                    // ================================================================

                    case "21":
                        Console.Clear();
                        // ***************************************
                        Console.WriteLine("Adding a new order.\n");
                        {
                            OrderEntity order = new OrderEntity();
                            order.OrderDate = DateTime.Now;
                            order.Products = new List<ProductEntity>();

                            var productList = productLogic.GetAllProducts().ToList();
                            ProductEntity product = null;
                            string userInput2;

                            do
                            {
                                // +++++++++++++
                                PrintDivider();
                                PrintProductList(productLogic);
                                PrintDivider();
                                PrintOrderItem(order,true);
                                PrintDivider();
                                // +++++++++++++

                                Console.WriteLine("Input ProductId or Keyword to add to the order, ");
                                Console.WriteLine("'Undo' to remove the last item, or <Enter> to Send the Order:");
                                userInput2 = Console.ReadLine();
                                userInput2 = userInput2.Trim();


                                product = null;
                                int productId = 0;
                                if (userInput2.ToLower() == "undo" || userInput2.ToLower() == "u")
                                {
                                    var lastProduct = order.Products.LastOrDefault();
                                    // Simple version removes the first occurrence of the last product in the list.
                                    // if (lastProduct != null) order.Products.Remove(lastProduct);
                                    // Fixed Below
                                    if (lastProduct != null)
                                    {
                                        //  ICollection<ProductEntity> tmp;   // cjm
                                        // .Reverse and/or .ToList do not work here. // Several Errors with other methods. 
                                        // and many of those Errors fail to build but don't show up in the Error List.
                                        Stack<ProductEntity> stack = new Stack<ProductEntity>(order.Products);
                                        order.Products = stack.ToList();
                                        order.Products.Remove(lastProduct);
                                        stack = new Stack<ProductEntity>(order.Products);
                                        order.Products = stack.ToList();
                                    }
                                }
                                else if (userInput2 == "") product = null;
                                else if (int.TryParse(userInput2, out productId)) product = productLogic.GetProductById(productId);
                                else product = productLogic.GetProductByName(userInput2);


                                Console.Clear();
                                if (product != null)
                                {
                                    if (order.Products == null) order.Products = new List<ProductEntity>();
                                    order.Products.Add(product);
                                }
                                else
                                {
                                    if (userInput2 != "") Console.WriteLine($"Product '{userInput2}' was not found.");
                                }
                                userInput2 = userInput2.ToLower();
                            } while (userInput2 != "");


                            // Save the order
                            if (order.Products == null) order.Products = new List<ProductEntity>();
                            foreach (var item in order.Products)
                            {
                                if (item.Orders == null) item.Orders = new List<OrderEntity>();
                                item.Orders.Add(order);
                            }
                            if (order.Products.Count > 0)
                            {
                                orderLogic.AddOrder(order);
                            }
                            if (order.Id > 0)
                                Console.WriteLine("Order Added.");
                            else
                                Console.WriteLine("Order Not Added.");
                            PrintDivider();
                            PrintOrderList(orderLogic);
                        }

                        break;

                    case "22":  // find order by id
                        {
                            Console.Clear();
                            PrintDivider();
                            PrintOrderList(orderLogic);
                            Console.WriteLine("\nEnter the order id you wish to view.");
                            string userInput2 = Console.ReadLine();
                            Console.WriteLine();
                            userInput2 = userInput2.Trim();
                            if (int.TryParse(userInput2, out int id) && orderLogic.GetOrderById(id) != null)
                            {
                                var order = orderLogic.GetOrderById(id);
                                PrintDivider();
                                Console.WriteLine($"Order {id}   Date: {order.OrderDate}");
                                foreach (var item in order.Products)
                                {
                                    PrintProductItem(item);
                                }
                            }
                            else
                            {
                                PrintDivider();
                                Console.WriteLine($"Order with id [{userInput2}] was not found.");
                            }
                            break;
                        }
                    case "23":  // list all orders
                        {
                            Console.Clear();
                            Console.WriteLine("The Following is a list of all Orders:\n");
                            PrintDivider();
                            PrintOrderList(orderLogic);
                            break;
                        }
                    case "24":  // unimplemented
                        {
                            Console.Clear();
                            Console.WriteLine("The Following is a list of all Orders:\n");
                            PrintDivider();
                            PrintOrderList(orderLogic, true);
                            break;
                        }
                    case "25":  // delete order by id
                        {
                            Console.Clear();
                            PrintDivider();
                            PrintOrderList(orderLogic);
                            Console.WriteLine("\nEnter the order id you wish to delete.");
                            string userInput2 = Console.ReadLine();
                            userInput2 = userInput2.Trim();
                            Console.Clear();
                            if (int.TryParse(userInput2, out int id) && orderLogic.GetOrderById(id) != null)
                            {
                                orderLogic.DeleteOrder(id);
                                Console.WriteLine($"Order with id {id} has been deleted.");
                            }
                            else
                            {
                                Console.WriteLine($"Order with id [{userInput2}] was not found.");
                            }
                            PrintDivider();
                            PrintOrderList(orderLogic);
                            break;
                        }
                        case "93":  // delete all orders
                        {
                            var orders = orderLogic.GetAllOrders().ToList();
                            foreach (var item in orders)
                            {
                                orderLogic.DeleteOrder(item.Id);
                            }
                            PrintDivider();
                            PrintProductList(productLogic);
                            PrintDivider();
                            PrintOrderList(orderLogic);
                            PrintDivider();
                            break;
                        }

                    // ================================================================

                    case "90":
                        productLogic.VerboseSQL = !productLogic.VerboseSQL;
                        Console.WriteLine($"VerboseSQL is now {productLogic.VerboseSQL}");
                        PrintDivider();
                        break;
                    case "91":  // unimplemented
                        {
                            Console.Clear();
                            Console.WriteLine("Unimplemented");
                            PrintDivider();
                            break;
                        }
                    case "94":
                        {
                            Console.Clear();
                            productLogic.DebugDatabaseInit();
                            PrintDivider();
                            PrintProductList(productLogic);
                            PrintDivider();
                            PrintOrderList(orderLogic);
                            PrintDivider();
                            break;
                        }
                    case "95":
                        {
                            var products = productLogic.GetAllProducts().ToList();
                            foreach (var item in products)
                            {
                                productLogic.DeleteProduct(item.Id);
                            }
                            var orders = orderLogic.GetAllOrders().ToList();
                            foreach (var item in orders)
                            {
                                orderLogic.DeleteOrder(item.Id);
                            }
                            break;
                        }

                    case "exit": Console.WriteLine("exit"); _exitProgram = true; break;
                    case "quit": Console.WriteLine("quit"); _exitProgram = true; break;
                    case "":
                        {
                            Console.WriteLine("<empty>\n");
                            PrintDivider();
                            if (_lastInput == "") _exitProgram = true;
                            else Console.WriteLine("Press <Enter> Again To Exit.");
                            break;
                        }
                    default: Console.WriteLine($"I do not recognize '{userInput}' as a valid input."); break;
                }
                _lastInput = userInput;
            } while (!_exitProgram);
        }

        public static void PrintProductItem(ProductEntity item)
        {
            Console.WriteLine($"{item.Id,3}: {item.Name,-30} - {item.Category,15} - Qty: {item.Quantity,2} - {item.Price:C}");
        }

        static void PrintDivider()
        {
            //               ("-----------------------------------------------------------------------------");
            Console.WriteLine("=============================================================================");
            Console.WriteLine();
        }


        static void PrintProductDetails(ProductEntity item)
        {
            if (item == null) return;

            string tmp = item.Description;
            if (tmp.Length > 80) tmp = tmp.Substring(0, 80);
            Console.WriteLine($"     \"{tmp}\"");

            if (item.Orders != null && item.Orders.Count > 0)
            {
                Console.Write($"     Orders: ");
                bool flag = false;
                foreach (var order in item.Orders)
                {
                    if (flag) Console.Write(", ");
                    Console.Write($"[{order.Id,3}]");
                    flag = true;
                }
            }
            Console.WriteLine();
            Console.WriteLine();
        }



        static void PrintProductList(IProductRepository? productLogic, bool printDetails = false)
        {
            var list = productLogic.GetAllProducts().ToList();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    PrintProductItem(item);
                    if (printDetails) PrintProductDetails(item);
                }
            }
            else
            {
                Console.WriteLine("Item List is Empty");
            }
        }


        static void PrintOrderList(IOrderRepository? orderLogic, bool printDetails = false)
        {
            // PrintOrderList(orderLogic);
            var list = orderLogic.GetAllOrders().ToList();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    PrintOrderItem(item, printDetails);
                }
            }
            else
            {
                Console.WriteLine("Order List is Empty");
            }
            // /PrintOrderList(orderLogic);
        }


        // Make sure the products are loaded from the database before passing into this method.
        public static void PrintOrderItem(OrderEntity item, bool printDetails = false)
        {
            Console.WriteLine($"Order {item.Id} - {item.OrderDate}");
            if (printDetails)
            {
                if (item.Products == null) Console.WriteLine("item.Products is null\n");
                if (item.Products.Count == 0) Console.WriteLine("item.Products is empty\n");
                else
                {
                    foreach (var product in item.Products)
                    {
                        PrintProductItem(product);
                    }
                    Console.WriteLine();
                }
            }
        }



        // MENU SYSTEM
        struct MenuEntry
        {
            public string command;
            public string name;
            public string description;
        }

        static MenuEntry[][] _menu =
            [
            new MenuEntry[0],
                new MenuEntry[0]
            ];

        static void MenuInit()
        {
            _menu = new MenuEntry[2][];
            _menu[0] = new MenuEntry[25];
            _menu[1] = new MenuEntry[25];
        }



        static void MenuAddRow(int column, int row, string command, string name, string description)
        {
            if (row >= _menu[column].Length - 1)
            {
                MenuEntry[] tmp = _menu[column];
                _menu[column] = new MenuEntry[tmp.Length + 1];
                tmp.CopyTo(_menu[column], 0);
            }
            _menu[column][row] = new MenuEntry { command = command, name = name, description = description };
        }
        static void MenuAddRow(int column, string command, string name, string description)
        {
            MenuAddRow(column, _menu[column].Length, command, name, description);
        }
        static void MenuAddRow(int column, string text)
        {
            MenuAddRow(column, text, null, null);
        }
        static void MenuAddRow(int column)
        {
            MenuAddRow(column, "", null, null);
        }
        static void MenuAddRow()
        {
            MenuAddRow(0);
            MenuAddRow(1);
        }
        static void MenuAddRow(string text)
        {
            MenuAddRow(0, text, null, null);
            MenuAddRow(1, "");
        }



        static void MenuDisplay()
        {
            for (int i = 0; i < 25; i++)
            {
                MenuPrintRow(i);
            }
        }

        static void MenuPrintRow(int row)
        {
            bool flag = false;
            if (_menu[0].Length > row && _menu[0][row].command != null)
            {
                flag = true;
                if (_menu[0][row].name == null || _menu[0][row].description == null)
                    Console.Write($"{_menu[0][row].command,-42}");
                else
                    Console.Write($"{_menu[0][row].command,2}: {_menu[0][row].name,-7}  {_menu[0][row].description,-29}");
            }
            if (_menu[1].Length > row && _menu[1][row].command != null)
            {
                flag = true;
                if (_menu[1][row].name == null || _menu[1][row].description == null)
                    Console.Write($"{_menu[1][row].command}");
                else
                    Console.Write($"{_menu[1][row].command,2}: {_menu[1][row].name,-7}  {_menu[1][row].description}");
            }
            if (flag)
                Console.WriteLine();
        }
    }
}