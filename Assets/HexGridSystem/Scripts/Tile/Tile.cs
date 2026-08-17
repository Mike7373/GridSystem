using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private int m_id;
    private TileType m_type;
    private Color m_color;

    private static List<Tile> s_instances = new();
    private static int s_instanceCount = 0;
    private static GameObject s_defaultPrefab = null;

    private static Action onReset;

    public int id => m_id;
    public TileType type => m_type;
    public Color color => m_color;
    public static GameObject defaultPrefab => s_defaultPrefab;
    public Tile(TileSettings settings)
    {
        onReset += KillInstance;

        if(s_defaultPrefab == null)
        {
            s_defaultPrefab = settings.defaultPrefab;
        }

        m_type = settings.type;
        m_color = settings.color;
        m_id = s_instanceCount++;
        
        s_instances.Add(this);
    }

    public void KillInstance()
    {
        onReset -= KillInstance;
        GC.SuppressFinalize(this);
    }

    public static Tile GetTileById(int id)
    {
        return s_instances.Find(x => x.m_id == id);
    }

    public static void Reset()
    {
        onReset.Invoke();
        s_instances.Clear();
        s_instanceCount = 0;
    }
}
