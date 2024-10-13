using UnityEngine;
using UnityEngine.UI;


public class BlockListSlot : MonoBehaviour
{
    public ItemData item;
    public Image image;

    public void InitializedImage() 
    {
        image.sprite = item.visual;
    }
    
    public void SpawnItemInHand() 
    {
        InventoryManager.instance.SpawnItemInHand(item, true);
    }
    
}
