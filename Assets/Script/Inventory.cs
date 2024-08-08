using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int selectedSlotIndex;
    public Slot[] hotbarSlots;
    public List<ItemData> hotBarContent = new List<ItemData>();
    public ItemData currentItem;
    public Transform selector;
    public Transform content;


    void Start()
    {
        RefreshContent();
    }
    public void AddItem(ItemData item) 
    {
        hotBarContent.Add(item);
        RefreshContent();
    }

    public void RemoveItem(ItemData item)
    {
        hotBarContent.Remove(item);
        RefreshContent();

    }
    public void RefreshContent()
    {
       
        // On peuple le visuel des slots selon le contenu réel de l'inventaire
        for (int i = 0; i < hotBarContent.Count; i++)
        {
            Slot currentSlot = content.GetChild(i).GetComponent<Slot>();

            currentSlot.item = hotBarContent[i];
            currentSlot.itemVisual.sprite = hotBarContent[i].visual;
        }


    }
    void Update()
    {
        Scroll();

    }



    public void Scroll() 
    {
        if (Input.mouseScrollDelta.y > 0)
        {

            if (selectedSlotIndex > 0) { selectedSlotIndex--; }

        }
        else if (Input.mouseScrollDelta.y < 0)
        {

            if (selectedSlotIndex < 8) { selectedSlotIndex++; }

        }

        selector.position = new Vector3(hotbarSlots[selectedSlotIndex].transform.position.x + -1f, 3.3f, 0f);
        currentItem = hotbarSlots[selectedSlotIndex].item;


    }

     
}
