using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCount : MonoBehaviour
{
    int bulletCount = 0;
    public int maxBulletCount = 10; // Số lượng đạn tối đa
    [SerializeField]private BulletOnEven bullet1, bullet2, bullet3;
    public bool outOfBullet = false;
    // Start is called before the first frame update

   

    // Update is called once per frame
    void Update()
    {
        bulletCount = bullet1.currentBulletCount +
                     bullet2.currentBulletCount +
                     bullet3.currentBulletCount;
        if (bulletCount >= maxBulletCount)
        {
            outOfBullet = true;
        }
        else
        {
            outOfBullet = false;
        }
    }
    public void ResetBulletCount()
    {
        bullet1.ReloadBulletOnEven();
        bullet2.ReloadBulletOnEven();
        bullet3.ReloadBulletOnEven();
        bulletCount = 0;
        outOfBullet = false;
        Debug.Log("Đã nạp lại đầy đạn o vi tri chinh " + gameObject.name);
    }
}
