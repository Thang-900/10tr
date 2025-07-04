using UnityEngine;

public class CameraPanZoom : MonoBehaviour
{
    public float zoomSpeed = 5f;
    public float minZoom = 3f;
    public float maxZoom = 20f;
    public float panSpeed = 0.5f;
    public float smoothTime = 0.2f;

    private Camera cam;
    private Vector3 lastMousePosition;
    private Vector3 panVelocity = Vector3.zero;
    private Vector3 targetPosition;

    private float targetZoom;
    private float zoomVelocity;

    void Start()
    {
        cam = Camera.main;
        targetPosition = cam.transform.position;
        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
        HandleZoom();
        HandlePan();

        // Di chuyển mượt vị trí camera
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPosition, ref panVelocity, smoothTime);

        // Phóng to/thu nhỏ mượt
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, smoothTime);
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            targetZoom -= scroll * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }
    }

    void HandlePan()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 move = new Vector3(-delta.x * panSpeed * Time.deltaTime, -delta.y * panSpeed * Time.deltaTime, 0);
            targetPosition += move;
            lastMousePosition = Input.mousePosition;
        }
    }
}
