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

        HexTileRenderer.tileDataRates = tileDataRates;

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject hex = new GameObject($"Hex {y},{x}");
                GameObject tile = new GameObject("Tile", typeof(HexTileRenderer));
                GameObject border = new GameObject("Border", typeof(HexTileRenderer));

                hex.transform.position = GetPositionForHexFromCoordinate(new Vector2Int(x, y));
                border.transform.position = new Vector3(0, height, 0);

                SetupHexComponents(tile, outerSize, innerSize, height, mainMaterial, hexDistance, true);
                SetupHexComponents(border, outerSize, outerSize - outerSize / 10, .1f, borderMaterial, hexDistance);

                tile.transform.SetParent(hex.transform, false);
                border.transform.SetParent(hex.transform, false);
                hex.transform.SetParent(transform, true);
            }
        }
    }

    private void SetupHexComponents(GameObject obj, float outerSize, float innerSize, float height, Material material, float hexDistance, bool isMain = false)
    {
        HexTileRenderer renderer = obj.GetComponent<HexTileRenderer>();
        renderer.isFlatTopped = isFlatTopped;
        renderer.outerSize = outerSize;
        renderer.innerSize = innerSize;
        renderer.height = height;
        renderer.isMainTile = isMain;
        renderer.SetMaterial(material);
        renderer.DrawMesh();
        renderer.outerSize = hexDistance;
    }

    private Vector3 GetPositionForHexFromCoordinate(Vector2Int coordinate)
    {
        int column = coordinate.x;
        int row = coordinate.y;
        float width;
        float height;
        float xPosition;
        float yPosition;
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
            yPosition = row * verticalDistance;

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
            yPosition = row * verticalDistance - offset;
        }

        return new Vector3(xPosition, 0, -yPosition);
    }

    [Serializable]
    public struct TileRate
    {
        public TileData tileData;
        [Range(0, 100)] public int spawnWeigth;
    }

}
