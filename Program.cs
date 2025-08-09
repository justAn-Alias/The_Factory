using System.IO; // For file operations
using Newtonsoft.Json; // Nuget package - sourced from google about saving data.

namespace The_Factory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double TotalProfit = 0;
            Shelf[] Inventory = new Shelf[0]; // Array of shelves, each shelf can hold multiple products

            // Due to the inventory storing shelves, and shelves storing products, the only way to save everything to a file is by saving the entire inventory.
            // Which is.. not ideal. 🤷

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto, // Enable type name handling to serialize and deserialize derived types correctly
                Formatting = Formatting.Indented // Use indented formatting for better readability
            };

            //FLAGS - Store 'flags' to know whether an event has happened or not.  
            bool LaunchedProgram = true;
            bool FirstTimeAddingShelf = true;

            void AddShelf()
            {
                Console.Clear();
                Console.WriteLine("Enter an ID for the shelf:");
                string shelfId = Console.ReadLine();

                // Check for duplicate shelf IDs
                if (Inventory.Any(shelf => shelf.GetId() == shelfId))
                {
                    Console.WriteLine($"A shelf with ID {shelfId} already exists.");
                    Console.WriteLine("Press any key to continue.");
                    Console.Read();
                    return;
                }

                Inventory = Inventory.Append(new Shelf(shelfId)).ToArray();
                Console.WriteLine($"Shelf with ID {shelfId} has been added.");

                if (FirstTimeAddingShelf)
                {
                    Console.WriteLine("\nThis note is shown because this is the first time you're adding a shelf:\nAll shelves are empty by default, and you need to add products to it.\nYou can do so in the 'Manage a shelf' menu.\n");
                    FirstTimeAddingShelf = false; // Set the flag to false after the first shelf is added  
                }

                Console.WriteLine("Press any key to continue.");
                Console.Read(); // Do not store any variable - just wait for the user to press any key  
            }

            void RemoveShelf()
            {
                Console.Clear();
                MenuBuilder selectMenu = new MenuBuilder("Select which shelf you want to remove:");

                for (int i = 0; i < Inventory.Length; i++)
                {
                    selectMenu.AddItem(Inventory[i].GetId());
                }

                int selection = selectMenu.DisplayMenu();
                if (selection == -1)
                {
                    return; // Exit if no shelf is selected
                }

                Shelf selectedShelf = Inventory[selection];
                Console.WriteLine($"You are about to remove the shelf with ID {selectedShelf.GetId()}.");
                Console.WriteLine("Do you want to proceed? (yes/no)");
                string confirmation = Console.ReadLine().ToLower();
                if (confirmation == "yes" || confirmation == "y")
                {
                    Inventory = Inventory.Where(shelf => shelf.GetId() != selectedShelf.GetId()).ToArray(); // Remove the selected shelf
                    Console.WriteLine($"Shelf with ID {selectedShelf.GetId()} has been removed.");
                    Console.Read(); // Wait for the user to press any key
                }
                else
                {
                    Console.WriteLine("Removal cancelled.");
                    Console.Read(); // Wait for the user to press any key
                }
            }

            void ListShelves()
            {
                Console.Clear();
                if (Inventory.Length == 0)
                {
                    Console.WriteLine("No shelves available in the inventory.");
                }
                else
                {
                    Console.WriteLine("Current shelves in the inventory:");
                    foreach (var shelf in Inventory)
                    {
                        Console.WriteLine(shelf.ToString());
                    }
                }

                Console.WriteLine("Press any key to continue.");
                Console.Read();
            }

            void ManageShelf()
            {
                Console.Clear();
                MenuBuilder selectMenu = new MenuBuilder("Select which shelf you want to manage:");

                for (int i = 0; i < Inventory.Length; i++)
                {
                    selectMenu.AddItem(Inventory[i].GetId());
                }

                int selection = selectMenu.DisplayMenu();
                if (selection == -1)
                {
                    return; // Exit if no shelf is selected
                }

                Shelf selectedShelf = Inventory[selection];
                ShelfManager shelfManager = new ShelfManager(selectedShelf); // Create a ShelfManager for the selected shelf
            }

            bool AreThereProducts() // Utility function
            {
                if (Inventory.Length == 0)
                {
                    return false; // No shelves, hence no products
                }
                foreach (var shelf in Inventory)
                {
                    if (shelf.GetProducts() != null && shelf.GetProducts().Length > 0)
                    {
                        return true; // If any shelf has products, return true
                    }
                }
                return false; // No products found in any shelf
            }

            void FindCheapestProduct()
            {
                Console.Clear();
                if (Inventory.Length == 0)
                {
                    Console.WriteLine("No shelves available to search for products.");
                    Console.WriteLine("Press any key to continue.");
                    Console.Read();
                    return;
                }

                Console.WriteLine("Finding the cheapest product across all shelves...");
                Product? cheapestProduct = null;
                foreach (var shelf in Inventory)
                {
                    Product? shelfCheapest = shelf.FindCheapest();
                    if (shelfCheapest != null && (cheapestProduct == null || shelfCheapest.GetPrice() < cheapestProduct.GetPrice()))
                    {
                        cheapestProduct = shelfCheapest;
                    }
                }
                Console.Clear();
                if (cheapestProduct != null)
                {
                    Console.WriteLine($"The cheapest product is: {cheapestProduct.GetName()} with ID {cheapestProduct.GetId()}; priced at {cheapestProduct.GetPrice()}.");
                }
                else
                {
                    Console.WriteLine("No products found in any shelf.");
                }

                Console.WriteLine("Press any key to continue.");
                Console.Read();
            }

            void FindMostExpensiveProduct()
            {
                Console.Clear();
                if (Inventory.Length == 0)
                {
                    Console.WriteLine("No shelves available to search for products.");
                    Console.WriteLine("Press any key to continue.");
                    Console.Read();
                    return;
                }
                Console.WriteLine("Finding the most expensive product across all shelves...");
                Product? mostExpensiveProduct = null;
                foreach (var shelf in Inventory)
                {
                    Product? shelfMostExpensive = shelf.FindMostExpensive();
                    if (shelfMostExpensive != null && (mostExpensiveProduct == null || shelfMostExpensive.GetPrice() > mostExpensiveProduct.GetPrice()))
                    {
                        mostExpensiveProduct = shelfMostExpensive;
                    }
                }
                Console.Clear();
                if (mostExpensiveProduct != null)
                {
                    Console.WriteLine($"The most expensive product is: {mostExpensiveProduct.GetName()} with ID {mostExpensiveProduct.GetId()}; priced at {mostExpensiveProduct.GetPrice()}.");
                }
                else
                {
                    Console.WriteLine("No products found in any shelf.");
                }
                Console.WriteLine("Press any key to continue.");
                Console.Read();
            }

            void SellProduct()
            {
                Console.Clear();
                // Check if there are any products in the inventory
                if (!AreThereProducts())
                {
                    Console.WriteLine("No products available to sell.");
                    Console.WriteLine("Press any key to continue.");
                    Console.Read();
                    return;
                }

                Console.WriteLine("Enter the ID of the product you want to sell:");
                string productId = Console.ReadLine();
                Product? productToSell = null;

                // Search for the product across all shelves
                foreach (var shelf in Inventory)
                {
                    productToSell = shelf.GetProducts()?.FirstOrDefault(product => product.GetId() == productId);
                    if (productToSell != null)
                    {
                        break; // Product found, exit the loop
                    }
                }

                if (productToSell == null)
                {
                    Console.WriteLine($"No product found with ID {productId}.");
                    Console.WriteLine("Press any key to continue.");
                    Console.Read();
                    return;
                }

                // Confirm the sale
                Console.WriteLine($"You are about to sell the product: {productToSell.GetName()} with ID {productToSell.GetId()}; priced at {productToSell.GetPrice()}.");
                Console.WriteLine("Do you want to proceed with the sale? (yes/no)");
                string confirmation = Console.ReadLine().ToLower();
                if (confirmation == "yes" || confirmation == "y")
                {
                    Console.WriteLine($"There are {productToSell.GetQuantity()} units available for sale.");
                    Console.WriteLine("How many units do you want to sell?");
                    int quantityToSell;
                    while (!int.TryParse(Console.ReadLine(), out quantityToSell) || quantityToSell < 0 || quantityToSell > productToSell.GetQuantity())
                    {
                        Console.WriteLine($"Invalid input. Please enter a number between 0 and {productToSell.GetQuantity()}.");
                    }
                    if (quantityToSell == 0)
                    {
                        Console.WriteLine("No units sold. Sale cancelled.");
                        return;
                    } 
                    else
                    {
                        Console.WriteLine($"Sold {quantityToSell} units of {productToSell.GetName()} with ID {productToSell.GetId()} for {productToSell.GetPrice() * quantityToSell}.");
                        TotalProfit += (productToSell.GetPrice() * quantityToSell); // Update total profit
                        productToSell.SetQuantity(productToSell.GetQuantity() - quantityToSell); // Update the product quantity

                        Console.Read();
                        return;
                    }
                } 
                else
                {
                    Console.WriteLine("Sale cancelled.");
                    Console.Read();
                    return;
                }
            }

            void SaveInventoryToFile(bool silent)
            {
                string json = JsonConvert.SerializeObject(Inventory, Formatting.Indented); // Serialize the inventory to JSON format
                File.WriteAllText("inventory.json", json); // Write the JSON to a file

                if (!silent)
                {
                    Console.WriteLine("Inventory saved to inventory.json.");
                    Console.Read();
                }
            }

            Shelf[] LoadInventoryFromFile(bool silent)
            {
                if (File.Exists("inventory.json"))
                {
                    string json = File.ReadAllText("inventory.json"); // Read the JSON from the file

                    if (!silent)
                    {
                        Console.WriteLine("Inventory loaded from inventory.json.");
                        Console.Read();
                    }

                    return JsonConvert.DeserializeObject<Shelf[]>(json) ?? new Shelf[0]; // Deserialize the JSON to an array of shelves
                }

                if (!silent)
                {
                    Console.WriteLine("No inventory file found. Starting with an empty inventory.");
                    Console.Read();
                }
                return new Shelf[0]; // Return an empty array if the file does not exist or failed to deserialize
            }

            Inventory = LoadInventoryFromFile(true); // Load the inventory from the file at the start of the program

            // Create a choice system for the user to interact with the inventory  
            MenuBuilder menu = new MenuBuilder($"Inventory Management System\n===============\nYou have {Inventory.Length} shelf(s) in your inventory.\nTotal Profit: {TotalProfit}\n===============\n");
            while (true)
            {
                menu.ClearMenu();

                menu.AddItem("Add a new shelf");
                if (Inventory.Length == 0)
                {
                    menu.AddUnselectableItem("Remove a shelf");
                    menu.AddUnselectableItem("View all shelves");
                    menu.AddUnselectableItem("Manage a shelf");
                    menu.AddUnselectableItem("Find cheapest product across all shelves");
                    menu.AddUnselectableItem("Find most expensive product across all shelves");
                }
                else
                {
                    menu.AddItem("Remove a shelf");
                    menu.AddItem("View all shelves");
                    menu.AddItem("Manage a shelf");

                    if (AreThereProducts())
                    {
                        menu.AddItem("Find cheapest product across all shelves");
                        menu.AddItem("Find most expensive product across all shelves");
                        menu.AddItem("Sell a product");
                    }
                    else
                    {
                        menu.AddUnselectableItem("Find cheapest product across all shelves");
                        menu.AddUnselectableItem("Find most expensive product across all shelves");
                        menu.AddUnselectableItem("Sell a product");
                    }
                }

                menu.AddUnselectableItem("View product stock (not implemented)");
                menu.AddItem("Save inventory data");
                menu.AddItem("Reload inventory data");

                if (LaunchedProgram)
                {
                    menu.SetTopText($"Inventory Management System\n===============\nWelcome to the factory inventory management system.\nYou can edit shelves and products here.\n===============\n");
                    LaunchedProgram = false;
                }
                else menu.SetTopText($"Inventory Management System\n===============\nYou have {Inventory.Length} shelf(s) in your inventory.\nTotal Profit: {TotalProfit}\n===============\n");
                int selection = menu.DisplayMenu();

                switch (selection)
                {
                    case -1:
                        Console.WriteLine("Exiting the system. Goodbye!");
                        SaveInventoryToFile(true); // Save the inventory to a file before exiting
                        return;
                    case 0:
                        AddShelf();
                        break;
                    case 1:
                        RemoveShelf();
                        break;
                    case 2:
                        ListShelves();
                        break;
                    case 3:
                        ManageShelf();
                        break;
                    case 4:
                        FindCheapestProduct();
                        break;
                    case 5:
                        FindMostExpensiveProduct();
                        break;
                    case 6:
                        SellProduct();
                        break;
                    case 8:
                        SaveInventoryToFile(false); // Save the inventory to a file
                        break;
                    case 9:
                        Inventory = LoadInventoryFromFile(false); // Reload the inventory from the file
                        break;
                    default:
                        Console.WriteLine("This feature is not implemented yet.");
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
