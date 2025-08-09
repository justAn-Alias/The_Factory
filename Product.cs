using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;

namespace The_Factory
{
    #pragma warning disable CS0659
    internal class Product
    #pragma warning restore CS0659
    {
        // Default parameters for the product;
        private string name;
        private string id; // unique identifier for the product
        private string? description;
        private double price;
        private int quantity;

        public Product GetSelf()
        {
            return this;
        }
        public void SetName(string name)
        {
            this.name = name;
        }
        public string GetName()
        {
            return this.name;
        }

        public void SetPrice(double price)
        {
            this.price = price;
        }
        public double GetPrice()
        {
            return this.price;
        }

        public int GetQuantity()
        {
            return this.quantity;
        }
        public void SetQuantity(int quantity)
        {
            this.quantity = quantity;
        }
        public int AddQuantity(int quantity)
        {
            return this.quantity + quantity;
        }

        public string GetDescription()
        {
            // ?? = if this.description is null, return a different value; hence: if description is null, return the name
            return this.description ?? this.name;
        }
        public void SetDescription(string description)
        {
            this.description = description;
        }

        public string GetId()
        {
            return this.id;
        }
        public void SetId(string id)
        {
            this.id = id;
        }

        public override string ToString()
        {
            return $"Product: {this.name}, ID: {this.id}, Price: {this.price:C}, Quantity: {this.quantity}, Description: {this.GetDescription()}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is Product product)
            {
                return this.id == product.id;
            }
            return false;
        }

        public void Combine(Product product)
        {
            if (this.Equals(product))
            {
                this.quantity += product.quantity;
            }
            else
            {
                Console.WriteLine("Cannot combine products with different IDs, because they are different.");
            }
        }

        public bool TryCombine(Product product, bool silent = true)
        {
            if (this.Equals(product))
            {
                this.quantity += product.quantity;
                return true;
            }
            else
            {
                if (!silent)
                    Console.WriteLine("Cannot combine products with different IDs, because they are different.");
                return false;
            }
        }

        [JsonProperty]
        public string Name
        {
            get => name;
            set => name = value;
        }

        [JsonProperty]
        public string Id
        {
            get => id;
            set => id = value;
        }

        [JsonProperty]
        public string? Description
        {
            get => description;
            set => description = value;
        }

        [JsonProperty]
        public double Price
        {
            get => price;
            set => price = value;
        }

        [JsonProperty]
        public int Quantity
        {
            get => quantity;
            set => quantity = value;
        }

        [JsonConstructor]
        public Product(string name, string id, double price, int quantity = 0, string? description = null)
        {
            this.name = name;
            this.id = id;
            this.price = price;
            this.quantity = quantity;
            this.description = description;
        }
    }
}
