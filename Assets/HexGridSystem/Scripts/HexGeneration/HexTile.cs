using UnityEngine;

public class HexTile : MonoBehaviour
{
    private TileDataSettings _tileDataSettings;
    private HexTileBody _tileBody;
    private HexTileBorder _tileBorder;

    private void Start()
    {
        InitializeComponents();
    }
    private void InitializeComponents()
    {
        _tileBody = GetComponentInChildren<HexTileBody>();
        _tileBorder = GetComponentInChildren<HexTileBorder>();
    }
    public void SetDataSettings(TileDataSettings settings)
    {
        _tileDataSettings = settings;
    }
}
