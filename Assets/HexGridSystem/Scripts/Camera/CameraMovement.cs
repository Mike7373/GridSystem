using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float m_dragSensitivity = 1f;

    private Camera m_camera;
    private bool m_isDragging = false;
    private Vector3 m_originMousePosition;

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
    }

    private void Update()
    {
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
        Vector3 moveVector = new Vector3(-deltaViewport.x * m_dragSensitivity, 0, -deltaViewport.y * m_dragSensitivity);

        transform.position += moveVector;
        m_originMousePosition = Input.mousePosition;
    }
}
