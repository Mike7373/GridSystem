using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class TileView : MonoBehaviour
{
    [SerializeField] private TMP_Text m_type;
    [SerializeField] private TMP_Text m_id;
    [SerializeField] private TMP_Text m_gridPosition;

    private const string TYPE_LABEL = "Type: ";
    private const string ID_LABEL = "Id: ";
    private const string GRID_POSITION_LABEL = "Grid Position: ";

    private Canvas m_canvas;
    private static TileView s_instance;
    public static TileView Instance => s_instance;

    private void Awake()
    {
        if (s_instance != null)
        {
            Debug.LogWarning($"[TileView/Awake] Too much TileView objects in the scene, check your hierarchy.");
            Destroy(gameObject);
            return;
        }
        s_instance = this;
        m_canvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        TileComponent.onTileClicked += Show;
    }
    private void Start()
    {
        Initialize();
        Hide();
    }

    public void Initialize()
    {
        m_type.text = "TYPE";
        m_id.text = "# ID";
        m_gridPosition.text = "(X, Y)";
    }

    /// <summary>
    /// Hide the canvas.
    /// </summary>
    public static void Hide()
    {
        Instance.m_canvas.enabled = false;
    }
    /// <summary>
    /// Show the canvas.
    /// </summary>
    public static void Show()
    {
        Instance.m_canvas.enabled = true;
    }
    /// <summary>
    /// Show the canvas updating the data inside it.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    /// <param name="gridPosition"></param>
    public static void Show(Tile tile)
    {
        string position = $"{Map.GetGridPositionFromId(tile.id).x.ToString()}, {Map.GetGridPositionFromId(tile.id).y.ToString()}";
        Instance.m_type.text = TYPE_LABEL + tile.type.ToString();
        Instance.m_id.text = ID_LABEL + tile.id.ToString();
        Instance.m_gridPosition.text = GRID_POSITION_LABEL + position;
        Show();
    }
}
