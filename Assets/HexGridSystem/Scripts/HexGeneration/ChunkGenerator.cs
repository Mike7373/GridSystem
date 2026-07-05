using System;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2Int gridSize;

    [Header("Tile Settings")]
    [SerializeField] private float outerSize = 1f;
    [SerializeField] private float innerSize = 0f;
    [SerializeField] private float height = 1f;
    [SerializeField] private bool isFlatTopped;
    [SerializeField] private float hexDistance = 0.01f;
    [SerializeField] private Material mainMaterial;
    [SerializeField] private Material borderMaterial;
    [SerializeField] private List<TileRate> tileDataRates;

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

        for (int z = 0; z < gridSize.y; z++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject hex = new GameObject($"Hex {z},{x}");
                GameObject tile = new GameObject("Tile", typeof(HexTileRenderer));
                GameObject border = new GameObject("Border", typeof(HexTileRenderer));

                HexTileRenderer tileRenderer = tile.GetComponent<HexTileRenderer>();
                HexTileRenderer borderRenderer = border.GetComponent<HexTileRenderer>();

                hex.transform.position = GetPositionForHexFromCoordinate(new Vector2Int(x, z));
                border.transform.position = new Vector3(0, height, 0);
                
                tileRenderer.SetTileDataRates(tileDataRates);
                tileRenderer.SetupHexComponents(outerSize, innerSize, height, mainMaterial, hexDistance, true, isFlatTopped);
                borderRenderer.SetupHexComponents(outerSize, outerSize - outerSize / 10, .1f, borderMaterial, hexDistance, false, isFlatTopped);

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
        float size = outerSize + hexDistance;

        if (!isFlatTopped)
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

    [Serializable]
    public struct TileRate
    {
        public TileData tileData;
        [Range(0, 100)] public int spawnWeigth;
    }

}
