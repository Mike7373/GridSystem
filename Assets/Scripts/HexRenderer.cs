using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class HexRenderer : MonoBehaviour
{
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;

    private List<Face> _faces;

    public Material material;
    public float innerSize = 1;
    public float outerSize = 1;
    public float height = 1;
    public bool isFlatTopped;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();

        _mesh = new Mesh();
        _mesh.name = "Hex";

        _meshFilter.mesh = _mesh;
        _meshRenderer.material = material;
    }

    private void OnEnable()
    {
        DrawMesh();
    }

    private void OnValidate()
    {
        if (Application.isPlaying && _mesh != null)
        {
            DrawMesh();
        }
    }

    public void DrawMesh()
    {
        DrawFaces();
        CombineFaces();
        _meshCollider.sharedMesh = _mesh;
        _meshCollider.convex = true;
    }

    public void SetMaterial(Material material)
    {
        _meshRenderer.material = material;
    }
    private void DrawFaces()
    {
        _faces = new List<Face>();

        //Top face
        for (int point = 0; point < 6; point++)
        {
            _faces.Add(CreateFace(innerSize, outerSize, height / 2f, height / 2f, point));
        }

        //Bottom face
        for (int point = 0; point < 6; point++)
        {
            _faces.Add(CreateFace(innerSize, outerSize, -height / 2f, -height / 2f, point, true));
        }

        //Outer face
        for (int point = 0; point < 6; point++)
        {
            _faces.Add(CreateFace(outerSize, outerSize, height / 2f, -height / 2f, point, true));
        }

        //Inner face
        for (int point = 0; point < 6; point++)
        {
            _faces.Add(CreateFace(innerSize, innerSize, height / 2f, -height / 2f, point, false));
        }
    }

    private void CombineFaces()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < _faces.Count; i++)
        {
            //Add vertices
            vertices.AddRange(_faces[i].vertices);
            uvs.AddRange(_faces[i].uvs);

            //offset triangles
            int offset = 4 * i;
            foreach (int triangle in _faces[i].triangles)
            {
                tris.Add(triangle + offset);
            }
        }

        _mesh.vertices = vertices.ToArray();
        _mesh.triangles = tris.ToArray();
        _mesh.uv = uvs.ToArray();
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
    }
    private Face CreateFace(float innerRad, float outerRad, float heightA, float heightB, int point, bool reverse = false)
    {
        Vector3 pointA = GetPoint(innerRad, heightB, point);
        Vector3 pointB = GetPoint(innerRad, heightB, point < 5 ? point + 1 : 0);
        Vector3 pointC = GetPoint(outerRad, heightA, point < 5 ? point + 1 : 0);
        Vector3 pointD = GetPoint(outerRad, heightA, point);

        Debug.Log($"Face {point}: A={pointA}, B={pointB}, C={pointC}, D={pointD}");


        List<Vector3> vertices = new List<Vector3>() { pointA, pointB, pointC, pointD };
        List<int> triangles = new List<int>() { 0, 3, 2, 2, 1, 0 };
        List<Vector2> uvs = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };


        if (reverse)
        {
            vertices.Reverse();
        }

        return new Face(vertices, triangles, uvs);
    }

    private Vector3 GetPoint(float size, float height, int index)
    {
        float angleDeg = isFlatTopped ? 60 * index : 60 * index - 30;
        float angleRad = Mathf.PI / 180 * angleDeg;

        return new Vector3(size * Mathf.Cos(angleRad), height, size * Mathf.Sin(angleRad));
    }

    private void OnDrawGizmos()
    {
        if (_faces == null) return;

        Gizmos.color = Color.red;
        foreach (var face in _faces)
        {
            foreach (var v in face.vertices)
            {
                Gizmos.DrawSphere(transform.TransformPoint(v), 0.05f);
            }
        }
    }

    public struct Face
    {
        public List<Vector3> vertices { get; private set; }
        public List<int> triangles { get; private set; }
        public List<Vector2> uvs { get; private set; }

        public Face(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
        {
            this.vertices = vertices;
            this.triangles = triangles;
            this.uvs = uvs;
        }
    }

}
