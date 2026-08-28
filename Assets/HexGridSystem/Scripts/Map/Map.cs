using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map
{
    private Dictionary<Vector2Int, int> m_grid;
    private Vector2Int m_mapSize;
    private List<TileGenerationRate> m_generationRates = new();
    private float m_seed;
    private float m_scale;

    private static Map s_instance = null;

    public Vector2Int mapSize => m_mapSize;
    public Dictionary<Vector2Int, int> grid => m_grid;

    public Map(MapSettings settings)
    {
        if(s_instance != null)
        {
            Debug.LogWarning("[Map] Too many object of type Map! This one will be destroyed.");
            return;
        }
        s_instance = this;

        m_grid = new Dictionary<Vector2Int, int>();
        m_mapSize = settings.size;  
        m_generationRates = settings.tileGenerationRate;
        m_seed = UnityEngine.Random.Range(0,1000);
        m_scale = settings.scale;

        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                //Debug.Log($"[Map] Generating tile at position [{y}][{x}]");
                m_grid.Add(new Vector2Int(y, x), new Tile(NewTileSettings(y, x)).id);
            }
        }
    }

    private TileSettings NewTileSettings(int xPos, int yPos)
    {
        if (m_generationRates.Count <= 0)
        {
            Debug.LogError($"[Map/GetTileSettings] There are no generation rates!");
            return null;
        }

        float value;
        float rateMax = 0f;
        TileSettings result = null;

        foreach (TileGenerationRate rate in m_generationRates)
        {
            rateMax += rate.spawnWeight;
        }

        float x = (xPos / (float)m_mapSize.x + m_seed) / m_scale;
        float y = (yPos / (float)m_mapSize.y + m_seed) / m_scale;

        value = Mathf.Abs(Mathf.PerlinNoise(x, y));
        //Debug.Log($"[Map/GetTileSettings] Weight value: {value} | RateMax: {rateMax}");
        value *= rateMax;

        for (int i = 0, minRate = 0; i < m_generationRates.Count; i++)
        {
            int maxRate = minRate + m_generationRates[i].spawnWeight;
            //Debug.Log($"[Map/GetTileSettings] MinRate: {minRate} | MaxRate: {maxRate}");
            if (value >= minRate && value < maxRate)
            {
                result = m_generationRates[i].settings;
                break;
            }
            minRate = maxRate;
        }

        //Debug.Log($"[Map/GetTileSettings] Tile type selected: {(result != null ? result.type.ToString() : "null")}");
        return result;
    }
    public static void Reset()
    {
        GC.SuppressFinalize(s_instance);
        s_instance = null;
    }
    public bool IsCellEmpty(Vector2Int coordinates)
    {
        return !m_grid.ContainsKey(coordinates);
    }

    public static Vector2Int GetGridPositionFromId(int id)
    {
        return s_instance.m_grid.FirstOrDefault(x => x.Value == id).Key;
    }
}