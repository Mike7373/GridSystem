using UnityEngine;

public class TileComponent : MonoBehaviour
{
    private TileSettings m_tileDataSettings;
    private TileBodyComponent m_tileBody;
    private TileBorderComponent m_tileBorder;

    private void Start()
    {
        InitializeComponents();
    }
    private void InitializeComponents()
    {
        m_tileBody = GetComponentInChildren<TileBodyComponent>();
        m_tileBorder = GetComponentInChildren<TileBorderComponent>();
    }
    public void SetDataSettings(TileSettings settings)
    {
        m_tileDataSettings = settings;
    }
}
