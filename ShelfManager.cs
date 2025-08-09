using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Factory
{
    internal class ShelfManager
    {
        private readonly Shelf _shelf;

        public ShelfManager(Shelf shelf)
        {
            _shelf = shelf ?? throw new ArgumentNullException(nameof(shelf));

            Main();
        }

        public string GetId() => _shelf.GetId();
        public Product[]? GetProducts() => _shelf.GetProducts();
        public void AddProduct(Product product) => _shelf.AddProduct(product);
        public void RemoveProduct(Product product) => _shelf.RemoveProduct(product);
        public Shelf GetShelf() => _shelf;

        public Product CreateProduct(string name, string id, double price = 0, int quantity = 1, string description = "")
        {
            Product newProduct = new Product(name, id, price, quantity, description);

            AddProduct(newProduct);
            return newProduct;
        }
        public Product CreateProductGUI()
        {
            Console.Clear();
            Console.WriteLine("Creating a new product\n====================\n\nEnter an ID for the product:");
            
            string id = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("Product ID cannot be empty. Please try again.");
                return CreateProductGUI(); // Recursively call to re-enter product details
            }

            // Check if the ID already exists
            if (_shelf.FindProduct(id) != null)
            {
                Console.WriteLine($"Product with ID '{id}' already exists. Please enter a unique ID.");
                return CreateProductGUI(); // Recursively call to re-enter product details
            }

            Console.WriteLine("Enter a name for the product:");
            string name = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Product name cannot be empty. Please try again.");
                Console.Read();
                return CreateProductGUI(); // Recursively call to re-enter product details
            }

            Console.WriteLine("Enter a price for the product (default is 0):");
            string priceInputStr = Console.ReadLine() ?? "0";
            if (!double.TryParse(priceInputStr, out double priceInput) || priceInput <= 0)
            {
                priceInput = 0;
            }
            double price = priceInput > 0 ? (double)priceInput : 0; // Ensure price is at least 0

            Console.WriteLine("Enter a quantity for the product (default is 1):");
            string quantityInputStr = Console.ReadLine() ?? "1";
            if (!int.TryParse(quantityInputStr, out int quantityInput) || quantityInput <= 0)
            {
                quantityInput = 1;
            }
            int quantity = quantityInput > 0 ? (int)quantityInput : 0; // Ensure quantity is at least 1

            Console.WriteLine("Enter a description for the product (optional):");
            string? description = Console.ReadLine();
            description = string.IsNullOrWhiteSpace(description) ? null : description; // Set to null if empty

            Product newProduct = new Product(name, id, price, quantity, description);
            AddProduct(newProduct);
            Console.WriteLine($"Product '{newProduct.GetName()}' created successfully and added to shelf '{_shelf.GetId()}'.");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            return newProduct; // might be useful; will set to void if not
        }
        public Product? FindProduct(Product product)
        {
            return _shelf.FindProduct(product);
        }
        public Product? FindProduct(string productId)
        {
            return _shelf.FindProduct(productId);
        }

        public void RemoveProductGUI()
        {
            /*
            Console.Clear();
            Console.WriteLine("Removing a product\n====================\n\nEnter the ID of the product you want to remove:");
            string productId = Console.ReadLine() ?? "";
            Product? productToRemove = FindProduct(productId);
            if (productToRemove == null)
            {
                Console.WriteLine($"No product found with ID {productId}.");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                return;
            }
            _shelf.RemoveProduct(productToRemove);
            Console.WriteLine($"Product '{productToRemove.GetName()}' removed successfully from shelf '{_shelf.GetId()}'.");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            */

            Console.Clear();
            MenuBuilder menu = new MenuBuilder("Removing a product\n====================\n\nSelect a product to remove:");
            Product[]? products = _shelf.GetProducts();
            if (products == null || products.Length == 0)
            {
                Console.WriteLine("No products available to remove.");
                Console.WriteLine("Press any key to continue.");
                Console.Read();
                return;
            }

            for (int i = 0; i < products.Length; i++)
            {
                menu.AddItem($"{products[i].GetName()} (ID: {products[i].GetId()})");
            }

            int choice = menu.DisplayMenu();

            if (choice == -1)
            {
                Console.WriteLine("Cancelled.");
                Console.Read();
                return;
            }
            
            _shelf.RemoveProduct(products[choice]);
            Console.WriteLine($"Product '{products[choice].GetName()}' removed successfully from shelf '{_shelf.GetId()}'.");
            Console.WriteLine("Press any key to continue.");
            Console.Read();
        }
        public int RestockProduct(Product product, int quantity)
        {
            product.SetQuantity(product.GetQuantity() + quantity);
            return product.GetQuantity();
        }
        public void RestockProductGUI()
        {
            /*
            Console.Clear();
            Console.WriteLine("Restocking a product\n====================\n\nEnter the ID of the product you want to restock:");
            string productId = Console.ReadLine() ?? "";
            Product? productToRestock = FindProduct(productId);
            if (productToRestock == null)
            {
                Console.WriteLine($"No product found with ID {productId}.");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine($"Current quantity of '{productToRestock.GetName()}': {productToRestock.GetQuantity()}");
            int quantityToAdd = -1;
            while (quantityToAdd < 0) // will try to get a positive integer from the user until it succeeds
            {
                Console.WriteLine("Enter the quantity to add (must be a positive integer):");
                string input = Console.ReadLine() ?? "0";
                if (int.TryParse(input, out int quantity) && quantity > 0)
                {
                    quantityToAdd = quantity;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a positive integer.");
                }
            }
            int newQuantity = RestockProduct(productToRestock, quantityToAdd);
            Console.WriteLine($"Product '{productToRestock.GetName()}' restocked successfully. New quantity: {newQuantity}.");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            return;
            */

            Console.Clear();
            MenuBuilder menu = new MenuBuilder("Restocking a product\n====================\n\nSelect a product to restock:");
            Product[]? products = _shelf.GetProducts();
            if (products == null || products.Length == 0)
            {
                Console.WriteLine("No products available to restock.");
                Console.WriteLine("Press any key to continue.");
                Console.Read();
                return;
            }
            for (int i = 0; i < products.Length; i++)
            {
                menu.AddItem($"{products[i].GetName()} (ID: {products[i].GetId()})");
            }
            int choice = menu.DisplayMenu();

            if (choice == -1)
            {
                Console.WriteLine("Cancelled.");
                Console.Read();
                return;
            }

            Product productToRestock = products[choice];
            Console.WriteLine($"Current quantity of '{productToRestock.GetName()}': {productToRestock.GetQuantity()}");
            int quantityToAdd = -1;
            while (quantityToAdd < 0) // will try to get a positive integer from the user until it succeeds
            {
                Console.WriteLine("Enter the quantity to add (must be a positive integer):");
                string input = Console.ReadLine() ?? "0";
                if (int.TryParse(input, out int quantity) && quantity > 0)
                {
                    quantityToAdd = quantity;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a positive integer.");
                }
            }
            int newQuantity = RestockProduct(productToRestock, quantityToAdd);
            Console.WriteLine($"Product '{productToRestock.GetName()}' restocked successfully. New quantity: {newQuantity}.");
            Console.WriteLine("Press any key to continue.");
            Console.Read();
        }
        public void DisplayProducts()
        {
            Console.Clear();
            Console.WriteLine($"Products on shelf '{_shelf.GetId()}':");
            Product[]? products = _shelf.GetProducts();
            if (products == null || products.Length == 0)
            {
                Console.WriteLine("No products available on this shelf.");
            }
            else
            {
                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.GetId()}, Name: {product.GetName()}, Price: {product.GetPrice()}, Quantity: {product.GetQuantity()}, Description: {product.GetDescription()}");
                }
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        public void Main()
        {
            MenuBuilder menu = new MenuBuilder($"Shelf Manager\nCurrently managing shelf ID {_shelf.GetId()}\n===============\n");
            while (true)
            {
                menu.ClearMenu();
                menu.AddItem("Create Product");
                
                if (_shelf.GetProducts() != null && _shelf.GetProducts().Length > 0)
                {
                    menu.AddItem("Remove Product");
                    menu.AddItem("Display Products");
                    menu.AddItem("Restock Product");
                    menu.AddItem("Find Cheapest Product");
                    menu.AddItem("Find Most Expensive Product");
                }
                else
                {
                    menu.AddUnselectableItem("No products available to remove or display.");
                }

                int selection = menu.DisplayMenu();

                switch (selection)
                {
                    case 0:
                        CreateProductGUI();
                        break;
                    case 1:
                        RemoveProductGUI();
                        break;
                    case 2:
                        DisplayProducts();
                        break;
                    case 3:
                        RestockProductGUI();
                        break;
                    case 4:
                        Product? cheapestProduct = _shelf.FindCheapest();
                        if (cheapestProduct != null)
                        {
                            Console.WriteLine($"Cheapest Product: {cheapestProduct.GetName()} - Price: {cheapestProduct.GetPrice()}");
                        }
                        else
                        {
                            Console.WriteLine("No products available to determine the cheapest product.");
                        }
                        Console.WriteLine("Press any key to continue.");
                        Console.Read();
                        break;
                    case 5:
                        Product? mostExpensiveProduct = _shelf.FindMostExpensive();
                        if (mostExpensiveProduct != null)
                        {
                            Console.WriteLine($"Most Expensive Product: {mostExpensiveProduct.GetName()} - Price: {mostExpensiveProduct.GetPrice()}");
                        }
                        else
                        {
                            Console.WriteLine("No products available to determine the most expensive product.");
                        }
                        Console.WriteLine("Press any key to continue.");
                        Console.Read();
                        break;
                    default:
                        return; // Exit the method if an unrecognized selection is made (usually just escape)
                }
            }
            
        }

    }
}
