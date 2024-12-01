using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemsTest : MonoBehaviour
{
    public Inventory inventory; // Link on inventory
    public InventoryUI inventoryUI; // Link on Inventory UI for Update

    // Items
    public Item item;
    public Item item2;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool wasAdded = inventory.Add(item2);
            bool wasAdded1 = inventory.Add(item);

            if (wasAdded)
            {
                inventoryUI.UpdateUI(); // Update UI inventory
                Destroy(gameObject); // Delete item from Scene
            }
        }
    }
}
