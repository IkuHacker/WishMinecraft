using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTableManager : MonoBehaviour
{
    public ItemInventory[] itemInventory;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (InventrySlot slot in InventoryManager.instance.inventorySlots)
        {
            if(slot.transform.childCount > 1) 
            {
                slot.transform.GetChild(0).GetComponent<InventoryItem>().item
            }
        }
    }
}
