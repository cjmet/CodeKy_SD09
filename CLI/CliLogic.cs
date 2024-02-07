using CodeKY_SD01.Logic;
using DataLibrary;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeKY_SD01.Logic;
using DataLibrary;


// ######################################################
// CLEAN THIS LEGACY MESS UP LATER
// ######################################################

namespace CodeKY_SD01
{
    public static class CliLogic
    {
        public static void CliSwitch(IProductRepository productLogic, IOrderRepository orderLogic, int input)
        {
            //orderLogic = productLogic; // cjm 
            String userInput = input.ToString();
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
                            Console.Clear();
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
                            Console.WriteLine("'Undo' or 'Remove' to delete items from the order");
                            Console.WriteLine("Press <Enter> to Send the Order:");
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
                                            //  ICollection<ProductEntity> tmp;   
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
                                case "":    // Aka: Send Order
                                    {
                                        break;
                                    } 

                                default:
                                    {  // Add Product
                                        if (int.TryParse(userInput2, out productId)) product = productLogic.GetProductById(productId);
                                        else product = productLogic.GetProductByName(userInput2);
                                        if (product != null)
                                        {
                                            if (order.Products == null) order.Products = new List<ProductEntity>();  // cjm 
                                            order.Products.Add(product); 
                                        }
                                        break;
                                    } // /Add Product
                            }

                            if (userInput2 != "" && product == null) Console.WriteLine($"Product '{userInput2}' was not found.");
                        } while (userInput2 != "");


                        // Save the order
                        if (order.Products == null) order.Products = new List<ProductEntity>();  // cjm 
                        foreach (var item in order.Products)
                        {
                            if (item.Orders == null) item.Orders = new List<OrderEntity>();
                            //item.Orders.Add(order); // cjm  // Tracking takes care of this?
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
                        ProgramInfo(productLogic, orderLogic);
                        Console.WriteLine();
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
                    Console.Clear();
                    productLogic.VerboseSQL = !productLogic.VerboseSQL;
                    Console.WriteLine($"VerboseSQL is now {productLogic.VerboseSQL}");
                    PrintDivider();
                    PrintProductList(productLogic);
                    PrintDivider();
                    PrintOrderList(orderLogic);
                    PrintDivider();
                    break;
                case "92":
                    {
                        Console.Clear();
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
                case "93":  // delete all orders
                    {
                        Console.Clear();
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
                case "94":
                    {
                        Console.Clear();
                        //productLogic.DebugDatabaseInit();
                        PrintDivider();
                        PrintProductList(productLogic);
                        PrintDivider();
                        PrintOrderList(orderLogic);
                        PrintDivider();
                        break;
                    }
                case "95":
                    {
                        Console.Clear();
                        Console.WriteLine("Wiping the Database.");
                        //Console.WriteLine("To completely RESET the Database, Restart the program now.");
                        productLogic.ResetDatabase();
                        PrintDivider();
                        PrintProductList(productLogic);
                        PrintDivider();
                        PrintOrderList(orderLogic);
                        PrintDivider();
                        break;
                    }
            }




        }


        public static void PrintProductItem(ProductEntity item)
        {
            Console.WriteLine($"{item.Id,3}: {item.Name,-30} - {item.Category,15} - Qty: {item.Quantity,2} - {item.Price:C}");
        }

        public static void PrintDivider()
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



        public static void PrintProductList(IProductRepository? productLogic, bool printDetails = false)
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


        public static void PrintOrderList(IOrderRepository? orderLogic, bool printDetails = false)
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



        public static void ProgramInfo(IProductRepository? productLogic, IOrderRepository? orderLogic)
        {
            Console.WriteLine($"productLogic Name: {productLogic.ProductInterfaceFilename}");
            Console.WriteLine($"productLogic Func: {productLogic.ProductInterfaceFunctionName()}");
            Console.WriteLine($"productLogic Path: {productLogic.ProductDbPath}");

            Console.WriteLine($"orderLogic   Name: {orderLogic.OrderInterfaceFilename}");
            Console.WriteLine($"orderLogic   Func: {orderLogic.OrderInterfaceFunctionName()}");
            Console.WriteLine($"orderLogic   Path: {orderLogic.OrderDbPath}");

            if (productLogic.DataExists())
            {
                Console.WriteLine("Order Repository Already Contains Data.");
                Console.WriteLine($"Products: {productLogic.GetAllProducts().Count()}     Orders: {orderLogic.GetAllOrders().Count()}");
            }
        }


    }
}
