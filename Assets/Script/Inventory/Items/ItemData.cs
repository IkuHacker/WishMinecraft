using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New item")]
public class ItemData : ScriptableObject
{
    [Header("Data")]
    public string itemName;
    [TextArea(3, 10)]
    public string description;

    [Header("Visual")]
    public Sprite visual;
    public Texture visualOfParticle;

    [Header("Block")]
    public bool isABlock;
    public BlockType blocs;

    public bool stackable;
    public int maxStack;

    [Header("Type")]
    public Type type;

    public enum Type
    {
        Block,
        Tool,
        Consumable,
        Resource,

    }



}
