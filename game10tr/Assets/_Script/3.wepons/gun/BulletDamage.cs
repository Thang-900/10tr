using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public ObjectBool bulletPool;                 // Script quản lý Pool
    public float bulletSpeed = 10f;
    public float bulletMaxDistance = 10f;
    public GameObject startPosition;
    public float fireCooldown = 0.3f;             // Khoảng thời gian giữa các phát bắn

    private float lastFireTime = -999f;

    void Update()
    {
        if (Input.GetMouseButton(0))              // Giữ chuột trái để bắn liên tục
        {
            if (Time.time - lastFireTime >= fireCooldown)
            {
                Shoot();
                lastFireTime = Time.time;
            }
        }
    }

    void Shoot()
    {
        Vector3 targetPosition = TakeMousePosition();
        Vector3 origin = startPosition.transform.position;

        // Giới hạn khoảng cách bay của viên đạn
        if ((targetPosition - origin).magnitude > bulletMaxDistance)
        {
            targetPosition = origin + (targetPosition - origin).normalized * bulletMaxDistance;
        }

        GameObject bullet = bulletPool.GetObject();
        if (bullet != null)
        {
            bullet.transform.position = origin;
            bullet.transform.rotation = Quaternion.identity;
            bullet.SetActive(true);

            // Gửi vị trí đích đến viên đạn
            BulletMover mover = bullet.GetComponent<BulletMover>();
            if (mover != null)
            {
                mover.SetTarget(targetPosition, bulletSpeed);
            }
        }
    }

    Vector3 TakeMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0f);
    }
}
