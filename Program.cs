using CodeKY_SD01.Interfaces;
using CodeKY_SD01.Logic;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using CodeKY_SD01.Data;
using Microsoft.EntityFrameworkCore;




namespace CodeKY_SD01
{
	internal class Program
	{

        // <SD01>
        //static IServiceProvider CreateServiceCollection()
        //{
        //    return new ServiceCollection().AddTransient<IProductLogic, ProductLogic>().BuildServiceProvider();
        //}
		// </SD01>

        static IServiceProvider CreateServiceCollection()
        {
            return new ServiceCollection()
				.AddTransient<IProductRepository,ProductLogic> ()
				.AddTransient<ProductContext>()
				.BuildServiceProvider();
		}

		static void Main(string[] args)
		{

			// <SD01>
			//var services = CreateServiceCollection();
			//var productLogic = services.GetService<IProductLogic>();
			// </SD01>

			//var productLogic = new CodeKY_SD01.Logic.ProductLogic();
			var services = CreateServiceCollection();
			var productLogic = services.GetService<IProductRepository>();

			string userInput;
			Console.WriteLine("Welcome to our Pet Shop!");
			Console.WriteLine("------------------------");
			Console.WriteLine();

			do
			{
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine("----------------------------------------------------------");
				Console.WriteLine("Press 1:  ADD       add a product.");
				Console.WriteLine("Press 2:  FIND      find a product by name.");
				Console.WriteLine("Press 3:  LIST      list all products.");
				Console.WriteLine("Press 4:  IN-STOCK  list only in-stock products.");
				Console.WriteLine();
                Console.WriteLine("Press 9:  TEST		Add Test Data to the Database.");
                Console.WriteLine();
				                Console.WriteLine("Type 'exit' to quit.");

				userInput = Console.ReadLine();
				userInput = userInput.Trim();
				userInput = userInput.ToLower();
				Console.WriteLine();
				Console.WriteLine("-=========================================================");
				Console.WriteLine();

				switch (userInput)
				{
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
							userInput2 = userInput2.Trim();
							var catFood = productLogic.GetProductsByName(userInput2).ToList();
							if (catFood != null && catFood.Count > 0)
							{
								foreach (var item in catFood)
								{
                                    // Console.WriteLine(JsonSerializer.Serialize(item, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true }));
                                    PrintLineItem(item);
                                    Console.WriteLine($"     \"{ item.Description}\"");
									Console.WriteLine();
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
							var list = productLogic.GetAllProducts().ToList();
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
							var list = productLogic.GetOnlyInStockProducts().ToList();
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
						
					case "9":
						{
							var tmp = new ProductLogic();
							tmp.DebugDatabaseInit();
							break;
                        }
	
					case "exit": Console.WriteLine("exit"); break;
					case "quit": Console.WriteLine("quit"); break;
					case "": Console.WriteLine("<empty>"); break;
					default: Console.WriteLine($"I do not recognize '{userInput}' as a valid input."); break;
				}

				Console.WriteLine();

			} while (!(userInput.Equals("exit", StringComparison.OrdinalIgnoreCase)
				|| userInput.Equals("quit", StringComparison.OrdinalIgnoreCase)
				|| userInput.Equals("", StringComparison.OrdinalIgnoreCase)
				));
		}

        static void PrintLineItem(ProductEntity item)
        {
            Console.WriteLine($"{item.Id,3}: {item.Name,-30} - {item.Category,15} - Qty: {item.Quantity,2} - {item.Price:C}");
        }

    }

}