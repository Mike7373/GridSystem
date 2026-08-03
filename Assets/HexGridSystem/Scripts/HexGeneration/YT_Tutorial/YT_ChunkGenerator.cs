using System.Collections.Generic;
using UnityEngine;
using static ChunkSettings;

public class YT_ChunkGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    private Vector2Int _chunkSize;
    public Vector2Int ChunkSize { get { return _chunkSize; } set { _chunkSize = value; } }

    [Header("Tile Settings")]

    [SerializeField] private ChunkSettings _chunkSettings;

    private void Awake()
    {
        _chunkSize = YT_MapGenerator.ChunkSize;   
    }
    private void OnValidate()
    {
        if (Application.isPlaying && isActiveAndEnabled && _chunkSettings != null)
        {
            LayoutGrid();
        }
    }

    public void Initialize()
    {
        LayoutGrid();
    }

    private void ClearGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void LayoutGrid()
    {
        if (_chunkSettings == null)
        {
            Debug.LogError("ChunkSettings is not assigned on ChunkGenerator.", this);
            return;
        }

        ClearGrid();

        for (int z = 0; z < _chunkSize.y; z++)
        {
            for (int x = 0; x < _chunkSize.x; x++)
            {
                GameObject hex = new GameObject($"Hex {z},{x}");
                GameObject tile = new GameObject("Tile", typeof(YT_HexTileRenderer));
                GameObject border = new GameObject("Border", typeof(YT_HexTileRenderer));

                YT_HexTileRenderer tileRenderer = tile.GetComponent<YT_HexTileRenderer>();
                YT_HexTileRenderer borderRenderer = border.GetComponent<YT_HexTileRenderer>();

                hex.transform.position = GetPositionForHexFromCoordinate(new Vector2Int(x, z));
                border.transform.position = new Vector3(0, YT_MapGenerator.TileHeight, 0);
                
                tileRenderer.SetTileDataSettings(GetGenTileData());
                tileRenderer.SetupHexComponents(YT_MapGenerator.TileOuterSize, YT_MapGenerator.TileInnerSize, YT_MapGenerator.TileHeight, YT_MapGenerator.TileMainMaterial, YT_MapGenerator.TileDeltaDistance, true, YT_MapGenerator.IsTileTopFlat);
                borderRenderer.SetupHexComponents(YT_MapGenerator.TileOuterSize, YT_MapGenerator.TileOuterSize - YT_MapGenerator.TileOuterSize / 10, .1f, YT_MapGenerator.TileBorderMaterial, YT_MapGenerator.TileDeltaDistance, false, YT_MapGenerator.IsTileTopFlat);

                tile.transform.SetParent(hex.transform, false);
                border.transform.SetParent(hex.transform, false);
                hex.transform.SetParent(transform, true);
            }
        }
    }

    private Vector3 GetPositionForHexFromCoordinate(Vector2Int coordinate)
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
        float size = YT_MapGenerator.TileOuterSize + YT_MapGenerator.TileDeltaDistance;

        if (!YT_MapGenerator.IsTileTopFlat)
        {
            shouldOffset = (row % 2) == 0;
            width = Mathf.Sqrt(3) * size;
            height = 2f * size;

            horizontalDistance = width;
            verticalDistance = height * (3f / 4f);

            offset = shouldOffset ? width / 2 : 0;

            xPosition = column * horizontalDistance + offset;
            zPosition = row * verticalDistance;

        }
        else
        {
            shouldOffset = (column % 2) == 0;
            width = 2f * size;
            height = Mathf.Sqrt(3) * size;

            horizontalDistance = width * (3f / 4f);
            verticalDistance = height;

            offset = shouldOffset ? height / 2 : 0;
            xPosition = column * horizontalDistance;
            zPosition = row * verticalDistance - offset;
        }

        return new Vector3(transform.position.x + xPosition, 0, transform.position.z + (-zPosition));
    }

    public void SetMapSettings(ChunkSettings settings)
    {
        _chunkSettings = settings;
    }
    private TileDataSettings GetGenTileData()
    {
        List<TileGenerationRate> generationData = _chunkSettings.tileGenerationData;

        if (_chunkSettings.tileGenerationData.Count <= 0)
        {
            Debug.LogWarning("[ChunkGenerator/GetGenTileData] No tiles data available!");
            return null;
        }

        int percSum = 0;
        int rand;
        List<TileGenerationRate> tiles = new List<TileGenerationRate>();
        TileDataSettings result = null;

        for (int i = 0; i < generationData.Count; i++)
        {
            tiles.Add(generationData[i]);
            percSum += generationData[i].spawnWeight;
        }

        rand = Random.Range(0, percSum);

        for (int i = 0, minRate = 0; i < tiles.Count; i++)
        {
            int maxRate = minRate + tiles[i].spawnWeight;
            //Debug.Log($"[ChunkGenerator/GetGenTileData] Rand: {rand} | MinRate: {minRate} | MaxRate: {maxRate} | PercSum: {percSum}");

            if (rand >= minRate && rand < maxRate)
            {
                result = tiles[i].tileData;
                break;
            }
            minRate = maxRate;
        }
        if (result == null)
        {
            Debug.LogError($"[ChunkGenerator/GetGenTileData] Result is null!");
        }
        return result;
    }
}