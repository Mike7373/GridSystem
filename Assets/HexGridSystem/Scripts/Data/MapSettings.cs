using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapSettings", menuName = "Map Generation/Map Data")]
public class MapSettings : ScriptableObject
{
    [Header("Map Generation Settings")]
    public Vector2Int size;
    [Range(0,100)]public float seed;
    [Range(0,10)]public float scale;
    [Header("Tile Generation Settings")]
    public List<TileGenerationRate> tileGenerationRate;
    public GameObject tilePrefab;
    public bool isTileTopFlat = true;
}
