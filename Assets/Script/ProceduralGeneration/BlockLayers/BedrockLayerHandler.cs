using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedrockLayerHandler : BlockLayerHandler
{
    public BlockType bedrockBlockType;
    public int bedrockDepth = 1; // Depth of the bedrock layer from the bottom

    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        if (y < bedrockDepth)
        {
            Vector3Int pos = new Vector3Int(x, y, z);
            Chunk.SetBlock(chunkData, pos, bedrockBlockType);
            return true;
        }
        return false;
    }
}
