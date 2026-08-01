using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HexTileBorder : MonoBehaviour
{
    [Header("Border Style Settings")]
    [SerializeField] private Color _hoverColor = Color.white;
    [SerializeField] private Color _selectColor = Color.yellow;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        Hide();
    }

    public void Hover()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", _hoverColor);
        _meshRenderer.SetPropertyBlock(block);
    }

    public void Select()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", _selectColor);
        _meshRenderer.SetPropertyBlock(block);
    }
    public void Hide()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", new Color(0, 0, 0, 0));
        _meshRenderer.SetPropertyBlock(block);
    }
}
