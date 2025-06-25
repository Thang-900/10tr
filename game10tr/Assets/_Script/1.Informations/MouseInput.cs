using UnityEngine;

public class ClickToMoveAStar : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Nhấn chuột trái
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPos.z = 0f; // vì 2D
            target.position = clickPos;
        }
    }
}
