using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    public Image image;
    public Text countText;
    [HideInInspector] public Transform parentAfterDrag;
    public int count = 1;
    [HideInInspector] public ItemData item;

    private void Start()
    {
        RefreshCount();
    }

    public void InitializeItem(ItemData newItem)
    {
        item = newItem;
        image.sprite = newItem.visual;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        countText.enabled = count > 1;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerClick.name);

        if (InventoryManager.instance.IsDraggingItem())
        {
            // Si un autre item est déjà en drag, on ignore l'interaction
            return;
        }

        if (InventoryManager.instance.draggedItem == this)
        {
            // Lâcher l'item
            transform.SetParent(parentAfterDrag);
            InventoryManager.instance.StopDraggingItem();
            image.raycastTarget = true;  // Réactiver le raycastTarget après avoir lâché l'item
        }
        else
        {
            // Commencer à prendre l'item
            InventoryManager.instance.StartDraggingItem(this);
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);  // Le déplacer au niveau supérieur pour le drag
            image.raycastTarget = false;  // Désactiver le raycast pendant le drag
        }
    }

    private void Update()
    {
        if (InventoryManager.instance.draggedItem == this)
        {
            // Suivre la position de la souris pendant que l'item est pris
            transform.position = Input.mousePosition;
        }
    }
}
