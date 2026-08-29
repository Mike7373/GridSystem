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
    private static List<PrefabSpawnRate> s_dynamicPrefabs = null;

    private static Action onReset;

    public int id => m_id;
    public TileType type => m_type;
    public Color color => m_color;
    public Tile(TileSettings settings)
    {
        onReset += KillInstance;

        if (s_dynamicPrefabs == null)
        {
            s_dynamicPrefabs = settings.dynamicPrefabs;
        }

        m_type = settings.type;
        m_color = settings.color;
        m_id = s_instanceCount++;

        s_instances.Add(this);
    }

    public static GameObject GetPrefabToSpawn()
    {
        if (s_dynamicPrefabs.Count <= 0)
        {
            Debug.LogError($"[Tile/GetPrefabToSpawn] There are no generation rates!");
            return null;
        }

        float value;
        float rateMax = 0f;
        GameObject result = null;

        foreach (PrefabSpawnRate rate in s_dynamicPrefabs)
        {
            rateMax += rate.spawnWeight;
        }

        value = UnityEngine.Random.Range(0f, 1f);
        value *= rateMax;

        for (int i = 0, minRate = 0; i < s_dynamicPrefabs.Count; i++)
        {
            int maxRate = minRate + s_dynamicPrefabs[i].spawnWeight;
            //Debug.Log($"[Tile/GetPrefabToSpawn] MinRate: {minRate} | MaxRate: {maxRate}");
            if (value >= minRate && value < maxRate)
            {
                result = s_dynamicPrefabs[i].prefab;
                break;
            }
            minRate = maxRate;
        }

        return result;
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
