using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapSettings", menuName = "Map Generation/Map Data")]
public class MapSettings : ScriptableObject
{
    [Header("Map Generation Settings")]
    public Vector2Int mapSize;
    public List<TileGenerationRate> tileGenerationRate;
    public string seed;
    [Header("Tile Generation Settings")]
    public GameObject tilePrefab;
    public bool isTileTopFlat = true;
}
