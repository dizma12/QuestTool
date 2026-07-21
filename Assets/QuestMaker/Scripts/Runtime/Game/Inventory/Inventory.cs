using QuestMaker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;


namespace QuestMaker.Runtime.Game
{
    public class Inventory : IInventoryReader
    {
        private readonly Dictionary<Item, int> _items = null; 
        private readonly Action _inventoryChanged = null;
        public Inventory(Action callback, Dictionary<Item, int> premadeInventory = null)
        {
            if (callback != null)
                _inventoryChanged = callback;

            if (premadeInventory == null || !ValidateInventory(premadeInventory))
            {
                _items = new(RuntimeSettings.MAX_INVENTORY_CAPACITY);
                return;
            }
            else
                _items = premadeInventory;
        }

        private bool ValidateInventory(Dictionary<Item, int> premadeInventory)
        {
            if (premadeInventory.Count > RuntimeSettings.MAX_INVENTORY_CAPACITY)
            {
                ConsoleLogger.LogError(this, $"Provided inventory exceeds the maximum capacity of: {RuntimeSettings.MAX_INVENTORY_CAPACITY}");
                return false;
            }

            foreach( var item in premadeInventory.Keys )
            {
                if(item == null || premadeInventory[item] <= 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Adds new Item to inventory. If u want to add a stack use AddItemStack.
        /// </summary>
        /// <param name="item">The item too add</param>
        /// <param name="amount">The amount of item stacks</param>
        public void AddItem(Item item, int amount)
        {
            if(_items.Count >= RuntimeSettings.MAX_INVENTORY_CAPACITY)
            {
                ConsoleLogger.LogWarning(this, $"Inventory is at max capacity ({RuntimeSettings.MAX_INVENTORY_CAPACITY}) cannot add more.");
                return;
            }
            if (item == null || amount <= 0)
            {
                ConsoleLogger.LogError(this, $"Item to Add cant be null or amount <= 0");
                return;
            }

            if (!_items.ContainsKey(item))
            {
                _items.Add(item, amount);
                ConsoleLogger.Log(this, $"Added Item: {item.Name} with amount: {amount}");
                _inventoryChanged?.Invoke();
            }
            else
                ConsoleLogger.LogError(this, $"Inventory already contains item {item.Name}, to add stack use AddItemStack");
        }

        /// <summary>
        /// Adds Stacks to item. If item does not exists tries to add it with total amount of amountToAdd
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amountToAdd"></param>
        public void AddItemStack(Item item, int amountToAdd)
        {
            //Checks
            if (item == null || amountToAdd <= 0)
            {
                ConsoleLogger.LogError(this, $"Item to Add cant be null or amount <= 0");
                return;
            }
            if (!_items.TryGetValue(item, out int currentStacks))
            {
                ConsoleLogger.Log(this, $"Inventory does not contain item {item.Name}. Trying to add new entry with total amount {amountToAdd}");
                AddItem(item, amountToAdd);
                return;
            }


            currentStacks += amountToAdd;
            _items[item] = currentStacks;
            _inventoryChanged?.Invoke();
            ConsoleLogger.Log(this, $"Succesfuly added stacks for item {item}. New amount= {currentStacks}");

        }

        /// <summary>
        /// Completely removes Item from Inventory. If you want to remove a stack use RemoveItemStack.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(Item item)
        {
            if (item == null)
            {
                ConsoleLogger.LogError(this, $"Item to Remove cant be null");
                return;
            }

            if (!_items.ContainsKey(item))
            {
                ConsoleLogger.LogError(this, $"Inventory does not contain item {item.Name}");
                return;
            }
            _items.Remove(item);
            _inventoryChanged?.Invoke();
        }

        /// <summary>
        /// Tries To remove item stacks from Item. If stacks are < 0 it removes the item from inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amountToRemove"></param>
        public void RemoveItemStack(Item item, int amountToRemove)
        {
            //Checks
            if (item == null || amountToRemove <= 0)
            {
                ConsoleLogger.LogError(this, $"Item to Remove cant be null or amount <= 0");
                return;
            }

            if (!_items.TryGetValue(item, out int currentStacks))
            {
                ConsoleLogger.LogError(this, $"Failed to retrieve item from inventory: {item.Name}");
                return;
            }

            //Remover
            currentStacks -= amountToRemove;
            if(currentStacks <= 0)
            {
                ConsoleLogger.Log(this, $"Current stacks are 0, removing {item.Name} completly");
                RemoveItem(item);
            }
            else
            {
                _items[item] = currentStacks;
                _inventoryChanged?.Invoke();
                ConsoleLogger.Log(this, $"Succesfuly removed {amountToRemove} stacks from item {item.name}");
            }
        }

        /// <summary>
        /// Adds new Item to inventory. If u want to add a stack use AddItemStack.
        /// </summary>
        public void AddItem(ItemStack item) => AddItem(item.Item, item.Amount);

        /// <summary>
        /// Adds Stacks to item. If item does not exists tries to add it with total amount of amountToAdd
        /// </summary>
        public void AddItemStack(ItemStack item) => AddItemStack(item.Item, item.Amount);

        /// <summary>
        /// Completely removes Item from Inventory. If you want to remove a stack use RemoveItemStack.
        /// </summary>
        public void RemoveItem(ItemStack item) => RemoveItem(item.Item);

        /// <summary>
        /// Tries To remove item stacks from Item. If stacks are < 0 it removes the item from inventory.
        /// </summary>
        public void RemoveItemStack(ItemStack item, int amountToRemove) => RemoveItemStack(item.Item, amountToRemove); 


        /// <summary>
        /// Retrieves all items in inventory and returns it as ItemStack[]. 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public ItemStack[] RetrieveAll()=> _items.Select(i => new ItemStack(i.Key, i.Value)).ToArray();
        

        /// <summary>
        /// Retrieves the current stack of item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>0 if item not found.</returns>
        public int GetItemCount(Item item) => item != null && _items.TryGetValue(item,out int count) ? count : 0;
        public int GetItemCount(ItemStack item) => item.Item != null && _items.TryGetValue(item.Item,out int count) ? count : 0;
    }
}
