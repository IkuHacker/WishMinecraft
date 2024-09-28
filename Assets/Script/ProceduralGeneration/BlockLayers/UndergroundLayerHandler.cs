using UnityEngine;

public class UndergroundLayerHandler : BlockLayerHandler
{
    public BlockType undergroundBlockType;
    public BlockType blockType;
    public BlockType bedrockBlockType; // Ajoutez une r�f�rence au type de bloc bedrock

    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        Vector3Int pos = new Vector3Int(x, y, z);

        // V�rifiez si c'est la derni�re couche pour placer la bedrock
        if (y == 0)  // Ou vous pouvez ajuster cette valeur pour d�finir l'�paisseur de la couche de bedrock
        {
            Chunk.SetBlock(chunkData, pos, bedrockBlockType);
            return true;
        }
        else if (y < surfaceHeightNoise - 5)
        {
            // Placer un autre bloc souterrain (blockType)
            Chunk.SetBlock(chunkData, pos, blockType);
            return true;
        }
        else if (y < surfaceHeightNoise)
        {
            // Placer le bloc souterrain standard (undergroundBlockType)
            Chunk.SetBlock(chunkData, pos, undergroundBlockType);
            return true;
        }

        return false;
    }
}
