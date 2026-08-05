using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private MapSettings m_mapSettings;

    private Map m_map;

    public void Generate()
    {
        m_map = new Map(m_mapSettings.mapSize, m_mapSettings.tileGenerationRate);

        if (m_mapSettings == null)
        {
            Debug.LogError("[MapGenerator/Generate] MapSettings is not assigned on MapGenerator.", this);
            return;
        }

        for (int z = 0; z < m_map.mapSize.y; z++)
        {
            for (int x = 0; x < m_map.mapSize.x; x++)
            {
                Tile tile = Tile.GetTileById(m_map.grid[new Vector2Int(z, x)]);

                //GameObject hex = new GameObject($"Hex {z},{x}");
                GameObject tileObj = Instantiate(Tile.defaultPrefab);
                tileObj.name = $"[{x},{x}] {tile.type} tile";

                tileObj.transform.position = GetPositionForHexFromCoordinate(new Vector2Int(z, x));
                border.transform.position = new Vector3(0, YT_MapGenerator.TileHeight, 0);

                //TODO: decommentare e sistemare
                //tileRenderer.SetTileDataSettings(GetGenTileData());
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
}
