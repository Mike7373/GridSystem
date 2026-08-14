using UnityEngine;

public class TileComponent : MonoBehaviour
{
    private TileBodyComponent m_tileBody;
    private TileBorderComponent m_tileBorder;
    private Vector3 m_startingTransformPosition;

    public void Initialize(Color color, Vector3 startingTransformPosition)
    {
        m_tileBody = GetComponentInChildren<TileBodyComponent>();
        m_tileBorder = GetComponentInChildren<TileBorderComponent>();
        m_tileBody.SetColor(color);
        m_startingTransformPosition = startingTransformPosition;
    }
}
