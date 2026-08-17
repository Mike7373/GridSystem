using System;
using System.Collections.Generic;
using UnityEngine;

public class TileComponent : MonoBehaviour
{
    private TileBodyComponent m_tileBody;
    private TileBorderComponent m_tileBorder;
    private int m_tileId;
    private static List<TileComponent> s_instances = new();

    public static event Action<Tile> onTileClicked;
    private static Action onReset;
    public int TileId => m_tileId;

    private void Awake()
    {
        s_instances.Add(this);
    }
    private void OnEnable()
    {
        onReset += KillInstance;

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
        onReset -= KillInstance;

        if (m_tileBody != null && m_tileBorder != null)
        {
            m_tileBody.onClick -= ShowTileData;
            m_tileBody.onClick -= m_tileBorder.TrySelect;
            m_tileBody.onFocus -= m_tileBorder.TryFocus;
            m_tileBody.onUnfocus -= m_tileBorder.TryHide;
        }
    }
    public void KillInstance()
    {
        Destroy(gameObject);
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

    public static void Reset()
    {
        onReset?.Invoke();
        //for(int i =0; i < s_instances.Count; i++)
        //{
        //    Destroy(s_instances[i].gameObject);
        //}
        s_instances.Clear();
    }
}
