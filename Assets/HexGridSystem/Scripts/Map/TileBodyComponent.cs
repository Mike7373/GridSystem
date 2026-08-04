using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TileBodyComponent : MonoBehaviour
{
    private MeshRenderer m_meshRenderer;

    private void Awake()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetColor(Color color)
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", color);
        m_meshRenderer.SetPropertyBlock(block);
    }
}
