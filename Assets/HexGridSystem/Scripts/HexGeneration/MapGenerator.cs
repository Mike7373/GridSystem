using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //Singleton
    private static MapGenerator _instance;
    public static MapGenerator Instance => _instance;

    #region Inspector Data
    [Header("Map Generation Settings")]
    [Tooltip("Map size in chunk")]
    [SerializeField] private Vector2Int _mapSize;
    [Header("Chunk Generation Settings")]
    [SerializeField] private Vector2Int _chunkSize;
    [SerializeField] private List<ChunkGenerationRate> chunkGenerationData;
    [Header("Tile Generation Settings")]
    [SerializeField] private bool _isTileTopFlat = true;
    [SerializeField] private float _tileOuterSize = 1f;
    [SerializeField] private float _tileInnerSize = 0f;
    [SerializeField] private float _tileDeltaDistance = 0.1f;
    [SerializeField] private float _tileHeight = 1f;
    [SerializeField] private Material _tilemainMaterial;
    [SerializeField] private Material _tileBorderMaterial;
    #endregion

    //Chunk Properties
    public static Vector2Int ChunkSize => Instance._chunkSize;

    //Tile Properties
    public static bool IsTileTopFlat => Instance._isTileTopFlat;
    public static float TileOuterSize => Instance._tileOuterSize;
    public static float TileInnerSize => Instance._tileInnerSize;
    public static float TileDeltaDistance => Instance._tileDeltaDistance;
    public static float TileHeight => Instance._tileHeight;
    public static Material TileMainMaterial => Instance._tilemainMaterial;
    public static Material TileBorderMaterial => Instance._tileBorderMaterial;

    #region Unity Functions
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private void OnEnable()
    {
        GenerateMap();
    }

    private void OnValidate()
    {
        if (Application.isPlaying && isActiveAndEnabled)
        {
            GenerateMap();
        }
    }
    #endregion

    private void ClearMap()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void GenerateMap()
    {
        ClearMap();

        for (int z = 0; z < _mapSize.y; z++)
        {
            for (int x = 0; x < _mapSize.x; x++)
            {
                GameObject chunk = new GameObject($"Chunk {z},{x}", typeof(ChunkGenerator));
                ChunkGenerator chunkGenerator = chunk.GetComponent<ChunkGenerator>();

                chunk.transform.position = GetPositionForChunkFromCoordinate(new Vector2Int(x, z));
                chunkGenerator.SetMapSettings(GetGenChunkData());
                chunkGenerator.Initialize();

                chunk.transform.SetParent(transform, true);
            }
        }
    }

    private Vector3 GetPositionForChunkFromCoordinate(Vector2Int coordinate)
    {
        int column = coordinate.x;
        int row = coordinate.y;
        float width;
        float height;
        float xPosition;
        float zPosition;
        bool shouldOffset;
        float horizontalDistance;
        float verticalDistance;
        float offset;
        float size = _tileOuterSize + _tileDeltaDistance;

        if (!_isTileTopFlat)
        {
            shouldOffset = (row % 2) == 0;
            width = Mathf.Sqrt(3) * size * _chunkSize.x;
            height = 2f * size * _chunkSize.y;

            horizontalDistance = width;
            verticalDistance = height * (3f / 4f);

            offset = shouldOffset ? width / _chunkSize.x / 2 : 0;

            xPosition = column * horizontalDistance + offset;
            zPosition = row * verticalDistance;

        }
        else
        {
            shouldOffset = (column % 2) == 0 && (_mapSize.y % 2) == 0;
            width = 2f * size * _chunkSize.x;
            height = Mathf.Sqrt(3) * size * _chunkSize.y;

            horizontalDistance = width * (3f / 4f);
            verticalDistance = height;

            offset = shouldOffset ? (height + _chunkSize.y * Mathf.Sqrt(3)) / 2 : 0;
            xPosition = column * horizontalDistance;
            zPosition = row * verticalDistance - offset;
        }

        return new Vector3(transform.position.x + xPosition, 0, transform.position.z + (-zPosition));
    }

    [Serializable]
    public struct ChunkGenerationRate
    {
        public ChunkSettings chunkData;
        [Min(0)] public int spawnWeight;
    }

    private ChunkSettings GetGenChunkData()
    {
        if (chunkGenerationData.Count <= 0)
        {
            Debug.LogWarning("[MapGenerator/GetGenTileData] No chunk data available!");
            return null;
        }

        int percSum = 0;
        int rand;
        List<ChunkGenerationRate> tiles = new List<ChunkGenerationRate>();
        ChunkSettings result = null;

        for (int i = 0; i < chunkGenerationData.Count; i++)
        {
            tiles.Add(chunkGenerationData[i]);
            percSum += chunkGenerationData[i].spawnWeight;
        }

        rand = UnityEngine.Random.Range(0, percSum);

        for (int i = 0, minRate = 0; i < tiles.Count; i++)
        {
            int maxRate = minRate + tiles[i].spawnWeight;
            //Debug.Log($"[MapGenerator/GetGenTileData] Rand: {rand} | MinRate: {minRate} | MaxRate: {maxRate} | PercSum: {percSum}");

            if (rand >= minRate && rand < maxRate)
            {
                result = tiles[i].chunkData;
                break;
            }
            minRate = maxRate;
        }
        if (result == null)
        {
            Debug.LogError($"[MapGenerator/GetGenTileData] Result is null!");
        }
        return result;
    }
}
