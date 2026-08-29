using System;
using System.Collections.Generic;
using UnityEngine;
using static BiomeSettings;

[CreateAssetMenu(fileName = "TileDataSettings", menuName = "Map Generation/Tile Definition")]
public class TileSettings : ScriptableObject
{
    public TileType type;
    public Color color;
    [Tooltip("The decoration prefab that can be found in the biome based on percentage")]
    public List<PrefabSpawnRate> dynamicPrefabs = new();
    [Range(0, 100), Tooltip("% of chance that a dynamic prefab spawns")]
    public int dynamicPrefabsSpawnRate;
}

[Serializable]
public struct PrefabSpawnRate
{
    [Tooltip("Decoration prefab")]
    public GameObject prefab;
    [Range(0, 1), Tooltip("Spawn weight of this prefab")]
    public int spawnWeight;
}
public enum TileType
{
    Plain = 0,
    Water,
    Forest,
    Mountain,
    Sand
}
