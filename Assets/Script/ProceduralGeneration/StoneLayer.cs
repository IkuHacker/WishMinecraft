using UnityEngine;

public class StoneLayer : BlockLayerHandler
{
    public BlockType stoneBlockType;
    public int stoneLayerStartDepth = 2; // Starting depth for stone layer

    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        if (y >= stoneLayerStartDepth && y < surfaceHeightNoise)
        {
            Vector3Int pos = new Vector3Int(x, y, z);
            Chunk.SetBlock(chunkData, pos, stoneBlockType);
            return true;
        }
        return false;
    }
}
