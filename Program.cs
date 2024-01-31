using CodeKY_SD01.Interfaces;
using CodeKY_SD01.Logic;
using CodeKY_SD01.Products;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Text.Json;


// q: what does solid stand for in OO programming
// a: Single Responsibility, Open-Closed, Liskov Substitution, Interface Segregation, Dependency Inversion

// q: what is a class
// a: a class is a blueprint for an object


namespace CodeKY_SD01
{
	internal class Program
	{
		static void Main(string[] args)
		{

			var services = CreateServiceCollection();
			//var productLogic = new ProductLogic();
			var productLogic = services.GetService<IProductLogic>();

			#if DEBUG
			productLogic.DebugDatabaseInit();
			#endif

			string userInput;
			Console.WriteLine("Welcome to our Pet Shop!");
			Console.WriteLine("------------------------");
			Console.WriteLine();

			do
			{
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine("----------------------------------------------------------");
				Console.WriteLine("Press 1 to add a product.");
				Console.WriteLine("Press 2 to view a Cat Food by name.");
				Console.WriteLine();
				Console.WriteLine("Press 8 to search a list of products for a keyword.");
				Console.WriteLine("Press 9 to view a list of products.");
				Console.WriteLine();
				Console.WriteLine("Press 10 to view a list of In-Stock products by Name.");
				Console.WriteLine("Press 11 to view a list of In-Stock products by Product.");
				Console.WriteLine();
				Console.WriteLine("Press 20 to view a total value of In-Stock inventory");
				Console.WriteLine("Press 21 to import a JSON encoded product");
				Console.WriteLine("Press 22 to find any product by name");
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
						CatFood catChow = new CatFood();
						catChow.AddCatFood();

						productLogic.AddProduct(catChow);

#if DEBUG
						Console.WriteLine(JsonSerializer.Serialize(catChow, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true }));
#endif

						Console.WriteLine("Product Added.");
						break;
					case "2":
						{
							Console.WriteLine("Enter the Cat Food you wish to view.");
							string userInput2 = Console.ReadLine();
							userInput2 = userInput2.Trim();
							CatFood catFood = productLogic.GetProductByName<CatFood>(userInput2);
							if (catFood != null)
							{
								Console.WriteLine(JsonSerializer.Serialize(catFood, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true }));
							}
							else
							{
								Console.WriteLine($"Product '{userInput2}' was not found.");
							}
							break;
						}
					case "8":
						{
							Console.WriteLine("Enter the Key Word");
							string userInput2 = Console.ReadLine();
							userInput2 = userInput2.Trim();
							List<Product> results = productLogic.SearchProducts(userInput2);
							if (results.Count > 0)
							{
								Console.WriteLine();
								foreach (var result in results)
								{
									Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true }));
								}
								Console.WriteLine($"{results.Count} Items Found.");
							}
							else
							{
								Console.WriteLine($"Product '{userInput2}' was not found.");
							}
							break;
						}
					case "9":
						{
							var list = productLogic.GetAllProducts();
							if (list != null && list.Count > 0)
							{
								Console.WriteLine("The Following is a list of all Items.");
								foreach (var item in productLogic.GetAllProducts())
								{
									Console.WriteLine(item.Name);
								}
							}
							else
							{
								Console.WriteLine("Item List is Empty");
							}
							break;
						}
					case "10":
						{
							var list = productLogic.GetOnlyInStockProductsByName();
							if (list != null && list.Count > 0)
							{
								Console.WriteLine("The Following is a list of all In-Stock Items.");
								foreach (var item in list)
								{
									Console.WriteLine(item);
								}
							}
							else
							{
								Console.WriteLine("Item List is Empty");
							}
							break;
						}
					case "11":
						{
							var list = productLogic.GetOnlyInStockProducts();
							if (list != null && list.Count > 0)
							{
								Console.WriteLine("The Following is a list of all In-Stock Items.");
								Console.WriteLine();
								foreach (var result in list)
								{
									Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true }));
								}
								Console.WriteLine($"{list.Count} Items Found.");
							}
							else
							{
								Console.WriteLine("Item List is Empty");
							}
							break;
						}
					case "20":
						{
							var total = productLogic.GetTotalPriceOfInventory();
							Console.WriteLine($"The Total Value of all In-Stock Items is: {total}");
							break;
						}
					case "21":
						{
							Console.WriteLine("Importing a JSON Encoded Product.");
							Console.WriteLine("Paste JSON code here:");
							Product product;
							string jsonInput;
							Boolean TEST=false;
							if (TEST)
							{
								product = productLogic.GetTestProduct();
								jsonInput = JsonSerializer.Serialize(product, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true });
							}
							else
							{
								jsonInput = Console.ReadLine();
							}
							product = JsonSerializer.Deserialize<Product>(jsonInput, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true });
							productLogic.AddProduct(product);
							
							Console.WriteLine("Product Added.");
							break;
						}
					case "22":
						{
							Console.WriteLine("Enter the Product you wish to view.");
							string userInput2 = Console.ReadLine();
							userInput2 = userInput2.Trim();
							Product product = productLogic.GetProductByName<Product>(userInput2);
							if (product != null)
							{
								Console.WriteLine(JsonSerializer.Serialize(product, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true }));
							}
							else
							{
								Console.WriteLine($"Product '{userInput2}' was not found.");
							}
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
		static IServiceProvider CreateServiceCollection()
		{
			return new ServiceCollection().AddTransient<IProductLogic, ProductLogic>().BuildServiceProvider();
		}
	}

}