using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBehaviour : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField, Range(0, 3)] private float m_zoomSensitivity = 1f;
    [Tooltip("How many time the player can <b>zoom in</b> from the base camera position")]
    [SerializeField, Min(0)] private int m_maxZoomIn = 2;
    [Tooltip("How many time the player can <b>zoom out</b> from the base camera position")]
    [SerializeField, Min(0)] private int m_maxZoomOut = 5;
    [Header("Movement")]
    [SerializeField, Range(0, 3)] private float m_dragSensitivity = 1f;

    private Camera m_camera;
    private bool m_isDragging = false;
    private Vector3 m_originMousePosition;
    private const int DRAG_MULTIPLIER = 10;
    private int m_currentScroll = 0;

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
    }

    private void Update()
    {
        TryZoom();
        SetDragging();
        if (!m_isDragging || !Input.GetMouseButton(2)) return;
        Move();
    }

    private void SetDragging()
    {
        if (Input.GetMouseButtonDown(2))
        {
            m_originMousePosition = Input.mousePosition;
            m_isDragging = true;
            return;
        }
        if (Input.GetMouseButtonUp(2))
        {
            m_isDragging = false;
        }
    }

    private void Move()
    {
        Vector3 deltaScreen = Input.mousePosition - m_originMousePosition;
        Vector3 deltaViewport = m_camera.ScreenToViewportPoint(deltaScreen);
        Vector3 moveVector = new Vector3(-deltaViewport.x * m_dragSensitivity * DRAG_MULTIPLIER, 0, -deltaViewport.y * m_dragSensitivity * DRAG_MULTIPLIER);

        transform.position += moveVector;
        m_originMousePosition = Input.mousePosition;
    }
    /// <summary>
    /// Move the camera on the Z axes - 
    /// zoomDirection: true = zoom in | false = zoom out
    /// </summary>
    /// <param name="zoomDirection"></param>
    private void Zoom(bool zoomDirection)
    {
        int direction = zoomDirection ? 1 : -1;
        Vector3 zoomVector = transform.forward * direction * m_zoomSensitivity;

        transform.position += zoomVector;
    }

    private void TryZoom()
    {
        float scrollValue = Input.mouseScrollDelta.y;
        if (scrollValue == 0) return;

        if (scrollValue > 0)
        {
            if (m_currentScroll >= m_maxZoomIn) return;

            m_currentScroll++;
            Zoom(true);
            return;
        }
        if (m_currentScroll <= -m_maxZoomOut) return;

        m_currentScroll--;
        Zoom(false);
    }
}