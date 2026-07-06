using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Generation Settings")]
    [Tooltip("Map size in chunk")]
    [SerializeField] private Vector2Int mapSize;
    [Header("Chunk Generation Settings")]
    [SerializeField] private Vector2Int chunkSize;
    [SerializeField] private List<ChunkGenerationRate> chunkGenerationData;
    //[Header("Tile Generation Settings")]
    //[SerializeField] private 

    #region Unity Functions
    private void OnEnable()
    {

    }

    private void OnValidate()
    {

    }
    #endregion

    //private Vector3 GetPositionForHexFromCoordinate(Vector2Int coordinate)
    //{
    //    int column = coordinate.x;
    //    int row = coordinate.y;
    //    float width;
    //    float height;
    //    float xPosition;
    //    float zPosition;
    //    bool shouldOffset;
    //    float horizontalDistance;
    //    float verticalDistance;
    //    float offset;
    //    float size = _outerSize + _hexDistance;

    //    if (!_isFlatTopped)
    //    {
    //        shouldOffset = (row % 2) == 0;
    //        width = Mathf.Sqrt(3) * size;
    //        height = 2f * size;

    //        horizontalDistance = width;
    //        verticalDistance = height * (3f / 4f);

    //        offset = shouldOffset ? width / 2 : 0;

    //        xPosition = column * horizontalDistance + offset;
    //        zPosition = row * verticalDistance;

    //    }
    //    else
    //    {
    //        shouldOffset = (column % 2) == 0;
    //        width = 2f * size;
    //        height = Mathf.Sqrt(3) * size;

    //        horizontalDistance = width * (3f / 4f);
    //        verticalDistance = height;

    //        offset = shouldOffset ? height / 2 : 0;
    //        xPosition = column * horizontalDistance;
    //        zPosition = row * verticalDistance - offset;
    //    }

    //    return new Vector3(transform.position.x + xPosition, 0, transform.position.z + (-zPosition));
    //}

    [Serializable]
    public struct ChunkGenerationRate
    {
        public ChunkSettings chunkData;
        [Min(0)] public int spawnWeight;
    }
}
