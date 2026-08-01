using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HexTileBody : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetColor(Color color)
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", color);
        _meshRenderer.SetPropertyBlock(block);
    }
}
