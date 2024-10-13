using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockUtilityManager : MonoBehaviour
{
    public UttilityBlock[] uttilityBlock;

    public static BlockUtilityManager instance;

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

    public bool IsAnyPanelOpen()
    {
        foreach (var block in uttilityBlock)
        {
            if (block.panel.activeSelf) // Vérifie si le panneau est actif
            {
                return true; // Si un panneau est actif, retourne true
            }
        }
        return false; // Sinon, retourne false
    }


}

[System.Serializable]
public class UttilityBlock
{
    public BlockType block;
    public BlockUtilityType blockUtility;
    public GameObject panel;

}
