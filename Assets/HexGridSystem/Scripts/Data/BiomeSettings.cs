using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeDefinition", menuName = "Map Generation/Biome Definition")]
public class BiomeSettings : ScriptableObject
{
    [Tooltip("The tile that dominate the biome")]
    public TileSettings mainTile;
    [Tooltip("The tile that can be found in the biome based on moisture value")]
    public List<MoistureTileRange> dynamicTiles = new();
    

    [Serializable]
    public struct MoistureTileRange
    {
        public TileSettings tile;
        [Range(0, 1)]
        public float minMoistureValue;
        [Range(0, 1)]
        public float maxMoistureValue;
    }
}
