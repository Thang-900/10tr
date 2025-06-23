using UnityEngine;

public class BulletMover : MonoBehaviour
{
    private Vector3 targetPosition;
    private float speed;
    private bool isMoving = false;

    public void SetTarget(Vector3 target, float bulletSpeed)
    {
        targetPosition = target;
        speed = bulletSpeed;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if ((transform.position - targetPosition).sqrMagnitude < 0.01f)
        {
            isMoving = false;
            gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        isMoving = false;
    }
}
