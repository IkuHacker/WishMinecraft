using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemRecipe/InventoryRecipe")]
public class InventoryRecipe : ScriptableObject
{
    public ItemInventory output;

    public ItemData item_00;
    public ItemData item_01;
    public ItemData item_10;

    public ItemData item_11;
 


    public ItemData GetItem(int x, int y)
    {
        if (x == 0 && y == 0) return item_00;
        if (x == 0 && y == 1) return item_01;
        if (x == 1 && y == 0) return item_10;

        if (x == 1 && y == 1) return item_11;
      

        return null;
    }
}

[System.Serializable]
public class ItemInventory
{
    public ItemData item;
    public int count;

}
