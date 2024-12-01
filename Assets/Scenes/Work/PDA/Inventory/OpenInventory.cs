using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInventory : MonoBehaviour
{
    [SerializeField] private GameObject InventoryPanel;
    private bool inventoryChecker;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && inventoryChecker == false) {
            InventoryPanel.SetActive(true);
            inventoryChecker = true;
        } else if(Input.GetKeyDown(KeyCode.E) && inventoryChecker == true) {
            InventoryPanel.SetActive(false);
            inventoryChecker = false;
        }
    }
}
