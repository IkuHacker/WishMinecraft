using UnityEngine;
using UnityEngine.EventSystems;

public class InventrySlot : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        

        if (!InventoryManager.instance.IsDraggingItem() || eventData.button == PointerEventData.InputButton.Right)
        {
            return;
        }

        InventoryItem draggedItem = InventoryManager.instance.draggedItem;

        // Si le slot a d�j� un item, on v�rifie si on peut fusionner ou �changer
        if (transform.childCount == 1)
        {
            InventoryItem existingItem = transform.GetChild(0).GetComponent<InventoryItem>();

            // Fusion si c'est le m�me type d'item
            if (existingItem.item == draggedItem.item && existingItem != draggedItem)
            {
                HandleMerge(existingItem, draggedItem);
            }
            else
            {
                // Si les items ne peuvent pas �tre fusionn�s, on effectue un �change (swap)
                HandleSwap(existingItem, draggedItem);
            }
        }
        else
        {
            Debug.Log("La case est vide");
            PlaceItemInSlot(draggedItem);
        }
    }

    private void HandleMerge(InventoryItem existingItem, InventoryItem draggedItem)
    {
        int totalCount = existingItem.count + draggedItem.count;

        // Si le total d�passe la capacit� de stack, on ajuste
        if (totalCount > draggedItem.item.maxStack)
        {
            int remainingItems = totalCount - draggedItem.item.maxStack;
            existingItem.count = draggedItem.item.maxStack;
            existingItem.RefreshCount();

            // Mettre � jour l'item d�plac� avec les items restants
            draggedItem.count = remainingItems;
            draggedItem.RefreshCount();
            Debug.Log($"Stack maximum atteint. {remainingItems} item(s) restant(s).");
        }
        else
        {
            // Fusionner les objets
            existingItem.count = totalCount;
            existingItem.RefreshCount();

            // D�truire l'objet d�plac�, puisqu'il a �t� fusionn�
            Destroy(draggedItem.gameObject);
            InventoryManager.instance.StopDraggingItem();  // Arr�ter le drag
        }
    }

    private void HandleSwap(InventoryItem existingItem, InventoryItem draggedItem)
    {
        // Le parent de l'item actuellement en main devient temporaire
        Transform originalParent = draggedItem.parentAfterDrag;

        // L'item en main est plac� dans le slot cibl�
        draggedItem.transform.SetParent(transform);
        draggedItem.transform.localPosition = Vector3.zero;

        // L'item dans le slot passe sous la souris, en mode "drag"
        existingItem.transform.SetParent(originalParent);
        existingItem.transform.localPosition = Vector3.zero;

        // L'item qui �tait dans le slot est maintenant le nouvel item en main
        InventoryManager.instance.draggedItem = existingItem;
        existingItem.parentAfterDrag = originalParent;

        // R�activer les raycasts
        existingItem.image.raycastTarget = false;  // D�sactiver le raycast de l'item actuellement d�plac�
        draggedItem.image.raycastTarget = true;  // R�activer le raycast de l'item qui vient d'�tre plac�

        // On ne stoppe pas le drag ici, car on continue de d�placer l'autre item
    }

    private void PlaceItemInSlot(InventoryItem draggedItem)
    {
        // Placer l'objet dans le slot vide
        draggedItem.parentAfterDrag = transform;
        draggedItem.transform.SetParent(transform);
        draggedItem.transform.localPosition = Vector3.zero;  // Remettre l'item � la position locale correcte
        draggedItem.image.raycastTarget = true;
        InventoryManager.instance.StopDraggingItem();  // Arr�ter le drag
    }

    private void Update()
    {
        // Si un slot a plus d'un enfant, on s'assure qu'il n'en reste qu'un
        if (transform.childCount > 1)
        {
            for (int i = 1; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
