using Pathfinding;
using UnityEngine;

public class BulletOnEven : MonoBehaviour
{
    public Transform firePoint;

    public int currentBulletCount = 0;

    private AIMoveToSafeAtkCheckRange atkCheck;

    void Start()
    {
        atkCheck = GetComponentInParent<AIMoveToSafeAtkCheckRange>();
        currentBulletCount = 0;

        if (firePoint == null)
            Debug.LogWarning(message: "firePoint chưa được gán!");
    }

    public void Fire()
    {
        // Kiểm tra điều kiện
        if (firePoint == null || PoolBomBullet.Instance == null)
        {
            Debug.LogWarning("firePoint hoặc PoolBomBullet.Instance bị thiếu.");
            return;
        }

        if (atkCheck == null || atkCheck.closestEnemy == null)
        {
            Debug.LogWarning("Không tìm thấy enemy để bắn.");
            return;
        }
        if (firePoint == null || PoolBomBullet.Instance == null)
        {
            Debug.LogWarning("firePoint hoặc PoolBomBullet.Instance bị thiếu.");
            return;
        }

        // Lấy mục tiêu
        Vector2 targetPos = atkCheck.closestEnemy.position;

        GameObject bullet = PoolBomBullet.Instance.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.identity;

        bullet.GetComponent<Bullet>().Initialize(targetPos);
        bullet.GetComponent<Bullet>().enemyTag = atkCheck.targetLayer;
        currentBulletCount++;
    }

    public void ReloadBulletOnEven()
    {
        currentBulletCount = 0;
        Debug.Log("Đã nạp lại đầy đạn o vi tri "+gameObject.name);
    }
}
