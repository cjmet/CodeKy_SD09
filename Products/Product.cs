using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeKY_SD01.Products
{
    public class Product
    {
        [JsonPropertyOrder(0)]
        public int Id;
        [JsonPropertyOrder(1)]
        public string Name;
        [JsonPropertyOrder(2)]
        public string Description;
        [JsonPropertyOrder(3)]
        public decimal Price;
        [JsonPropertyOrder(4)]
        public int Quantity;

        public void AddProduct()
        {
            string userInput;
            Console.WriteLine("Enter the Product Name");
            userInput = Console.ReadLine();
            userInput = userInput.Trim();
            Name = userInput;

            Console.WriteLine("Enter the Product Description");
            userInput = Console.ReadLine();
            userInput = userInput.Trim();
            Description = userInput;
            {
                decimal d;
                do
                {
                    Console.WriteLine("Enter the Product Price");
                    userInput = Console.ReadLine();
                    userInput = userInput.Trim();
                } while (!decimal.TryParse(userInput, out d));
                Price = d;
            }

            {
                int i;
                do
                {
                    Console.WriteLine("Enter the Product Quantity");
                    userInput = Console.ReadLine();
                    userInput = userInput.Trim();
                } while (!int.TryParse(userInput, out i));
                Quantity = i;
            }
        }

        public void AddProduct(string name, string description, decimal price, int quantity)
        {
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
        }
    }



}
