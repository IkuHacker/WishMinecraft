using System.Collections.Generic;
using UnityEngine;

public class HotBarManager : MonoBehaviour
{
    public List<ItemData> hotBarContent = new List<ItemData>();
    public List<InventrySlot> hotBarSlot = new List<InventrySlot>();

    public Transform content;

    private int selectedSlotIndex = 0; // Initialis� � 0 (le premier slot)
    public Transform selector;

    public ItemData[] items;

    [HideInInspector]
    public ItemData currentItem;

    public Sprite invisiblePixel;

    void Start()
    {
        RefreshContent();
        UpdateSelectorPosition(); // Met � jour la position du s�lecteur pour l'initialisation
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
        // On peuple le visuel des slots selon le contenu r�el de l'inventaire
        for (int i = 0; i < hotBarContent.Count; i++)
        {
            Slot currentSlot = content.GetChild(i).GetComponent<Slot>();

            currentSlot.item = hotBarContent[i];
            if (hotBarContent[i] == null)
            {
                currentSlot.item = null;
                currentSlot.itemVisual.sprite = invisiblePixel;
            }
            else
            {
                currentSlot.item = hotBarContent[i];
                currentSlot.itemVisual.sprite = hotBarContent[i].visual;
            }
        }
    }

    private void Update()
    {
        HandleNumberKeySelection();  // Ajout de la gestion des touches num�riques et pav� num�rique
        Scroll();                    // Gestion du d�filement � la molette
        UpdateHotBarContent();        // Mise � jour du contenu de la hotbar
        RefreshContent();
    }

    // Gestion des touches num�riques (1-9) et du pav� num�rique (Keypad1-Keypad9)
    private void HandleNumberKeySelection()
    {
        // V�rifier les touches du clavier num�rique classique (Alpha1 � Alpha9)
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + (i - 1)) || Input.GetKeyDown(KeyCode.Keypad1 + (i - 1))) // Alpha et Keypad
            {
                selectedSlotIndex = i - 1; // Les slots sont index�s de 0 � 8
                UpdateSelectorPosition();  // Mettre � jour la position du s�lecteur
                currentItem = content.GetChild(selectedSlotIndex).GetComponent<Slot>().item; // Mettre � jour l'item s�lectionn�
            }
        }
    }

    // Gestion de la molette pour faire d�filer les slots de la hotbar
    public void Scroll()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            if (selectedSlotIndex > 0) { selectedSlotIndex--; }
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            if (selectedSlotIndex < hotBarSlot.Count - 1) { selectedSlotIndex++; }
        }

        UpdateSelectorPosition(); // Mettre � jour la position du s�lecteur apr�s d�filement
        currentItem = content.GetChild(selectedSlotIndex).GetComponent<Slot>().item;
    }

    // Mise � jour du contenu de la hotbar pour chaque slot
    private void UpdateHotBarContent()
    {
        for (int i = 0; i < hotBarSlot.Count; i++)
        {
            if (hotBarSlot[i].transform.childCount > 0)
            {
                // Si le slot a un enfant (item), on r�cup�re l'item
                hotBarContent[i] = hotBarSlot[i].transform.GetChild(0).GetComponent<InventoryItem>().item;
            }
            else
            {
                // Si le slot est vide, on affecte null
                hotBarContent[i] = null;
            }
        }
    }

    // Mise � jour de la position du s�lecteur visuel
    private void UpdateSelectorPosition()
    {
        selector.position = new Vector3(
            content.GetChild(selectedSlotIndex).transform.position.x + -1f, // Ajuste la position en x
            3.3f, // Position y fixe
            0f); // Position z fixe
    }
}
