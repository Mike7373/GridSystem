using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChunkData", menuName = "Map Generation/Chunk Data")]
public class ChunkSettings : ScriptableObject
{
    public string label;
    public List<TileGenerationRate> tileGenerationData;

    private void Awake()
    {
        label = name;
    }

    [Serializable]
    public struct TileGenerationRate
    {
        public TileSettings tileData;
        [Min(0)] public int spawnWeight;
    }
}
