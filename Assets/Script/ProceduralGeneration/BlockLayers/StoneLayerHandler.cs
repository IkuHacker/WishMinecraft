using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneLayerHandler : BlockLayerHandler
{
    [Range(0, 1)]
    public float stoneThreshold = 0.5f;
    public BlockType biomeBlock;
    public BlockType bedrockBlock; // Ajoutez cette ligne pour référencer le bloc de bedrock

    [SerializeField]
    private NoiseSettings stoneNoiseSettings;

    public DomainWarping domainWarping;

    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        if (chunkData.worldPosition.y > surfaceHeightNoise)
            return false;

        stoneNoiseSettings.worldOffset = mapSeedOffset;
        float stoneNoise = domainWarping.GenerateDomainNoise(chunkData.worldPosition.x + x, chunkData.worldPosition.z + z, stoneNoiseSettings);
        int endPosition = surfaceHeightNoise;
        if (chunkData.worldPosition.y < 0)
        {
            endPosition = chunkData.worldPosition.y + chunkData.chunkHeight;
        }

        if (stoneNoise > stoneThreshold)
        {
            for (int i = chunkData.worldPosition.y; i <= endPosition; i++)
            {
                Vector3Int pos = new Vector3Int(x, i, z);
                if (i == chunkData.worldPosition.y) // Si c'est la dernière couche, placez de la bedrock
                {
                    Chunk.SetBlock(chunkData, pos, bedrockBlock);
                }
                else
                {
                    Chunk.SetBlock(chunkData, pos, biomeBlock);
                }
            }
            return true;
        }
        return false;
    }
}
