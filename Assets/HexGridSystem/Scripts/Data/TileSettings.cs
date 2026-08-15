using UnityEngine;

[CreateAssetMenu(fileName = "TileDataSettings", menuName = "Map Generation/Tile Data Settings")]
public class TileSettings : ScriptableObject
{
    public TileType type;
    public Color color;
    public GameObject defaultPrefab;
}

public enum TileType
{
    Plain = 0,
    Water,
    Forest,
    Mountain,
    Beach
}
