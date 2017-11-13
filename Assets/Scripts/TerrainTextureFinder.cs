using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TerrainTextureFinder {

    // Returns array containing mix of textures on terrain at position
	public static float[] GetTextureMix(Terrain terrain, Vector3 pos)
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;

        // Get splat map cell the position falls in
        int mapX = (int)(((pos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
        int mapZ = (int)(((pos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);
        // Get splat data for this cell
        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
        //TODO simpler way to convert to 1D? Does "float[] cellmix = splatmapData[0,0]" make sense?
        float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];
        for(int n = 0; n<cellMix.Length; ++n)
        {
            cellMix[n] = splatmapData[0, 0, n];
        }
        return cellMix;
    }

    public static int GetMainTexture(Terrain terrain, Vector3 pos)
    {
        float[] mix = GetTextureMix(terrain, pos);
        float max = 0;
        int maxIndex = 0;
        for(int n = 0; n < mix.Length; ++n)
        {
            if(mix[n] > max)
            {
                max = mix[n];
                maxIndex = n;
            }
        }
        return maxIndex;
    }
}
