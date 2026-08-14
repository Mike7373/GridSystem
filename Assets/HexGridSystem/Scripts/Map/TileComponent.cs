using UnityEngine;

public class TileComponent : MonoBehaviour
{
    private TileBodyComponent m_tileBody;
    private TileBorderComponent m_tileBorder;
    private int m_tileId;

    public delegate void TileComponentEvent(Tile tile); 
    public static event TileComponentEvent onTileClicked;
    public int TileId => m_tileId;

    private void OnEnable()
    {
        if(m_tileBody != null && m_tileBorder != null)
        {
            m_tileBody.onClick += ShowTileData;
            m_tileBody.onClick += m_tileBorder.TrySelect;
            m_tileBody.onFocus += m_tileBorder.TryFocus;
            m_tileBody.onUnfocus += m_tileBorder.TryHide;
        }
    }
    private void OnDisable()
    {
        if (m_tileBody != null && m_tileBorder != null)
        {
            m_tileBody.onClick -= ShowTileData;
            m_tileBody.onClick -= m_tileBorder.TrySelect;
            m_tileBody.onFocus -= m_tileBorder.TryFocus;
            m_tileBody.onUnfocus -= m_tileBorder.TryHide;
        }
    }
    public void Initialize(Color color, Vector3 startingTransformPosition, int id)
    {
        m_tileBody = GetComponentInChildren<TileBodyComponent>();
        m_tileBorder = GetComponentInChildren<TileBorderComponent>();
        m_tileBody.SetColor(color);
        m_tileId = id;

        if (m_tileBody != null && m_tileBorder != null)
        {
            m_tileBody.onClick += ShowTileData;
            m_tileBody.onClick += m_tileBorder.TrySelect;
            m_tileBody.onFocus += m_tileBorder.TryFocus;
            m_tileBody.onUnfocus += m_tileBorder.TryHide;
        }
    }

    public void ShowTileData()
    {
        onTileClicked?.Invoke(Tile.GetTileById(m_tileId));
    }
}
