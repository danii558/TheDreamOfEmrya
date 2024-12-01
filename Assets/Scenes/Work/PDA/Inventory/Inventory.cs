// Inventory.cs
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public int space = 20; // max items in Inventory

    // Add items in inventory
    public bool Add(Item item)
    {
        if (items.Count >= space)
        {
            Debug.Log("Не хватает места в инвентаре");
            return false; // inventory full
        }
        items.Add(item);
        Debug.Log(item.itemName + " добавлен в инвентарь");
        return true;
    }

    // Delete items from inventory
    public void Remove(Item item)
    {
        items.Remove(item);
        Debug.Log(item.itemName + " удален из инвентаря");
    }
}
