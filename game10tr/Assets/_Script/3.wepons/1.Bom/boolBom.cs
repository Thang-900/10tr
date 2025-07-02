using System.Collections.Generic;
using UnityEngine;

public class PoolBom : MonoBehaviour
{
    public static PoolBom Instance;
    public GameObject bombPrefab;
    public int poolSize = 10;

    private Queue<GameObject> bombPool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bombPrefab);
            obj.SetActive(false);
            bombPool.Enqueue(obj);
        }
    }

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

}
