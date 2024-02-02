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
                .BuildServiceProvider();
        }

        static void Main(string[] args)
        {

            var services = CreateServiceCollection();
            var productLogic = services.GetService<IProductRepository>();
            Console.WriteLine($"Database Path: {productLogic.DbPath}\n");

            string userInput;
            Console.WriteLine("Welcome to our Pet Shop!");
            Console.WriteLine("------------------------");

            bool _exitProgram = false;
            string _lastInput = "false";
            do
            {
                Console.WriteLine();
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine("Press 1:  ADD       add a product.");
                Console.WriteLine("Press 2:  FIND      find a product by name.");
                Console.WriteLine("Press 3:  LIST      list all products.");
                Console.WriteLine("Press 4:  IN-STOCK  list only in-stock products.");
                Console.WriteLine();
                Console.WriteLine("Press 8:  DELETE    delete product by id.");
                Console.WriteLine("Press 9:  TEST      Add Test Data to the Database.");
                Console.WriteLine("Press 0:  VERBOSE   Toggle VerboseSQL Mode.");
                Console.WriteLine();
                Console.WriteLine("Type 'exit' to quit.");

                userInput = Console.ReadLine();
                userInput = userInput.Trim();
                userInput = userInput.ToLower();

                Console.Clear();
                Console.WriteLine($"User Input: {userInput}");

                switch (userInput)
                {
                    case "0":
                        productLogic.VerboseSQL = !productLogic.VerboseSQL;
                        Console.WriteLine($"VerboseSQL is now {productLogic.VerboseSQL}");
                        PrintDivider();
                        break;
                    case "1":
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
                    case "2":
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
                    case "3":
                        {
                            Console.WriteLine();
                            var list = productLogic.GetAllProducts().ToList();
                            PrintDivider();
                            if (list != null && list.Count > 0)
                            {
                                Console.WriteLine("The Following is a list of all Items.");
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
                    case "4":
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

                    case "8":
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
                    case "9":
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