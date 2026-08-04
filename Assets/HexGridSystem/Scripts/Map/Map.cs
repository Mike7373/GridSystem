using System;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private Tile[,] tiles;
    private List<TileGenerationRate> generationRates = new();
    private int seed;
    public Map(Vector2Int mapSize, List<TileGenerationRate> generationRates, string seed)
    {
        tiles = new Tile[mapSize.x, mapSize.y];
        this.generationRates = generationRates;
        this.seed = ConvertSeed(seed);
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                Debug.Log($"[Map] Generating tile at position [{x}][{y}]");
                //tiles[x, y] = new Tile(x, y);
            }
        }
    }

    //private Tile GetTileSettings(int xPos, int yPos)
    //{
    //    float value = Mathf.PerlinNoise(xPos + seed, yPos + seed);
        
    //}

    private int ConvertSeed(string seed)
    {
        string newSeed = "";
        foreach (char c in seed)
        {
            string oldSeed = newSeed;
            newSeed += Convert.ToUInt32(c).ToString();
            if(int.Parse(newSeed) >= int.MaxValue)
            {
                return int.Parse(oldSeed);
            }
        }
        return int.Parse(newSeed);
    }
}