using System;
using UnityEngine;

[Serializable]
public struct TileGenerationRate
{
    public TileSettings settings;
    [Min(0)] public int spawnWeight;
}