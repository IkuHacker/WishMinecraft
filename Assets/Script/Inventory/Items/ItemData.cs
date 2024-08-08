using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New item")]
public class ItemData : ScriptableObject
{
    [Header("Data")]
    public string itemName;
    [TextArea(3, 10)]
    public string description;
    public Sprite visual;
    public bool isABlock;
    public BlockType blocs;
    public bool stackable;
    public int maxStack;

}
    