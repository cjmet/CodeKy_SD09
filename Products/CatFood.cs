using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CodeKY_SD01.Products
{
    public class CatFood : Product
    {
        [JsonPropertyOrder(10)]
        public virtual double Weight { get; set; }
        [JsonPropertyOrder(11)]
        public bool KittenFood;


        public CatFood() { }

        public CatFood(string name, string description, decimal price, int quantity, double weight, bool isKittenFood)
        {
            AddCatFood(name, description, price, quantity, weight, isKittenFood);
        }
        public void AddCatFood()
        {
            AddProduct();
            KittenFood = true;

            string userInput;
            Console.WriteLine("Enter the Product Weight");
            userInput = Console.ReadLine();
            userInput = userInput.Trim();
            Weight = double.Parse(userInput);


            do
            {
                Console.WriteLine("Is this Product a Kitten Food (Yes/No)?");
                userInput = Console.ReadLine();
                userInput = userInput.Trim();
                userInput = userInput.ToLower();
            } while (!(userInput.StartsWith("y") || userInput.StartsWith("n")));
            if (userInput.StartsWith("y")) KittenFood = true;
            else if (userInput.StartsWith("n")) KittenFood = false;
            else { Debug.Assert(false); }
        }

        public void AddCatFood(string name, string description, decimal price, int quantity, double weight, bool isKittenFood)
        {
            AddProduct(name, description, price, quantity);
            Weight = weight;
            KittenFood = isKittenFood;
        }

    }
}
