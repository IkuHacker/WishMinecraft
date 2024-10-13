using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCraft : MonoBehaviour
{
    private ItemData item_00;
    private ItemData item_01;
    private ItemData item_10;
    private ItemData item_11;

    public Transform InventoryCraftContent;
    public GameObject inventoryItemPrefab;
    public Transform outputSlot;

    public InventoryRecipe[] recipes;  // Liste des recettes disponibles
    private ItemInventory currentOutput; // Résultat du craft

    [HideInInspector]public bool itemTakenFromOutput = true;
    private GameObject outputGameObject;

    private InventoryItem[] craftItems = new InventoryItem[4]; 
    // Tableau pour stocker les items du craft
    public static InventoryCraft instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        // Initialize crafting slots
        UpdateCraftingItems();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCraftingItems();
        CheckCrafting();

        // Vérifie si l'item dans l'output a été pris
        if (itemTakenFromOutput)
        {
            ReduceCraftingItems();  // Réduit les items utilisés
            currentOutput = null;   // Réinitialise l'output
            itemTakenFromOutput = false;
        }

        if (currentOutput == null) 
        {
            Destroy(outputGameObject);
        }
    }

    // Met à jour les items du craft en récupérant les enfants des slots
    void UpdateCraftingItems()
    {
        craftItems[0] = GetInventoryItemFromChild(InventoryCraftContent, 2);
        craftItems[1] = GetInventoryItemFromChild(InventoryCraftContent, 3);
        craftItems[2] = GetInventoryItemFromChild(InventoryCraftContent, 0);
        craftItems[3] = GetInventoryItemFromChild(InventoryCraftContent, 1);

        // Mise à jour des ItemData correspondants
        item_00 = craftItems[0] != null ? craftItems[0].item : null;
        item_01 = craftItems[1] != null ? craftItems[1].item : null;
        item_10 = craftItems[2] != null ? craftItems[2].item : null;
        item_11 = craftItems[3] != null ? craftItems[3].item : null;
    }

    // Récupère l'InventoryItem dans l'enfant spécifié
    InventoryItem GetInventoryItemFromChild(Transform parent, int index)
    {
        if (parent.childCount > index)  // Vérifie si le parent a assez d'enfants
        {
            Transform child = parent.GetChild(index);
            if (child.childCount > 0)  // Vérifie si l'enfant a aussi un enfant
            {
                Transform innerChild = child.GetChild(0);
                InventoryItem inventoryItem = innerChild.GetComponent<InventoryItem>();
                if (inventoryItem != null)  // Vérifie si le composant InventoryItem existe
                {
                    return inventoryItem;
                }
            }
        }
        return null;  // Retourne null si quelque chose est manquant
    }

    void SpawnNewItem(ItemData item, int count)
    {
        if (outputSlot.childCount == 0)
        {
            GameObject newItemGo = Instantiate(inventoryItemPrefab, outputSlot);
            outputGameObject = newItemGo;
            InventoryItem newItem = newItemGo.GetComponent<InventoryItem>();
            newItem.count = count;
            newItem.InitializeItem(item);
            newItem.RefreshCount();
            newItemGo.GetComponent<RectTransform>().sizeDelta = new Vector3(30, 30);
        }
    }

    void CheckCrafting()
    {
        currentOutput = null;  // Reset output

        foreach (InventoryRecipe recipe in recipes)
        {
            // Vérifie si les items correspondent à la recette
            if (item_00 == recipe.item_00 &&
                item_01 == recipe.item_01 &&
                item_10 == recipe.item_10 &&
                item_11 == recipe.item_11)
            {
                currentOutput = recipe.output;
                SpawnNewItem(currentOutput.item, currentOutput.count);
                break;  // Arrête la boucle si une recette correspond
            }
        }
    }

    // Fonction pour réduire la quantité des items utilisés dans le craft
    void ReduceCraftingItems()
    {
        for (int i = 0; i < craftItems.Length; i++)
        {
            if (craftItems[i] != null)
            {
                craftItems[i].count--;  // Réduit la quantité de 1
                craftItems[i].RefreshCount();  // Met à jour l'affichage de la quantité

                if (craftItems[i].count <= 0)
                {
                    Destroy(craftItems[i].gameObject);  // Supprime l'item si la quantité tombe à 0
                }
            }
        }
    }
}
