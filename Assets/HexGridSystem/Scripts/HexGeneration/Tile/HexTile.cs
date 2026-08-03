using UnityEngine;

public class HexTile : MonoBehaviour
{
    private TileDataSettings m_tileDataSettings;
    private HexTileBody m_tileBody;
    private HexTileBorder m_tileBorder;

    private void Start()
    {
        InitializeComponents();
    }
    private void InitializeComponents()
    {
        m_tileBody = GetComponentInChildren<HexTileBody>();
        m_tileBorder = GetComponentInChildren<HexTileBorder>();
    }
    public void SetDataSettings(TileDataSettings settings)
    {
        m_tileDataSettings = settings;
    }
}
