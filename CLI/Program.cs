using CodeKY_SD01.Logic;
using DataLibrary;
using Microsoft.Extensions.DependencyInjection;
using AngelHornetLibrary;
using AngelHornetLibrary.CLI;

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
            // Console.WriteLine("\n\n");

            Console.WriteLine();
            Console.WriteLine(AngelHornet.Logo());
            Console.WriteLine();

            string userInput;
            Console.WriteLine("Welcome to our Pet Shop!");
            Console.WriteLine("------------------------");

            // ############################################################################################################
            // MenuCli System - Work in Progress
            if (false)
            {
                MenuCli productMenu = new MenuCli();
                productMenu.AddItem("List", () => { Console.WriteLine("List"); });
                productMenu.AddItem("Detail", () => { Console.WriteLine("Detail"); });
                productMenu.AddItem("Add", () => { Console.WriteLine("Add"); });
                productMenu.AddItem("InStock", () => { Console.WriteLine("InStock"); });
                productMenu.AddItem("Update", () => { Console.WriteLine("Update"); });
                productMenu.AddItem("Delete", () => { Console.WriteLine("Delete"); });
                productMenu.AddItem(["Quit", "Exit"], () => { productMenu.Exit(); });

                productMenu.Loop();
                Environment.Exit(0);
            }


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
            MenuAddRow(0, "90", "Display", "Display Full Database.");
            MenuAddRow(1, "91", "Verbose", "Toggle VerboseSQL Mode.");
            MenuAddRow(0, "92", "Wipe", "delete all Products.");
            MenuAddRow(1, "93", "Wipe", "delete all Orders.");
            MenuAddRow(0, "94", "SeedDb", "Seed and/or List All Data.");
            MenuAddRow(1, "95", "WipeDb", "Wipe Databases.");

            MenuAddRow();

            MenuAddRow("Type 'exit' to quit.");

            do
            {
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

                    // ================================================================

                    case "21":
                        {
                            OrderEntity order = new OrderEntity();
                            order.OrderDate = DateTime.Now;
                            order.Products = new List<ProductEntity>();

                            var productList = productLogic.GetAllProducts().ToList();
                            ProductEntity product = null;
                            string userInput2;

                            do
                            {
                                //Console.Clear();
                                Console.WriteLine("Adding a new order.\n");
                                // +++++++++++++
                                PrintDivider();
                                PrintProductList(productLogic);
                                PrintDivider();
                                PrintOrderItem(order, true);
                                PrintDivider();
                                // +++++++++++++


                                product = null;
                                int productId = 0;
                                Console.WriteLine("Input ProductId or Keyword to add to the order, ");
                                Console.WriteLine("'Undo' to remove the last item, or <Enter> to Send the Order:");
                                userInput2 = Console.ReadLine();
                                userInput2 = userInput2.Trim().ToLower();


                                switch (userInput2)
                                {
                                    case "undo":
                                    case "u":
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
                                                // Futher Simplified
                                                //order.Products = stack.ToList();
                                                //order.Products.Remove(lastProduct);
                                                //stack = new Stack<ProductEntity>(order.Products);
                                                stack.Pop();
                                                order.Products = stack.Reverse().ToList();
                                                order.Products.Remove(lastProduct);
                                            }
                                            break;
                                        } // /Undo

                                    case "remove":
                                    case "r":
                                        {
                                            Console.WriteLine("Enter the product id or name to remove from the order:");
                                            var userInput3 = Console.ReadLine();
                                            userInput3 = userInput3.Trim().ToLower();
                                            if (int.TryParse(userInput3, out productId)) product = productLogic.GetProductById(productId);
                                            else if (userInput3 != "") product = productLogic.GetProductByName(userInput3);
                                            if (product != null) order.Products.Remove(product);
                                            break;
                                        } // /Remove


                                    default:
                                        {  // Add Product
                                            if (int.TryParse(userInput2, out productId)) product = productLogic.GetProductById(productId);
                                            else product = productLogic.GetProductByName(userInput2);
                                            if (product != null)
                                            {
                                                if (order.Products == null) order.Products = new List<ProductEntity>();
                                                order.Products.Add(product);
                                            }
                                            break;
                                        } // /Add Product
                                }

                                if (userInput2 != "" && product == null) Console.WriteLine($"Product '{userInput2}' was not found.");
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


                    // ================================================================


                    case "90":
                        {
                            Console.Clear();
                            Console.WriteLine("Displaying Full Database:");
                            PrintDivider();
                            PrintProductList(productLogic);
                            PrintDivider();
                            PrintOrderList(orderLogic);
                            PrintDivider();
                            break;
                        }
                        break;
                    case "91":
                        productLogic.VerboseSQL = !productLogic.VerboseSQL;
                        Console.WriteLine($"VerboseSQL is now {productLogic.VerboseSQL}");
                        PrintDivider();
                        break;
                    case "92":
                        {
                            var products = productLogic.GetAllProducts().ToList();
                            foreach (var item in products)
                            {
                                productLogic.DeleteProduct(item.Id);
                            }
                            productLogic.Seeded = false;
                            PrintDivider();
                            PrintProductList(productLogic);
                            PrintDivider();
                            PrintOrderList(orderLogic);
                            PrintDivider();
                            break;
                        }
                    case "93":  // delete all orders
                        {
                            var orders = orderLogic.GetAllOrders().ToList();
                            foreach (var item in orders)
                            {
                                orderLogic.DeleteOrder(item.Id);
                            }
                            productLogic.Seeded = false;
                            PrintDivider();
                            PrintProductList(productLogic);
                            PrintDivider();
                            PrintOrderList(orderLogic);
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
                            Console.WriteLine("Wiping the Database.");
                            Console.WriteLine("To completely RESET the Database, Restart the program now.");
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
                            productLogic.Seeded = false;
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