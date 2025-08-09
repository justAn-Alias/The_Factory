using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Factory
{
    internal class MenuBuilder
    {
        // This class is made to allow menu navigation with arrow keys.
        List<string> menuItems;
        string topText;
        int[] unselectableIndexes = new int[0]; // This will hold the indexes of unselectable items just to be able to skip them when the user tries to select them.
        int currentSelectionIndex;

        public MenuBuilder(string topText) // Create a menu with a top text
        {
            this.topText = topText;
            menuItems = new List<string>();
        }

        public void SetTopText(string text) // Set the top text of the menu
        {
            this.topText = text;
        }
        public string GetTopText() // Get the top text of the menu
        {
            return this.topText;
        }

        public void SetCurrentSelectionIndex(int index) // Set the current selection index
        {
            if (index >= 0 && index < menuItems.Count)
            {
                currentSelectionIndex = index;
            }
        }
        public int GetCurrentSelectionIndex() // Get the current selection index
        {
            return currentSelectionIndex;
        }
        public string GetCurrentSelection()
        {
            if (currentSelectionIndex >= 0 && currentSelectionIndex < menuItems.Count)
            {
                return menuItems[currentSelectionIndex];
            }
            return string.Empty; // Return empty string if index is out of bounds (shouldn't happen, but just in case)
        }

        public void AddItem(string item)
        {
            if (!string.IsNullOrWhiteSpace(item))
            {
                menuItems.Add(item);
            }
        }

        public void AddOrderedItem(string item, int position)
        {
            if (!string.IsNullOrWhiteSpace(item) && position >= 0 && position <= menuItems.Count)
            {
                menuItems.Insert(position, item);
            }

            // Shift unselectable indexes if necessary
            for (int i = 0; i < unselectableIndexes.Length; i++)
            {
                if (unselectableIndexes[i] >= position)
                {
                    unselectableIndexes[i]++;
                }
            }
        }

        public void AddUnselectableItem(string item)
        {
            // This method adds an item that cannot be selected, but is still displayed in the menu.
            // This will just be skipped when the user tries to select it.
            if (!string.IsNullOrWhiteSpace(item))
            {
                menuItems.Add(item);
                Array.Resize(ref unselectableIndexes, unselectableIndexes.Length + 1);
                unselectableIndexes[unselectableIndexes.Length - 1] = menuItems.Count - 1; // Store the index of the unselectable item
            }
        }

        public void RemoveItem(string item)
        {
            if (menuItems.Contains(item))
            {
                menuItems.Remove(item);

                // Check if the removed item was unselectable and adjust the unselectableIndexes accordingly
                int index = menuItems.IndexOf(item);
                if (index != -1)
                {
                    unselectableIndexes = unselectableIndexes.Where(i => i != index).ToArray(); // Remove the index from unselectableIndexes
                }
                menuItems.RemoveAt(index);
            }
        }

        public void ClearMenu()
        {
            menuItems.Clear();
            unselectableIndexes = new int[0]; // Reset unselectable indexes
            currentSelectionIndex = 0; // Reset current selection index
        }

        public void UpdateGUI()
        {
            Console.Clear();
            Console.WriteLine(topText);

            for (int i = 0; i < menuItems.Count; i++)
            {
                if (i == currentSelectionIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White; // Highlight the selected item
                }
                else if (unselectableIndexes.Contains(i))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    if (i == currentSelectionIndex) // supposedly this is never supposed to get to this point, but just in case
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black; // Normal background for unselectable items
                    }
                }
                else
                {
                    Console.ResetColor(); // Reset color for other items
                }
                Console.WriteLine($"{menuItems[i]}");
            }

            Console.ResetColor(); // Reset color after displaying the menu
            Console.WriteLine("\nUse the arrow keys to navigate, Enter to select, and Esc to exit.");
        }

        // Every menu is built differently so we don't need a method to clear the menu.
        public int DisplayMenu()
        {
            UpdateGUI();
            // Wait for input
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow)
                {
                    // Move up in the menu
                    do
                    {
                        currentSelectionIndex = (currentSelectionIndex - 1 + menuItems.Count) % menuItems.Count;
                    } while (unselectableIndexes.Contains(currentSelectionIndex));
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    // Move down in the menu
                    do
                    {
                        currentSelectionIndex = (currentSelectionIndex + 1) % menuItems.Count;
                    } while (unselectableIndexes.Contains(currentSelectionIndex));
                }
                else if (key == ConsoleKey.Enter)
                {
                    // Ignore if the current selection is unselectable (despite not being able to be selected, but you never know.)
                    if (unselectableIndexes.Contains(currentSelectionIndex))
                        continue;

                    Console.ReadLine();//wait until enter is pressed again to avoid enter being buffered into the selection - only once per menu
                    return currentSelectionIndex;
                }
                else if (key == ConsoleKey.Escape)
                {
                    // Exit the menu
                    return -1; // Indicate exit
                }

                UpdateGUI(); // Refresh the menu display after each key press
            }
        }
    }
}
