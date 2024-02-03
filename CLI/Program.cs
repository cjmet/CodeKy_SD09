using CodeKY_SD01.Logic;
using DataLibrary;
using Microsoft.Extensions.DependencyInjection;




namespace CodeKY_SD01
{
    internal class Program
    {

        static IServiceProvider CreateServiceCollection()
        {
            return new ServiceCollection()
                .AddTransient<IProductRepository, ProductLogic>()
                .AddTransient<ProductContext>()
                .AddTransient<IOrderRepository, ProductLogic>()
                .BuildServiceProvider();
        }

        static void Main(string[] args)
        {

            var services = CreateServiceCollection();
            var productLogic = services.GetService<IProductRepository>();
            var orderLogic = services.GetService<IOrderRepository>();
            Console.WriteLine($"Database Path: {productLogic.DbPath}\n");

            string userInput;
            Console.WriteLine("Welcome to our Pet Shop!");
            Console.WriteLine("------------------------");

            bool _exitProgram = false;
            string _lastInput = "false";
            do
            {
                Console.WriteLine();
                //               ("1234567890123456789012345678901234567..|..34567890123456789012345678901234567");
                // Console.WriteLine("-----------------------------------------------------------------------------");
                Console.WriteLine("Product Commands                          Orders Commands");
                Console.WriteLine("----------------                          ---------------");
                Console.Write($"{"11: Add      add a product.",-42}");
                Console.WriteLine($"{"21: Add      add an order."}");

                Console.Write($"{"12: Find     find product by name.",-42}");
                Console.WriteLine($"{"22: Find     find order by id."}");

                Console.Write($"{"13: List     list all products.",-42}");
                Console.WriteLine($"{"23: List     list all orders."}");

                Console.Write($"{"14: InStock  list in-stock products.",-42}");
                Console.WriteLine($"{"24:          unimplemented."}");

                Console.Write($"{"15: Delete   delete product by id.",-42}");
                Console.WriteLine($"{"25: Delete   delete order by id."}");

                Console.WriteLine();

                Console.Write($"{"Test Commands:",-42}");
                Console.WriteLine($"{""}");
                Console.Write($"{"90: Verbose  Toggle VerboseSQL Mode.",-42}");
                Console.WriteLine($"{"91: TEST     Add Test Data."}");

                Console.WriteLine();
                Console.WriteLine("Type 'exit' to quit.");

                userInput = Console.ReadLine();
                userInput = userInput.Trim();
                userInput = userInput.ToLower();

                Console.Clear();
                Console.WriteLine($"User Input: {userInput}");

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
                            productLogic.AddProduct(product);
                            PrintDivider();
                            if (product.Id > 0)
                                Console.WriteLine("Product Added.");
                            else
                                Console.WriteLine("Product Not Added.");
                        }
                        break;
                    case "12":
                        {
                            Console.WriteLine("Enter the product you wish to view.");
                            string userInput2 = Console.ReadLine();
                            Console.WriteLine();
                            userInput2 = userInput2.Trim();
                            var catFood = productLogic.GetAllProductsByName(userInput2).ToList();
                            PrintDivider();
                            if (catFood != null && catFood.Count > 0)
                            {
                                int flag = 0;
                                foreach (var item in catFood)
                                {
                                    // Console.WriteLine(JsonSerializer.Serialize(item, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true }));
                                    if (flag++ > 0) Console.WriteLine();
                                    PrintLineItem(item);
                                    Console.WriteLine($"     \"{item.Description}\"");
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
                            Console.WriteLine();
                            var list = productLogic.GetAllProducts().ToList();
                            PrintDivider();
                            if (list != null && list.Count > 0)
                            {
                                Console.WriteLine("The Following is a list of all Items:\n");
                                foreach (var item in list)
                                {
                                    PrintLineItem(item);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Item List is Empty");
                            }
                            break;
                        }
                    case "14":
                        {
                            Console.WriteLine();
                            var list = productLogic.GetOnlyInStockProducts().ToList();
                            PrintDivider();
                            if (list != null && list.Count > 0)
                            {
                                Console.WriteLine("The Following is a list of all In-Stock Items.");
                                foreach (var item in list)
                                {
                                    PrintLineItem(item);
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
                            Console.WriteLine("Enter the product id you wish to delete.");
                            string userInput2 = Console.ReadLine();
                            userInput2 = userInput2.Trim();
                            Console.WriteLine();
                            if (int.TryParse(userInput2, out int id) && productLogic.GetProductById(id) != null)
                            {
                                productLogic.DeleteProduct(id);
                                PrintDivider();
                                Console.WriteLine($"Product with id {id} has been deleted.");
                            }
                            else
                            {
                                PrintDivider();
                                Console.WriteLine($"Product with id {userInput2} was not found.");
                            }
                            break;
                        }

                    // ================================================================

                    case "21":
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
                                // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                                PrintDivider();
                                if (productList != null && productList.Count > 0)
                                {
                                    Console.WriteLine("Items:\n");
                                    foreach (var item in productList)
                                    {
                                        PrintLineItem(item);
                                    }
                                }
                                PrintDivider();
                                // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                                Console.WriteLine($"Order {order.Id}   Date: {order.OrderDate}");
                                foreach (var item in order.Products)
                                {
                                    PrintLineItem(item);
                                }
                                PrintDivider();
                                // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                                Console.WriteLine("Input ProductId, ProductName, Submit or <Enter>:");
                                userInput2 = Console.ReadLine();
                                userInput2 = userInput2.Trim();

                                product = null;
                                int productId = 0;
                                if (int.TryParse(userInput2, out productId)) product = productLogic.GetProductById(productId);
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
                            } while (userInput2 != "" && userInput2 != "done" && userInput2 != "submit" && userInput2 != "order");


                            // Save the order
                            if (order.Products == null) order.Products = new List<ProductEntity>();
                            foreach (var item in order.Products)
                            {
                                if (item.Orders == null) item.Orders = new List<OrderEntity>();
                                item.Orders.Add(order);
                            }
                            if (order.Products.Count > 0) orderLogic.AddOrder(order);

                            if (order.Id > 0)
                                Console.WriteLine("Order Added.");
                            else
                                Console.WriteLine("Order Not Added.");
                        }
                        break;

                    case "22":  // find order by id
                        {
                            Console.WriteLine("Enter the order id you wish to view.");
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
                                    PrintLineItem(item);
                                }
                            }
                            else
                            {
                                PrintDivider();
                                Console.WriteLine($"Order with id {userInput2} was not found.");
                            }
                            break;
                        }
                    case "23":  // list all orders
                        {
                            Console.WriteLine();
                            var list = orderLogic.GetAllOrders().ToList();
                            PrintDivider();
                            if (list != null && list.Count > 0)
                            {
                                Console.WriteLine("The Following is a list of all Orders:\n");
                                foreach (var item in list)
                                {
                                    orderLogic.GetOrderById(item.Id); // force load of products
                                    Console.WriteLine($"Order {item.Id} - {item.OrderDate}");
                                    foreach (var product in item.Products)
                                    {
                                        PrintLineItem(product);
                                    }
                                    Console.WriteLine();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Order List is Empty");
                            }
                            break;
                        }
                    case "24":  // unimplemented
                        {
                            Console.WriteLine("Unimplemented");
                            break;
                        }
                    case "25":  // delete order by id
                        {
                            Console.WriteLine("Enter the order id you wish to delete.");
                            string userInput2 = Console.ReadLine();
                            userInput2 = userInput2.Trim();
                            Console.WriteLine();
                            if (int.TryParse(userInput2, out int id) && orderLogic.GetOrderById(id) != null)
                            {
                                orderLogic.DeleteOrder(id);
                                PrintDivider();
                                Console.WriteLine($"Order with id {id} has been deleted.");
                            }
                            else
                            {
                                PrintDivider();
                                Console.WriteLine($"Order with id {userInput2} was not found.");
                            }
                            break;
                        }
                    // ================================================================
                    case "90":
                        productLogic.VerboseSQL = !productLogic.VerboseSQL;
                        Console.WriteLine($"VerboseSQL is now {productLogic.VerboseSQL}");
                        PrintDivider();
                        break;
                    case "91":
                        {
                            var tmp = new ProductLogic();
                            tmp.DebugDatabaseInit();
                            PrintDivider();
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

        static void PrintLineItem(ProductEntity item)
        {
            Console.WriteLine($"{item.Id,3}: {item.Name,-30} - {item.Category,15} - Qty: {item.Quantity,2} - {item.Price:C}");
        }

        static void PrintDivider()
        {
            Console.WriteLine("==========================================================");
            Console.WriteLine();
        }

    }

}