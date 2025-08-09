using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace The_Factory
{
    #pragma warning disable CS0659
    internal class Shelf
    #pragma warning restore CS0659
    {
        string id;
        Product[] products; // Array of products that appear on the shelf; Unlimited number of products can be added to the shelf

        public void SetId(string id)
        {
            this.id = id;
        }
        public string GetId()
        {
            return id;
        }

        public Product? FindProduct(Product product)
        {
            if (products == null || products.Length == 0)
            {
                return null; // No products on the shelf
            }
            foreach (var p in products)
            {
                if (p.GetId() == product.GetId())
                {
                    return p; // Product found, returning the product
                }
            }
            return null; // Product not found
        }
        public Product? FindProduct(string productId) // try finding products with the ID of the product
        {
            if (products == null || products.Length == 0)
            {
                return null; // No products on the shelf
            }
            foreach (var p in products)
            {
                if (p.GetId() == productId)
                {
                    return p; // Product found, returning the product
                }
            }
            return null; // Product not found
        }

        public Product[]? GetProducts()
        {
            return products; // Returns the array of products on the shelf, or null if there are no products
        }

        public void AddProduct(Product product)
        {
            if (products == null)
            {
                products = new Product[] { product };
            }
            else
            {
                Array.Resize(ref products, products.Length + 1);
                products[products.Length - 1] = product;
            }
        }

        public void RemoveProduct(Product product)
        {
            if (products == null || products.Length == 0)
            {
                return; // No products to remove
            }

            for (int i = 0; i < products.Length; i++)
            {
                if (products[i].GetId() == product.GetId())
                {
                    // Found the product, remove it
                    for (int j = i; j < products.Length - 1; j++)
                    {
                        products[j] = products[j + 1]; // Shift elements to the left
                    }
                    Array.Resize(ref products, products.Length - 1); // Resize the array to remove the last element
                    return;
                }
            }

            Console.WriteLine("Product not found on the shelf, cannot remove it.");
        }
        public void RemoveProduct(string productId)
        {
            if (products == null || products.Length == 0)
            {
                return; // No products to remove
            }

            for (int i = 0; i < products.Length; i++)
            {
                if (products[i].GetId() == productId)
                {
                    // Found the product, remove it
                    for (int j = i; j < products.Length - 1; j++)
                    {
                        products[j] = products[j + 1]; // Shift elements to the left
                    }
                    Array.Resize(ref products, products.Length - 1); // Resize the array to remove the last element
                    return;
                }
            }
            Console.WriteLine("Product not found on the shelf, cannot remove it.");
        }

        public Product? FindCheapest()
        {
            if (products == null || products.Length == 0)
            {
                return null; // No products on the shelf
            }
            Product cheapest = products[0]; // Assume the first product is the cheapest initially
            foreach (var product in products)
            {
                if (product.GetPrice() < cheapest.GetPrice())
                {
                    cheapest = product; // Update cheapest if a cheaper product is found
                }
            }
            return cheapest; // Return the cheapest product found
        }

        public Product? FindMostExpensive()
        {
            if (products == null || products.Length == 0)
            {
                return null; // No products on the shelf
            }
            Product mostExpensive = products[0]; // Assume the first product is the most expensive initially
            foreach (var product in products)
            {
                if (product.GetPrice() > mostExpensive.GetPrice())
                {
                    mostExpensive = product; // Update most expensive if a more expensive product is found
                }
            }
            return mostExpensive; // Return the most expensive product found
        }

        public string GetAllQuantities()
        {
            if (products == null || products.Length == 0)
            {
                return "No products on the shelf."; // No products to display quantities
            }
            StringBuilder sb = new StringBuilder();
            foreach (var product in products)
            {
                sb.AppendLine($"{product.GetName()} - {product.GetQuantity()}"); // Append each product's name and quantity
            }
            return sb.ToString(); // Return all quantities as a string
        }

        public override string ToString()
        {
            if (products == null || products.Length == 0)
            {
                return $"Shelf ID: {id}, No products on the shelf.";
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Shelf ID: {id};");
            foreach (var product in products)
            {
                sb.AppendLine(product.ToString());
            }
            return sb.ToString();
        }

        public override bool Equals(object? obj)
        {
            if (obj is Shelf shelf)
            {
                return this.id == shelf.id; // Compare shelves by their ID
            }
            return false;
        }

        public void Combine(Shelf shelf)
        {
            if (this.Equals(shelf))
            {
                if (shelf.products != null)
                {
                    foreach (var product in shelf.products)
                    {
                        this.AddProduct(product); // Add products from the other shelf to this shelf
                    }
                }
            }
            else
            {
                Console.WriteLine("Cannot combine shelves with different IDs, because they are different.");
            }
        }

        [JsonProperty]
        public string Id
        {
            get => id;
            set => id = value;
        }

        [JsonProperty]
        public Product[] Products
        {
            get => products;
            set => products = value;
        }

        public Shelf(string id)
        {
            this.id = id;
            this.products = Array.Empty<Product>(); // Initialize products as null
        }
        
        [JsonConstructor]
        public Shelf(string Id, Product[] Products)
        {
            this.id = Id;
            this.products = Products; // Initialize with the provided array of products (also using the public / json property setter)
        }
    }
}
