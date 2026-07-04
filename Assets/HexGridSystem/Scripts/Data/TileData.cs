using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "TileGeneration/TileData")]
public class TileData : ScriptableObject
{
    [Header("Generics")]
    public TileType tileType;
    [Header("Aspect")]
    public Color color;
    public GameObject defaultPrefab;
}

public enum TileType
{
    Plain = 0,
    Water,
    Forest,
    Mountain,
    Desert
}
