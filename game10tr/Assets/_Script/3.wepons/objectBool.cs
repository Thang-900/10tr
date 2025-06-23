using System.Collections.Generic;
using UnityEngine;

public class ObjectBool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int maxSize = 30;

    private List<GameObject> pool = new List<GameObject>();

    public GameObject GetObject()
    {
        foreach (var bullet in pool)
        {
            if (!bullet.activeInHierarchy)
                return bullet;
        }

        if (pool.Count < maxSize)
        {
            GameObject newBullet = Instantiate(bulletPrefab);
            newBullet.SetActive(false);
            pool.Add(newBullet);
            return newBullet;
        }

        return null; // Hết đạn và không tạo thêm nữa
    }
}
