using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeDefinition", menuName = "Map Generation/Biome Definition")]
public class BiomeSettings : ScriptableObject
{
    [Tooltip("The tile that dominate the biome")]
    public TileSettings mainTile;
    [Tooltip("The tile that can be found in the biome based on moisture value")]
    public List<MoistureTileRange> dynamicTiles;
    [Tooltip("The decoration prefab that can be found in the biome based on percentage")]
    public List<PrefabSpawnSettings> dynamicPrefabs;


    //private void OnValidate()
    //{
    //    bool isFixNeeded = false;
    //    MoistureTileRange tileToFix;
    //    float fixMinValue = -1;
    //    float fixMaxValue = -1;
    //    for (int i = 0; i < dynamicTiles.Count; i++)
    //    {
    //        if (dynamicTiles[i].minMoistureValue > dynamicTiles[i].maxMoistureValue)
    //        {
    //            tileToFix = dynamicTiles[i];
    //            fixMinValue = dynamicTiles[i].maxMoistureValue;
    //            isFixNeeded = true;
    //            break;
    //        }
    //        if (dynamicTiles[i].maxMoistureValue < dynamicTiles[i].minMoistureValue)
    //        {
    //            tileToFix = dynamicTiles[i];
    //            fixMaxValue = dynamicTiles[i].minMoistureValue;
    //            isFixNeeded = true;
    //            break;
    //        }
    //    }
    //    if (!isFixNeeded) return;

    //}

    [Serializable]
    public struct MoistureTileRange
    {
        public TileSettings tile;
        [Range(0, 1)]
        public float minMoistureValue;
        [Range(0, 1)]
        public float maxMoistureValue;
    }
    [Serializable]
    public struct PrefabSpawnSettings
    {
        [Tooltip("Decoration prefab")]
        public GameObject prefab;
        [Tooltip("The tile where the prefab can spawn on")]
        public TileSettings tile;
        [Range(0, 1), Tooltip("Decorator spawn rate on the assigned tile")]
        public float percentage;
    }
}
