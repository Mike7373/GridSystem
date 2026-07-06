using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private Vector2Int _chunkSize = new Vector2Int(5,5);
    public Vector2Int ChunkSize { get { return _chunkSize; } set { _chunkSize = value; } }

    [Header("Tile Settings")]
    [SerializeField] private float _outerSize = 1f;
    [SerializeField] private float _innerSize = 0f;
    [SerializeField] private float _height = 1f;
    [SerializeField] private bool _isFlatTopped;
    [SerializeField] private float _hexDistance = 0.01f;
    [SerializeField] private Material _mainMaterial;
    [SerializeField] private Material _borderMaterial;
    [SerializeField] private ChunkSettings _tileDataRates;

    private void OnEnable()
    {
        LayoutGrid();
    }
    private void OnValidate()
    {
        if (Application.isPlaying && isActiveAndEnabled)
        {
            LayoutGrid();
        }
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
        ClearGrid();

        for (int z = 0; z < _chunkSize.y; z++)
        {
            for (int x = 0; x < _chunkSize.x; x++)
            {
                GameObject hex = new GameObject($"Hex {z},{x}");
                GameObject tile = new GameObject("Tile", typeof(HexTileRenderer));
                GameObject border = new GameObject("Border", typeof(HexTileRenderer));

                HexTileRenderer tileRenderer = tile.GetComponent<HexTileRenderer>();
                HexTileRenderer borderRenderer = border.GetComponent<HexTileRenderer>();

                hex.transform.position = GetPositionForHexFromCoordinate(new Vector2Int(x, z));
                border.transform.position = new Vector3(0, _height, 0);
                
                tileRenderer.SetTileDataRates(_tileDataRates);
                tileRenderer.SetupHexComponents(_outerSize, _innerSize, _height, _mainMaterial, _hexDistance, true, _isFlatTopped);
                borderRenderer.SetupHexComponents(_outerSize, _outerSize - _outerSize / 10, .1f, _borderMaterial, _hexDistance, false, _isFlatTopped);

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
        float size = _outerSize + _hexDistance;

        if (!_isFlatTopped)
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