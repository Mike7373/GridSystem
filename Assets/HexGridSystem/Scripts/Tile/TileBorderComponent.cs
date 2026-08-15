using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TileBorderComponent : MonoBehaviour
{
    [Header("Border Style Settings")]
    [SerializeField] private Color m_hoverColor = Color.white;
    [SerializeField] private Color m_selectColor = Color.yellow;

    private MeshRenderer _meshRenderer;
    private static TileBorderComponent selectedTile;

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

    public void TryFocus()
    {
        if(selectedTile != this)
        {
            Focus();
        }
    }
    public void Focus()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", m_hoverColor);
        _meshRenderer.SetPropertyBlock(block);
    }

    public void TrySelect()
    {
        if (selectedTile != this)
        {
            Select();
        }
    }
    public void Select()
    {
        if (selectedTile != null)
        {
            selectedTile.Hide();
        }
        selectedTile = this;
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", m_selectColor);
        _meshRenderer.SetPropertyBlock(block);
    }
    public void Hide()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", new Color(0, 0, 0, 0));
        _meshRenderer.SetPropertyBlock(block);
    }
    public void TryHide()
    {
        if (selectedTile != this)
        {
            Hide();
        }
    }
}
