using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventrySlot[] inventorySlots; // Tableau des slots d'inventaire
    public ItemData[] blockList; // Tableau des items
    public GameObject blockListSlotPrefab;
    public Transform blockListContent;

    public GameObject inventoryItemPrefab; // Le prefab de l'item d'inventaire
    public GameObject inventoryUI; // L'UI de l'inventaire à afficher/masquer
    public ItemData itemToAdd; // L'item qui sera ajouté en appuyant sur 'A'

    [HideInInspector]public bool isInventoryOpen = false; // L'état de l'inventaire (ouvert/fermé)

    public static InventoryManager instance;

    [HideInInspector] public InventoryItem draggedItem = null;

    public World world;

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

    private void Start()
    {
        InitializedBlockList();
    }

    public void InitializedBlockList()
    {
        foreach (ItemData item in blockList)
        {
            GameObject currentBlockData = Instantiate(blockListSlotPrefab, blockListContent);
            BlockListSlot blockListSlot = currentBlockData.GetComponent<BlockListSlot>();
            blockListSlot.item = item;

            blockListSlot.InitializedImage();
        }
    }

    void Update()
    {
        // Ouvrir/fermer l'inventaire avec 'E' ou 'Tab'
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }


        // Si un item est en cours de drag, il suit la souris
        if (draggedItem != null)
        {
            draggedItem.transform.position = Input.mousePosition; // Suivre la position de la souris
        }


        if (world.IsWorldCreated) 
        {
            if (isInventoryOpen)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
      
    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen); // Activer ou désactiver l'UI de l'inventaire
    }

    public bool AddItem(ItemData item)
    {
        // Vérifier si un slot contient déjà cet item et si on peut le stacker
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventrySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && item.stackable && itemInSlot.count < item.maxStack)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        // Trouver un slot vide pour y ajouter l'item
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventrySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false; // Si l'inventaire est plein
    }

    void SpawnNewItem(ItemData item, InventrySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform); // Instancier le nouvel item
        InventoryItem newItem = newItemGo.GetComponent<InventoryItem>();
        newItem.InitializeItem(item); // Initialiser le nouvel item avec les données fournies
    }

    // Méthode pour créer l'item directement dans la main (drag automatique)
    public void SpawnItemInHand(ItemData item)
    {
        Debug.Log("item dans la main");
        // Instancier l'item sous l'InventoryManager
        GameObject newItemGo = Instantiate(inventoryItemPrefab, transform);
        newItemGo.GetComponent<RectTransform>().sizeDelta = new Vector3(30, 30);

        InventoryItem newItem = newItemGo.GetComponent<InventoryItem>();

        // Initialiser l'item avec les données fournies
        newItem.InitializeItem(item);

        // Assurer que l'item soit bien visible
        newItem.transform.SetParent(inventoryUI.transform, false); // S'assurer que l'item soit bien dans l'UI
        newItem.transform.SetAsLastSibling(); // S'assurer que l'item soit rendu au-dessus des autres éléments de l'UI

        // Désactiver le raycast pour l'item actuellement déplacé
        newItem.image.raycastTarget = false;

        // Commencer le drag
        StartDraggingItem(newItem);


        newItem.transform.position = Input.mousePosition;

        newItemGo.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

    }

    public bool IsDraggingItem()
    {
        return draggedItem != null;
    }

    public void StartDraggingItem(InventoryItem item)
    {
        draggedItem = item;
    }

    public void StopDraggingItem()
    {
        draggedItem = null;
    }
}
