using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemRecipe/CraftingTableRecipe")]
public class RecipeScriptableObject : ScriptableObject {

    public ItemData output;

    public ItemData item_00;
    public ItemData item_10;
    public ItemData item_20;

    public ItemData item_01;
    public ItemData item_11;
    public ItemData item_21;

    public ItemData item_02;
    public ItemData item_12;
    public ItemData item_22;


    public ItemData GetItem(int x, int y) {
        if (x == 0 && y == 0) return item_00;
        if (x == 1 && y == 0) return item_10;
        if (x == 2 && y == 0) return item_20;

        if (x == 0 && y == 1) return item_01;
        if (x == 1 && y == 1) return item_11;
        if (x == 2 && y == 1) return item_21;

        if (x == 0 && y == 2) return item_02;
        if (x == 1 && y == 2) return item_12;
        if (x == 2 && y == 2) return item_22;

        return null;
    }


}
