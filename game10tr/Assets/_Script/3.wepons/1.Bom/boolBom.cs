using System.Collections.Generic;
using UnityEngine;

public class PoolBomBullet : MonoBehaviour
{
    public static PoolBomBullet Instance;

    public GameObject bombPrefab;
    public GameObject bulletPrefab;
    public int poolSize = 10;

    private Queue<GameObject> bombPool = new Queue<GameObject>();
    private Queue<GameObject> bulletPool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Khởi tạo bombPool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bombPrefab);
            obj.SetActive(false);
            bombPool.Enqueue(obj);
        }

        // Khởi tạo bulletPool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            bulletPool.Enqueue(obj);
        }
    }

    // ==== Bomb ====
    public GameObject GetBomb()
    {
        if (bombPool.Count > 0)
        {
            GameObject obj = bombPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(bombPrefab);
            return obj;
        }
    }

    public void ReturnBomb(GameObject obj)
    {
        obj.SetActive(false);
        bombPool.Enqueue(obj);
    }

    // ==== Bullet ====
    public GameObject GetBullet()
    {
        if (bulletPool.Count > 0)
        {
            GameObject obj = bulletPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(bulletPrefab);
            return obj;
        }
    }

    public void ReturnBullet(GameObject obj)
    {
        obj.SetActive(false);
        bulletPool.Enqueue(obj);
    }

    public Transform GetNearestBomStore(Vector3 fromPosition)
    {
        GameObject[] bomStores = GameObject.FindGameObjectsWithTag("BomStore");
        float minDist = Mathf.Infinity;
        Transform nearest = null;

        foreach (GameObject store in bomStores)
        {
            float dist = Vector3.Distance(fromPosition, store.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = store.transform;
            }
        }

        return nearest;
    }
    public Transform GetNearestEnemyBomStore(Vector3 fromPosition)
    {
        GameObject[] bomStores = GameObject.FindGameObjectsWithTag("EnemyBomStore");
        float minDist = Mathf.Infinity;
        Transform nearest = null;

        foreach (GameObject store in bomStores)
        {
            float dist = Vector3.Distance(fromPosition, store.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = store.transform;
            }
        }

        return nearest;
    }
    public Transform GetNearestBulletStore(Vector3 fromPosition)
    {
        GameObject[] bomStores = GameObject.FindGameObjectsWithTag("BulletStore");
        float minDist = Mathf.Infinity;
        Transform nearest = null;

        foreach (GameObject store in bomStores)
        {
            float dist = Vector3.Distance(fromPosition, store.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = store.transform;
            }
        }

        return nearest;
    }
    public Transform GetNearestEnemyBulletStore(Vector3 fromPosition)
    {
        GameObject[] bomStores = GameObject.FindGameObjectsWithTag("EnemyBulletStore");
        float minDist = Mathf.Infinity;
        Transform nearest = null;

        foreach (GameObject store in bomStores)
        {
            float dist = Vector3.Distance(fromPosition, store.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = store.transform;
            }
        }

        return nearest;
    }
}
