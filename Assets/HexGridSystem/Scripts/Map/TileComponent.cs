using UnityEngine;

public class TileComponent : MonoBehaviour
{
    private TileBodyComponent m_tileBody;
    private TileBorderComponent m_tileBorder;

    public void Initialize(Color color)
    {
        m_tileBody = GetComponentInChildren<TileBodyComponent>();
        m_tileBorder = GetComponentInChildren<TileBorderComponent>();
        m_tileBody.SetColor(color);
    }


}
