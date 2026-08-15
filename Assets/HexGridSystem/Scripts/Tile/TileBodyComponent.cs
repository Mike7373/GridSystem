using UnityEngine;

[RequireComponent(typeof(MeshRenderer),typeof(MeshCollider))]
public class TileBodyComponent : MonoBehaviour
{
    private MeshRenderer m_meshRenderer;
    public delegate void MouseEvent();
    
    public event MouseEvent onClick;
    public event MouseEvent onFocus;
    public event MouseEvent onUnfocus;

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
    private void OnMouseDown()
    {
        onClick?.Invoke();
    }
    private void OnMouseEnter()
    {
        onFocus?.Invoke();
    }
    private void OnMouseExit()
    {
        onUnfocus?.Invoke();
    }


}
