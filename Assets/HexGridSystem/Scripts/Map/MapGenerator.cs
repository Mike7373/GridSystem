using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    [SerializeField] private MapSettings m_mapSettings;
    [SerializeField] private Transform m_parent;
    [SerializeField] private Camera m_camera;

    private Map m_map;

    private void Start()
    {
        Generate();
    }
    public void Generate()
    {
        if (m_parent == null)
        {
            Debug.LogWarning($"[MapGenerator/Generate] There is no parent object set for tile instances. Generated objects will be free in the scene.");
        }

        if (m_mapSettings == null)
        {
            Debug.LogError("[MapGenerator/Generate] MapSettings is not assigned on MapGenerator.", this);
            return;
        }

        m_map = new Map(m_mapSettings);

        for (int z = 0; z < m_map.mapSize.y; z++)
        {
            for (int x = 0; x < m_map.mapSize.x; x++)
            {
                Tile tile = Tile.GetTileById(m_map.grid[new Vector2Int(x, z)]);

                //GameObject hex = new GameObject($"Hex {z},{x}");
                GameObject tileObj = Instantiate(Tile.defaultPrefab);
                TileComponent tileComponent = tileObj.GetComponent<TileComponent>();
                Vector3 tileTransformPosition = GetPositionForHexFromCoordinate(new Vector2Int(x, z));

                tileObj.name = $"[{x},{x}] {tile.type} tile";
                tileObj.transform.position = tileTransformPosition;
                tileComponent.Initialize(tile.color, tileTransformPosition);

                if (m_parent != null)
                    tileObj.transform.SetParent(m_parent);
            }
        }

        SetCameraPosition();
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
        float size = 1.01f; //Switch with magic number

        if (m_mapSettings.isTileTopFlat)
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

    //Update and improve this method, now it works but it's not correct
    private void SetCameraPosition()
    {
        if (m_camera == null)
        {
            Debug.LogWarning($"[MapGenerator/Generate] Camera field is not assigner. Main camera will be used for the starting view,");
            m_camera = Camera.main;
        }

        Vector3 newPosition = new Vector3((m_map.mapSize.x * Mathf.Sqrt(3) * 1.01f) / 2, m_camera.transform.position.y, -(m_map.mapSize.y * Mathf.Sqrt(3) * 1.01f) / 2);

        m_camera.transform.position = newPosition;
    }
}
