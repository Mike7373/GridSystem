using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapSettings", menuName = "Map Generation/Map Settings")]
public class MapSettings : ScriptableObject
{
    [Header("Map Generation Settings")]
    public Vector2Int size;
    [Range(0,10)]public float scale;
    [Header("Tile Generation Settings")]
    public List<TileGenerationRate> tileGenerationRate;
    public GameObject tilePrefab;
    public bool isTileTopFlat = true;
}
