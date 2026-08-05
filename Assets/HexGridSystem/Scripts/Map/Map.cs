using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private Dictionary<Vector2Int, int> m_grid;
    private List<TileGenerationRate> m_generationRates = new();
    private int m_seed;
    private Vector2Int m_mapSize;

    public Vector2Int mapSize => m_mapSize;
    public Dictionary<Vector2Int, int> grid => m_grid;

    public Map(Vector2Int mapSize, List<TileGenerationRate> generationRates)
    {
        m_grid = new Dictionary<Vector2Int, int>();
        m_generationRates = generationRates;
        m_seed = Random.Range(0, int.MaxValue);
        m_mapSize = mapSize;
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                Debug.Log($"[Map] Generating tile at position [{y}][{x}]");
                m_grid.Add(new Vector2Int(x, y), new Tile(NewTileSettings(x, y)).id);
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

        float value = Mathf.PerlinNoise(xPos + m_seed, yPos + m_seed);
        int rateMax = 0;
        TileSettings result = null;

        foreach (TileGenerationRate rate in m_generationRates)
        {
            rateMax += rate.spawnWeight;
        }
        
        value *= rateMax;

        for(int i = 0, minRate = 0; i < m_generationRates.Count; i++)
        {
            int maxRate = minRate + m_generationRates[i].spawnWeight;
            if(value >= minRate && value < maxRate)
            {
                result = m_generationRates[i].settings;
                break;
            }
            minRate = maxRate;
        }

        Debug.Log($"[Map/GetTileSettings] Tile type selected: {result.type.ToString()}");
        return result;
    }

    public bool IsCellEmpty(Vector2Int coordinates)
    {
        return !m_grid.ContainsKey(coordinates);
    }
}