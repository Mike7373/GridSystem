using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HexGridLayout : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2Int gridSize;

    [Header("Tile Settings")]
    public float outerSize = 1f;
    public float innerSize = 0f;
    public float height = 1f;
    //public bool isFlatTopped;
    public float hexDistance = 0.01f;

    public Material material;

    private void OnEnable()
    {
        LayoutGrid();
    }
    private void OnValidate()
    {
        //if (Application.isPlaying)
        //{
        //    LayoutGrid();
        //}
    }

    private void ClearGrid()
    {
        List<Transform> children = new();
        for(int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }
        for (int i = 0; i < children.Count; i++)
        {
            Destroy(children[i].gameObject);
        }
    }

    private void LayoutGrid()
    { 
        ClearGrid();

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject tile = new GameObject($"Hex {y},{x}", typeof(HexRenderer));
                tile.transform.position = GetPositionForHexFromCoordinate(new Vector2Int(x, y));

                HexRenderer hexRenderer = tile.GetComponent<HexRenderer>();
                hexRenderer.isFlatTopped = isFlatTopped;
                hexRenderer.outerSize = outerSize;
                hexRenderer.innerSize = innerSize;
                hexRenderer.height = height;
                hexRenderer.SetMaterial(material);
                hexRenderer.DrawMesh();
                hexRenderer.outerSize = hexDistance;

                tile.transform.SetParent(transform, true);
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
            shouldOffset = (row % 2) == 0;
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
}
