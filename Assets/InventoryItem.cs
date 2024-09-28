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
            // Si un autre item est d�j� en drag, on ignore l'interaction
            return;
        }

        if (InventoryManager.instance.draggedItem == this)
        {
            // L�cher l'item
            transform.SetParent(parentAfterDrag);
            InventoryManager.instance.StopDraggingItem();
            image.raycastTarget = true;  // R�activer le raycastTarget apr�s avoir l�ch� l'item
        }
        else
        {
            // Commencer � prendre l'item
            InventoryManager.instance.StartDraggingItem(this);
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);  // Le d�placer au niveau sup�rieur pour le drag
            image.raycastTarget = false;  // D�sactiver le raycast pendant le drag
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
