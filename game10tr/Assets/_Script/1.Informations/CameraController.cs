using UnityEngine;

public class CameraPanZoom : MonoBehaviour
{
    public float zoomSpeed = 5f;
    public float minZoom = 3f;
    public float maxZoom = 20f;
    public float panSpeed = 0.5f;

    private Camera cam;
    private Vector3 lastMousePosition;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleZoom();
        HandlePan();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float newSize = cam.orthographicSize - scroll * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    }

    void HandlePan()
    {
        if (Input.GetMouseButtonDown(2)) // chuột giữa
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 move = new Vector3(-delta.x * panSpeed * Time.deltaTime, -delta.y * panSpeed * Time.deltaTime, 0);
            cam.transform.Translate(move);
            lastMousePosition = Input.mousePosition;
        }
    }
}
